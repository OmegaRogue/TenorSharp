using System.Collections.Generic;

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace TenorSharp.SearchResults;

/// <summary>
/// An Object containing the new anonymous id for a user.
/// </summary>
public class Session
{
	/// <summary>
	/// catch-all for any not explicitly defined fields
	/// </summary>
	[JsonExtensionData]
	public IDictionary<string, JToken> Members;

	/// <summary>
	///     an anonymous id used to represent a user.
	///     This allows for tracking without the use of personally identifiable information
	/// </summary>
	[JsonProperty("anon_id", Required = Required.Always)]
	public string AnonId { get; set; }
}