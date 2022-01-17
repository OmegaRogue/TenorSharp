using Newtonsoft.Json;

namespace TenorSharp.SearchResults;

/// <summary>
/// The Response to a Register Share Request
/// </summary>
public class Register
{
	/// <summary>
	///     set to “ok” if share registration was successful
	/// </summary>
	[JsonProperty("status", Required = Required.Always)]
	public string ShareStatus { get; set; } = null!;
}