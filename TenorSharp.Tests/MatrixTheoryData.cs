using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;

using Xunit;

namespace TenorSharp.Tests;

public class MatrixTheoryData<T1, T2> : TheoryData<T1, T2>
{
	public MatrixTheoryData(IEnumerable<T1> data1, IEnumerable<T2> data2)
	{
		Contract.Assert(data1 != null && data1.Any());
		Contract.Assert(data2 != null && data2.Any());

		foreach (var t1 in data1)
		foreach (var t2 in data2)
			Add(t1, t2);
	}
}

public class MatrixTheoryData<T1, T2, T3> : TheoryData<T1, T2, T3>
{
	public MatrixTheoryData(IEnumerable<T1> data1, IEnumerable<T2> data2, IEnumerable<T3> data3)
	{
		Contract.Assert(data1 != null && data1.Any());
		Contract.Assert(data2 != null && data2.Any());
		Contract.Assert(data3 != null && data3.Any());

		foreach (var t1 in data1)
		foreach (var t2 in data2)
		foreach (var t3 in data3)
			Add(t1, t2, t3);
	}
}

public class MatrixTheoryData<T1, T2, T3, T4> : TheoryData<T1, T2, T3, T4>
{
	public MatrixTheoryData(IEnumerable<T1> data1, IEnumerable<T2> data2, IEnumerable<T3> data3, IEnumerable<T4> data4)
	{
		Contract.Assert(data1 != null && data1.Any());
		Contract.Assert(data2 != null && data2.Any());
		Contract.Assert(data3 != null && data3.Any());
		Contract.Assert(data4 != null && data4.Any());

		foreach (var t1 in data1)
		foreach (var t2 in data2)
		foreach (var t3 in data3)
		foreach (var t4 in data4)
			Add(t1, t2, t3, t4);
	}
}