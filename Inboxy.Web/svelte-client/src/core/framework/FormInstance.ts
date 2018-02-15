import * as umf from "uimf-core";
import { InputFieldValue } from "./InputFieldValue";
import { OutputFieldValue } from "./OutputFieldValue";
import { InputController } from "./InputController";
import { ControlRegister } from "./ControlRegister";
import { FormEventArguments, FormResponseEventArguments } from "./FormEventArguments";
import { UmfApp } from "./UmfApp";
import { FormResponse } from "uimf-core";

export class FormInstance {
    public readonly metadata: umf.FormMetadata;
    public outputs: Array<OutputFieldValue> = [];
    public inputs: Array<InputController<any>> = [];

    constructor(metadata: umf.FormMetadata, controlRegister: ControlRegister) {
        this.metadata = metadata;
        this.inputs = controlRegister.createInputControllers(this.metadata.inputFields);
    }

    private enforceCanPostOnLoad() {
        // If user is trying to auto-submit a form which is not enabled for `PostOnLoad`.
        if (!this.metadata.postOnLoad) {
            throw new Error(`Invalid invocation of form '${this.metadata.id}'. Form cannot be auto-posted, because *PostOnLoad* is set to false.`);
        }
    }

    async allRequiredInputsHaveData(asPostOnLoad: boolean): Promise<boolean> {
        if (asPostOnLoad) {
            this.enforceCanPostOnLoad();
        }

        let formData = await this.getFormData(asPostOnLoad);

        return formData != null;
    }

    async submit(app: UmfApp, asPostOnLoad: boolean, args: any): Promise<FormResponse> {
        if (asPostOnLoad) {
            this.enforceCanPostOnLoad();
        }

        let formData = await this.getFormData(asPostOnLoad);

        // If not all required inputs are filled.
        if (formData == null) {
            throw new Error(`Form '${this.metadata.id}' cannot be submitted, because some required input fields do not have values.`);
        }

        await this.fire("form:posting", new FormEventArguments(app));

        let response = await app.server.postForm(this.metadata.id, formData);
        await this.fire("form:responseReceived", new FormResponseEventArguments(app, response));

        this.setOutputFieldValues(response);

        // Null response is treated as a server-side error.
        if (response == null) {
            throw new Error(`Received null response.`);
        }

        await app.runFunctions(response.metadata.functionsToRun);
        app.handleResponse(response, this, args);

        await this.fire("form:responseHandled", new FormResponseEventArguments(app, response));

        return response;
    }

    initializeInputFields(data: any) {
        var promises = [];

        for (let fieldMetadata of this.inputs) {
            let value = null;

            if (data != null) {
                for (let prop in data) {
                    if (data.hasOwnProperty(prop) && prop.toLowerCase() == fieldMetadata.metadata.id.toLowerCase()) {
                        value = data[prop];
                        break;
                    }
                }
            }

            promises.push(fieldMetadata.init(value));
        }

        return Promise.all(promises);
    }

    setInputFields(data: any) {
        for (let field of this.inputs) {
            field.value = data[field.metadata.id];
        }
    }

    getSerializedInputValues(): Promise<any> {
        var data = {};
        var promises = [];

        for (let input of this.inputs) {
            var promise = input.serialize().then(t => {
                // Don't include inputs without values, because we only
                // want to serialize "non-default" values.
                if (t.value != null && t.value != "") {
                    data[input.metadata.id] = t.value;
                }
            });

            promises.push(promise);
        }

        return Promise.all(promises).then(() => data);
    }

    getSerializedInputValuesFromObject(value: any): any {
        var data = {};

        var normalizedObject = {};
        for (let prop in value) {
            if (value.hasOwnProperty(prop)) {
                normalizedObject[prop.toLowerCase()] = value[prop];
            }
        }

        for (let input of this.inputs) {
            var valueAsString = input.serializeValue(normalizedObject[input.metadata.id.toLowerCase()]);

            // Don't include inputs without values, because we only
            // want to serialize "non-default" values.
            if (valueAsString != null && valueAsString != "") {
                data[input.metadata.id] = valueAsString;
            }
        }

        return data;
    }

