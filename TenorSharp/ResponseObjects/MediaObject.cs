using System;

using System.Text.Json;
using System.Text.Json.Serialization;

namespace TenorSharp.ResponseObjects;

public class MediaObject
{
	/// <summary>
	///     a url to a preview image of the media source
	/// </summary>
	[JsonPropertyName("preview")]
	public string Preview { get; set; }
	
	/// <summary>
	///     a url to the media source
	/// </summary>
	[JsonPropertyName("url")]
	public string Url { get; set; }
	
	/// <summary>
	///     width and height in pixels
	/// </summary>
	[JsonPropertyName("dims")] 
	public int[] Dims { get; set; }

	/// <summary>
	///     size of file in bytes
	/// </summary>
	[JsonPropertyName("size")]
	public int Size { get; set; }
}