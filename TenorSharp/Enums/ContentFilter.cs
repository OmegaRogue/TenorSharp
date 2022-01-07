namespace TenorSharp.Enums;

// ReSharper disable InconsistentNaming
/// <summary>
///     specify the content safety filter level
/// </summary>
public enum ContentFilter
{
	/// <summary>
	///     G
	/// </summary>
	high,

	/// <summary>
	///     G and PG
	/// </summary>
	medium,

	/// <summary>
	///     G, PG, and PG-13
	/// </summary>
	low,

	/// <summary>
	///     G, PG, PG-13, and R (no nudity)
	/// </summary>
	off
}