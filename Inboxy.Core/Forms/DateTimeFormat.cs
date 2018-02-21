namespace Inboxy.Core.Forms
{
	using UiMetadataFramework.Core.Binding;

	/// <summary>
	/// Sets datetime format for the output field of type <see cref="T:System.DateTime" />.
	/// </summary>
	public class DateTimeFormat : StringPropertyAttribute
	{
		public const string DateAndTime = "D MMM YYYY h:mma";

		public DateTimeFormat(string format) : base("format", format)
		{
		}
	}
}