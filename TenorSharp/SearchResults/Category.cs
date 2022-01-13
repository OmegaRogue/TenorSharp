using System.Collections.Generic;

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

using TenorSharp.ResponseObjects;

namespace TenorSharp.SearchResults;

/// <summary>
/// a json object containing a list of GIF categories associated with the provided type. Each category will include a corresponding search URL to be used if the user clicks on the category. 
/// </summary>
public class Category
{
	/// <summary>
	/// catch-all for any not explicitly defined fields
	/// </summary>
	[JsonExtensionData]
	public IDictionary<string, JToken> Members;

	/// <summary>
	///     an array of CategoryObjects where the “name” field has been translated to the passed in locale language.
	/// </summary>
	[JsonProperty("tags", Required = Required.Always)]
	public CategoryObject[] Tags { get; set; }
}