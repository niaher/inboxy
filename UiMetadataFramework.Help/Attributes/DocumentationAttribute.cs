namespace Inboxy.Help.Attributes
{
    using System;
    using UiMetadataFramework.Core.Binding;

    public class DocumentationAttribute : Attribute, ICustomPropertyAttribute
    {
        public DocumentationAttribute(DocumentPlacement placement, DocumentSourceType sourceType, string source)
        {
            this.Placement = placement;
            this.Source = source;
            this.SourceType = sourceType;
        }

        public DocumentPlacement Placement { get; set; }
        public string Source { get; set; }
        public DocumentSourceType SourceType { get; set; }

        public string Name
        {
            get => "Documentation";
            set
            {
            }
        }

        public object GetValue()
        {
            return new
            {
                Placement = this.Placement,
                SourceType = this.SourceType,
                Source = this.Source
            };
        }
    }

    public enum DocumentSourceType
    {
        String,
        File
    }

    public enum DocumentPlacement
    {
        Hint,
        ExternalContent,
        InlineDescription
    }
}