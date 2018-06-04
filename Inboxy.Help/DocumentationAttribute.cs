namespace Inboxy.Help
{
    using System;
    using System.IO;
    using Markdig;
    using UiMetadataFramework.Core.Binding;

    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Field | AttributeTargets.Property, AllowMultiple = true)]
    [CustomPropertyConfig(IsArray = true)]
    public class DocumentationAttribute : Attribute, ICustomPropertyAttribute
    {
        public DocumentationAttribute(DocumentationPlacement placement, DocumentationSourceType sourceType, string source, params string[] files)
        {
            this.Placement = placement;
            this.Files = files;
            this.SourceType = sourceType;
            this.Content = this.GetContent(source, sourceType);
        }

        /// <summary>
        /// Documentation content can be direct string or external file <seealso cref="DocumentationSourceType"/>
        /// </summary>
        public string Content { get; set; }

        public string[] Files { get; set; }

        /// <summary>
        /// Indecate where this documentation will be rendered
        /// </summary>
        public DocumentationPlacement Placement { get; set; }

        /// <summary>
        /// Indecate where the documentation content exists
        /// </summary>
        public DocumentationSourceType SourceType { get; set; }

        public string Name => "Documentation";

        public object GetValue()
        {
            return new
            {
                Placement = this.Placement,
                Files = this.Files,
                Content = this.Content
            };
        }

        private string GetContent(string source, DocumentationSourceType sourceType)
        {
            string content = string.Empty;
            if (sourceType == DocumentationSourceType.File)
            {
                try
                {
                    using (var sr = new StreamReader($"Help/{source}"))
                    {
                        // Read the stream to a string, and write the string to the console.
                        content = sr.ReadToEnd();
                    }
                }
                catch
                {
                    // ignored
                }
            }
            else
            {
                content = source;
            }

            var result = Markdown.ToHtml(content);

            return result;
        }
    }

    public enum DocumentationPlacement
    {
        Hint,
        Inline
    }

    public enum DocumentationSourceType
    {
        String,
        File
    }
}