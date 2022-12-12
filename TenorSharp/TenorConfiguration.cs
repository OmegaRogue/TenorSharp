using System.Collections.Generic;
using TenorSharp.Enums;

namespace TenorSharp;

public class TenorConfiguration
{
	public string        ApiKey        { get; set; }
	public string        AnonId        { get; set; }
	public AspectRatio   ArRange       { get; set; } = AspectRatio.all;
	public ContentFilter ContentFilter { get; set; } = ContentFilter.off;
	public List<GifFormat>   MediaFilter   { get; set; } = new();
	public Locale        Locale        { get; set; } = new("en_US");
}