namespace Inboxy.Core.Forms.Outputs
{
	using Microsoft.Exchange.WebServices.Data;
	using UiMetadataFramework.Core.Binding;

	[OutputFieldType("email-body")]
	public class EmailBody
	{
		public EmailBody(string text, BodyType type)
		{
			this.Text = text;
			this.Type = type;
		}

		public string Text { get; }
		public BodyType Type { get; }
	}
}