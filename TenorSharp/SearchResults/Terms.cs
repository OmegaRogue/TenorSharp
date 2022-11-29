using System.Text.Json.Serialization;

namespace TenorSharp.SearchResults;

public class Terms
{
	/// <summary>
	///     An array of suggested search terms.
	/// </summary>
	[JsonPropertyName("results")]
	public string[] SearchTerms { get; set; }
}