    static getOutputFieldValues(outputFieldsMetadata: umf.OutputFieldMetadata[], response: any): Array<OutputFieldValue> {
        var fields = Array<OutputFieldValue>();
        
        var normalizedResponse = FormInstance.getNormalizedObject(response);

        for (let field of outputFieldsMetadata) {
            var normalizedId = field.id.toLowerCase();

            fields.push({
                metadata: field,
                data: normalizedResponse[normalizedId]
            });
        }

        fields.sort((a: OutputFieldValue, b: OutputFieldValue) => {
            return a.metadata.orderIndex - b.metadata.orderIndex;
        });

        return fields;
    }

    setOutputFieldValues(response: umf.FormResponse) {
        if (response == null) {
            this.outputs = [];
            return;
        }

        var fields = Array<OutputFieldValue>();

        var normalizedResponse = FormInstance.getNormalizedObject(response);

        for (let field of this.metadata.outputFields) {
            fields.push({
                metadata: field,
                data: normalizedResponse[field.id.toLowerCase()]
            });
        }

        fields.sort((a: OutputFieldValue, b: OutputFieldValue) => {
            return a.metadata.orderIndex - b.metadata.orderIndex;
        });

        this.outputs = fields;
    }

    async handleEvent(eventName: string, eventMetadata: umf.EventHandlerMetadata, parameters: FormEventArguments): Promise<void> {
        await this.fire(eventName, parameters);
    }

    private async fire(eventName: string, parameters: FormEventArguments): Promise<void> {
        var promises = [];

        // Run input event handlers.
        for (let input of this.inputs) {
            if (input.metadata.eventHandlers != null) {
                for (let eventHandlerMetadata of input.metadata.eventHandlers) {
                    if (eventHandlerMetadata.runAt === eventName) {
                        let handler = parameters.app.controlRegister.inputFieldEventHandlers[eventHandlerMetadata.id];
                        if (handler == null) {
                            throw new Error(`Could not find input event handler '${eventHandlerMetadata.id}'.`);
                        }

                        let promise = handler.run(input, eventHandlerMetadata, parameters);
                        promises.push(promise);
                    }
                }
            }
        }

        // Run output event handlers.
        for (let output of this.outputs) {
            if (output.metadata.eventHandlers != null) {
                for (let eventHandlerMetadata of output.metadata.eventHandlers) {
                    if (eventHandlerMetadata.runAt === eventName) {
                        let handler = parameters.app.controlRegister.outputFieldEventHandlers[eventHandlerMetadata.id];
                        if (handler == null) {
                            throw new Error(`Could not find output event handler '${eventHandlerMetadata.id}'.`);
                        }

                        let promise = handler.run(output, eventHandlerMetadata, parameters);
                        promises.push(promise);
                    }
                }
            }
        }

        // Run form event handlers.
        this.metadata.eventHandlers
            .filter(t => t.runAt === eventName)
            .forEach(t => {
                let handler = parameters.app.controlRegister.formEventHandlers[t.id];
                if (handler == null) {
                    throw new Error(`Could not find form event handler '${t.id}'.`);
                }

                let promise = handler.run(this, t, parameters);
                promises.push(promise);
            });


        await Promise.all(promises);
    }

    private async getFormData(asPostOnLoad: boolean): Promise<any> {
        var data = {};
        var promises = [];
        var hasRequiredMissingInput = false;

        for (let input of this.inputs) {
            var promise = input.getValue().then(value => {
                data[input.metadata.id] = value;

                if (input.metadata.required && (value == null || (typeof (value) === "string" && value == ""))) {
                    hasRequiredMissingInput = true;
                }
            });

            promises.push(promise);
        }

        await Promise.all(promises);

        var skipValidation =
            !this.metadata.postOnLoadValidation &&
            this.metadata.postOnLoad &&
            // if initialization of the form, i.e. - first post.
            asPostOnLoad;


        // If not all required inputs were entered, then do not post.
        if (hasRequiredMissingInput &&
            !skipValidation) {
            return null;
        }

        return data;
    }

    private static getNormalizedObject(response: umf.FormResponse): any {
        var normalizedResponse = {};
        for (let field in response) {
            if (response.hasOwnProperty(field) && field !== "metadata") {
                normalizedResponse[field.toLowerCase()] = response[field];
            }
        }

        return normalizedResponse;
    }
}