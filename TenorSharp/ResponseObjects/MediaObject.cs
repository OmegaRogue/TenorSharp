using System;
using System.Collections.Generic;

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace TenorSharp.ResponseObjects;

/// <summary>
/// A media file on the Tenor API
/// </summary>
public class MediaObject
{
	/// <summary>
	///     width and height in pixels
	/// </summary>
	[JsonProperty("dims")]
	public int[] Dims;

	/// <summary>
	///		undocumented
	/// </summary>
	[JsonProperty("duration")]
	public double Duration;

	/// <summary>
	/// catch-all for any not explicitly defined fields
	/// </summary>
	[JsonExtensionData]
	public IDictionary<string, JToken> Members;

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