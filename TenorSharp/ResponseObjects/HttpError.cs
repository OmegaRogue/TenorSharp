using System;

using Newtonsoft.Json;

namespace TenorSharp.ResponseObjects;

/// <summary>
/// an HTTP Error Returned by the Tenor API
/// </summary>
[Obsolete("HttpError is deprecated, please use TenorException instead.")]
public class HttpError
{
	/// <summary>
	///     an optional numeric code
	/// </summary>
	[JsonProperty("code")]
	public int Code;

	/// <summary>
	///     a string message describing the error
	/// </summary>
	[JsonProperty("error")]
	public string Error;
}