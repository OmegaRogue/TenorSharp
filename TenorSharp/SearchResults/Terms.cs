using System.Collections.Generic;

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace TenorSharp.SearchResults;

/// <summary>
/// A list of Search Terms returned by an API Request
/// </summary>
public class Terms
{
	/// <summary>
	/// catch-all for any not explicitly defined fields
	/// </summary>
	[JsonExtensionData]
	public IDictionary<string, JToken> Members;

	/// <summary>
	///     An array of suggested search terms.
	/// </summary>
	[JsonProperty("results", Required = Required.Always)]
	public string[] SearchTerms;
}