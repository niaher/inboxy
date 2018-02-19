import {
	FormInstance,
	FormEventHandler,
	FormEventArguments
} from "../../framework/index";
import { EventHandlerMetadata } from "uimf-core";
import { ActionListEventArguments } from "core-ui/outputs/ActionListEventArguments";

/**
 * Reloads form after action.
 */
export class ReloadFormAfterAction extends FormEventHandler {
	run(form: FormInstance, eventHandlerMetadata: EventHandlerMetadata, args: ActionListEventArguments): Promise<void> {
		var isTopLevelForm = args.form.get("parent") == null;
		
		if (isTopLevelForm && eventHandlerMetadata.customProperties.formId === args.actionFormId) {
			args.form.submit();
		}

		return Promise.resolve();
	}
}
