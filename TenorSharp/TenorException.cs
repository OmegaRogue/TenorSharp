using System;
using System.Runtime.Serialization;

namespace TenorSharp;

/// <summary>
///     an Exception thrown by Tenor API Request.
/// </summary>
[Serializable]
public class TenorException : Exception
{
	/// <summary>
	/// The Error Code of the Exception
	/// </summary>
	public int ErrorCode;

	/// <summary>
	/// Creates a new Exception
	/// </summary>
	public TenorException()
	{
	}

	public TenorException(string message, int code) : base(message) => ErrorCode = code;

	public TenorException(string message, Exception inner, int code) : base(message, inner) => ErrorCode = code;

	protected TenorException(SerializationInfo info, StreamingContext context) : base(info, context)
	{
	}
}