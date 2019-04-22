using TenorSharp.Enums;

namespace TenorSharp
{
	public class TenorConfiguration
	{
		public string        ApiKey        { get; set; }
		public string        AnonId        { get; set; }
		public AspectRatio   ArRange       { get; set; } = AspectRatio.all;
		public ContentFilter ContentFilter { get; set; } = ContentFilter.off;
		public MediaFilter   MediaFilter   { get; set; } = MediaFilter.off;
		public Locale        Locale        { get; set; } = new Locale("en_US");
	}
}