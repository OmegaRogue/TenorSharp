using System.Collections.Generic;

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace TenorSharp.SearchResults;

/// <summary>
/// The Response to a Register Share Request
/// </summary>
public class Register
{
	/// <summary>
	/// catch-all for any not explicitly defined fields
	/// </summary>
	[JsonExtensionData]
	public IDictionary<string, JToken> Members;

	/// <summary>
	///     set to “ok” if share registration was successful
	/// </summary>
	[JsonProperty("status", Required = Required.Always)]
	public string ShareStatus { get; set; }
}