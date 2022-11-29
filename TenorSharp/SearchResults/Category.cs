using System.Text.Json.Serialization;

using TenorSharp.ResponseObjects;

namespace TenorSharp.SearchResults;

public class Category
{
	/// <summary>
	///     an array of CategoryObjects where the “name” field has been translated to the passed in locale language.
	/// </summary>
	[JsonPropertyName("tags")]
	public CategoryObject[] Tags { get; set; }
}