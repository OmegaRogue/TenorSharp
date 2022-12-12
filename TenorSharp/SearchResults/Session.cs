using System.Text.Json.Serialization;

namespace TenorSharp.SearchResults;

public class Session
{
	/// <summary>
	///     an anonymous id used to represent a user.
	///     This allows for tracking without the use of personally identifiable information
	/// </summary>
	[JsonPropertyName("anon_id")]
	public string AnonId { get; set; }
}