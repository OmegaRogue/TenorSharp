using System;
using System.Collections.Generic;

using System.Text.Json;
using System.Text.Json.Serialization;
using TenorSharp.Enums;

namespace TenorSharp.ResponseObjects;

public class GifObject
{
	/// <summary>
	///     Tenor result identifier
	/// </summary>
	[JsonPropertyName("id")]
	public string Id { get; set; }
	
	/// <summary>
	///     the title of the post.
	/// </summary>
	[JsonPropertyName("title")]
	public string Title { get; set; }
	
	/// <summary>
	///     An array of dictionaries with GifFormat as the key and MediaObject as the value
	/// </summary>
	[JsonPropertyName("media_formats")]
	public Dictionary<string, MediaObject> Media { get; set; }
	
	/// <summary>
	///     a unix timestamp representing when this post was created.
	/// </summary>
	[JsonPropertyName("created")]
	public float Created { get; set; }

	/// <summary>
	///     true if this post contains audio (only video formats support audio, the gif image file format can not contain audio
	///     information).
	/// </summary>
	[JsonPropertyName("hasaudio")]
	public bool HasAudio { get; set; }

	/// <summary>
	///     an array of tags for the post
	/// </summary>
	[JsonPropertyName("tags")]
	public string[] Tags { get; set; }

	/// <summary>
	///		the description of the post.
	/// </summary>
	[JsonPropertyName("content_description")]
	public string Description { get; set; }

	/// <summary>
	///     the full URL to view the post on tenor.com.
	/// </summary>
	[JsonPropertyName("itemurl")]
	public string ItemUrl { get; set; }
	
	/// <summary>
	///		returns true if this post contains captions.
	/// </summary>
	[JsonPropertyName("hascaption")]
	public bool HasCaption { get; set; }
	
	/// <summary>
	///     an array of flags for the post
	/// </summary>
	[JsonPropertyName("flags")]
	public string[] Flags { get; set; }

	/// <summary>
	///		the most common background pixel color of the content
	/// </summary>
	[JsonPropertyName("bg_color")]
	public string BackgroundColor { get; set; }

	/// <summary>
	///     a short URL to view the post on tenor.com.
	/// </summary>
	[JsonPropertyName("url")]
	public string Url { get; set; }

	/// <summary>
	/// Returns the media object for the given format
	/// </summary>
	public MediaObject GetMediaType(GifFormat format)
	{
		Media.TryGetValue(format.ToString(), out var media);
		return media;
	}
}