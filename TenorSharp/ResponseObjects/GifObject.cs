using System;
using System.Collections.Generic;

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

using TenorSharp.Enums;

namespace TenorSharp.ResponseObjects;

/// <summary>
/// An Object containing the data of a GIF.
/// </summary>
public class GifObject
{
	/// <summary>
	/// undocumented
	/// </summary>
	[JsonProperty("bg_color")]
	public string BgColor;

	/// <summary>
	/// undocumented
	/// </summary>
	[JsonProperty("composite")]
	public object Composite;

	/// <summary>
	///		undocumented
	/// </summary>
	[JsonProperty("content_decription")]
	public string ContentDescription;

	/// <summary>
	/// undocumented
	/// </summary>
	[JsonProperty("content_rating")]
	public string ContentRating;

	/// <summary>
	///     a unix timestamp representing when this post was created.
	/// </summary>
	[JsonProperty("created")]
	public double Created;

	/// <summary>
	/// undocumented
	/// </summary>
	[JsonProperty("flags")]
	public string[] Flags;

	/// <summary>
	/// undocumented
	/// </summary>
	[JsonProperty("h1_title")]
	public string H1Title;

	/// <summary>
	///     true if this post contains audio (only video formats support audio, the gif image file format can not contain audio
	///     information).
	/// </summary>
	[JsonProperty("hasaudio")]
	public bool HasAudio;

	/// <summary>
	///     true if this post contains captions
	/// </summary>
	[JsonProperty("hascaption")]
	public bool HasCaption;

	/// <summary>
	///     Tenor result identifier
	/// </summary>
	[JsonProperty("id")]
	public string Id;

	/// <summary>
	///     the full URL to view the post on tenor.com.
	/// </summary>
	[JsonProperty("itemurl")]
	public Uri ItemUrl;

	/// <summary>
	///     An array of dictionaries with GifFormat as the key and MediaObject as the value
	/// </summary>
	[JsonProperty("media")]
	public Dictionary<GifFormat, MediaObject>[] Media;

	/// <summary>
	/// catch-all for any not explicitly defined fields
	/// </summary>
	[Newtonsoft.Json.JsonExtensionData]
	public IDictionary<string, JToken> Members;

	/// <summary>
	///     the amount of Shares the post has.
	/// </summary>
	[JsonProperty("shares")]
	public int Shares;

	/// <summary>
	/// undocumented
	/// </summary>
	[JsonProperty("source_id")]
	public string SourceId;

	/// <summary>
	///     an array of tags for the post
	/// </summary>
	[JsonProperty("tags")]
	public string[] Tags;

	/// <summary>
	///     the title of the post.
	/// </summary>
	[JsonProperty("title")]
	public string Title;

	/// <summary>
	///     a short URL to view the post on tenor.com.
	/// </summary>
	[JsonProperty("url")]
	public Uri Url;
}