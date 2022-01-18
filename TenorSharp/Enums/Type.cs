namespace TenorSharp.Enums;

/// <summary>
/// The Type of Categories to return from a Category Request
/// </summary>

// ReSharper disable InconsistentNaming
public enum Type
{
	/// <summary>
	///     The current featured emotional / reaction based GIF categories including a preview GIF for each term.
	/// </summary>
	featured,

	/// <summary>
	///     The current featured emoji GIF categories
	/// </summary>
	emoji,

	/// <summary>
	///     The current trending search terms including a preview GIF for each term.
	/// </summary>
	trending
}