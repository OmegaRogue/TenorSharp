using System.Globalization;

namespace TenorSharp;

/// <summary>
/// A Class converting <see cref="CultureInfo"/> strings into the format used by the Tenor API.
/// </summary>
public class Locale : CultureInfo
{
	/// <inheritdoc cref="CultureInfo(int)"/>
	public Locale(int culture) : base(culture)
	{
	}

	/// <inheritdoc cref="CultureInfo(int, bool)"/>
	public Locale(int culture, bool useUserOverride) : base(culture, useUserOverride)
	{
	}

	/// <inheritdoc cref="CultureInfo(string)"/>
	public Locale(string name) : base(name.Replace('_', '-'))
	{
	}

	/// <inheritdoc cref="CultureInfo(string, bool)"/>
	public Locale(string name, bool useUserOverride) : base(name.Replace('_', '_'), useUserOverride)
	{
	}

	/// <summary>
	/// Implements object.ToString(). Returns the name of the <see cref="CultureInfo"/>,
	/// eg. "de_DE_phoneb", "en_US", or "fj_FJ".
	/// </summary>
	public override string ToString() => base.Name.Replace('-', '_');
}