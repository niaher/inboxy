namespace Inboxy.Help.Output
{
    using UiMetadataFramework.Core.Binding;

    [OutputFieldType("raw-html")]
    public class RawHtml
    {
        public RawHtml(string content)
        {
            this.Content = content;
        }

        public string Content { get; set; }
    }
}