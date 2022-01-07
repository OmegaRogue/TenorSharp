using Newtonsoft.Json;

namespace TenorSharp.SearchResults;

public class Session
{
	/// <summary>
	///     an anonymous id used to represent a user.
	///     This allows for tracking without the use of personally identifiable information
	/// </summary>
	[JsonProperty("anon_id", Required = Required.Always)]
	public string AnonId { get; set; }
}