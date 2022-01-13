using TenorSharp.Enums;
using TenorSharp.ResponseObjects;

namespace TenorSharp;

/// <summary>
/// an object containing the configuration for the Client
/// </summary>
public record struct TenorConfiguration
{
	/// <summary>
	/// client key for privileged API access
	/// </summary>
	public string ApiKey { get; set; }

	/// <summary>
	/// specify the anonymous_id tied to the given user
	/// </summary>
	public string AnonId { get; set; }

	/// <summary>
	/// Filter the response <see cref="GifObject"/> list to only include GIFs with aspect ratios that fit with in the selected range.
	/// </summary>
	public AspectRatio ArRange { get; set; } = AspectRatio.all;

	/// <summary>
	/// specify the content safety filter level
	/// </summary>
	public ContentFilter ContentFilter { get; set; } = ContentFilter.off;

	/// <summary>
	/// Reduce the Number of GIF formats returned in the <see cref="GifObject"/> list.
	/// </summary>
	public MediaFilter MediaFilter { get; set; } = MediaFilter.off;

	/// <summary>
	/// specify default language to interpret search string
	/// </summary>
	public Locale Locale { get; set; } = new("en_US");
}