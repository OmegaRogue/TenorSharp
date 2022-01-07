using System.Globalization;

namespace TenorSharp;

public class Locale : CultureInfo
{
	public Locale(int culture) : base(culture)
	{
	}

	public Locale(int culture, bool useUserOverride) : base(culture, useUserOverride)
	{
	}

	public Locale(string name) : base(name.Replace('_', '-'))
	{
	}

	public Locale(string name, bool useUserOverride) : base(name.Replace('_', '_'), useUserOverride)
	{
	}

	public override string ToString() => base.Name.Replace('-', '_');
}