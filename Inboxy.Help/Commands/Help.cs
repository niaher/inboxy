namespace Inboxy.Help.Commands
{
    using System;
    using System.IO;
    using System.Threading.Tasks;
    using CPermissions;
    using Inboxy.Help.Output;
    using Inboxy.Help.Security;
    using Inboxy.Infrastructure;
    using Inboxy.Infrastructure.Forms;
    using Inboxy.Infrastructure.Security;
    using Markdig;
    using MediatR;
    using UiMetadataFramework.Core;
    using UiMetadataFramework.Core.Binding;

    [MyForm(Id = "help", PostOnLoad = true, Label = "")]
    public class Help : IMyAsyncForm<Help.Request, Help.Response>,ISecureHandler
    {
        public async Task<Response> Handle(Request message)
        {
            var fileName = message.FileId;
            string content;

            try
            {
                // Open the text file using a stream reader.
                using (var sr = new StreamReader($"Help/{fileName}"))
                {
                    // Read the stream to a string, and write the string to the console.
                    content = await sr.ReadToEndAsync();
                }
            }
            catch (Exception)
            {
                throw new BusinessException($"Help file {message.FileId} could not be loaded.");
            }

            var result = Markdown.ToHtml(content);

            return new Response
            {
                Content = new RawHtml(result)
            };
        }

        public class Response : FormResponse<MyFormResponseMetadata>
        {
            [OutputField(Label = "")]
            public RawHtml Content { get; set; }
        }

        public class Request : IRequest<Response>
        {
            [InputField(Required = true, Hidden = true)]
            public string FileId { get; set; }
        }

        public UserAction GetPermission()
        {
            return HelpActions.ViewHelpFiles;
        }
    }
}