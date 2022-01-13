using System;

using Newtonsoft.Json;

namespace TenorSharp.ResponseObjects;

/// <summary>
///     an HTTP Error Returned by the Tenor API
/// </summary>
[Serializable]
public class TenorException : Exception
{
	/// <summary>
	/// an optional numeric code
	/// </summary>
	public int ErrorCode;

	/// <summary>
	/// Creates a Tenor API Error Exception
	/// </summary>
	/// <param name="error">a string message describing the error</param>
	/// <param name="code">an optional numeric code</param>
	[JsonConstructor]
	public TenorException(string error, int code) : base(error) => ErrorCode = code;

	/// <inheritdoc />
	public override string ToString()
	{
		Data["code"]  = ErrorCode;
		Data["error"] = Message;
		return base.ToString();
	}
}