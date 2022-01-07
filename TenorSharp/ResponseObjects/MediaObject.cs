using System;

using Newtonsoft.Json;

namespace TenorSharp.ResponseObjects;

public class MediaObject
{
	/// <summary>
	///     width and height in pixels
	/// </summary>
	[JsonProperty("dims")]
	public int[] Dims;

	/// <summary>
	///     a url to a preview image of the media source
	/// </summary>
	[JsonProperty("preview")]
	public string Preview;

	/// <summary>
	///     size of file in bytes
	/// </summary>
	[JsonProperty("size")]
	public int Size;

	/// <summary>
	///     a url to the media source
	/// </summary>
	[JsonProperty("url")]
	public Uri Url;
}