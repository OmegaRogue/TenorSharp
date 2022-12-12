using System.Text.Json.Serialization;

namespace TenorSharp.SearchResults;

public class Register
{
	/// <summary>
	///     set to “ok” if share registration was successful
	/// </summary>
	[JsonPropertyName("status")]
	public string ShareStatus { get; set; }
}