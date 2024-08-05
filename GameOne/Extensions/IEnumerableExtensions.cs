namespace GameOne.Extensions;

internal static class IEnumerableExtensions
{
	public static IEnumerable<(T1 _first, T2 _second)> EnumerateMany<T1, T2>
		(this IEnumerable<T1> _first, IEnumerable<T2> _second)
	{
		using var _firstIE = _first.GetEnumerator();
		using var _secondIE = _second.GetEnumerator();

		while(_firstIE.MoveNext() && _secondIE.MoveNext())
		{
			yield return (_firstIE.Current, _secondIE.Current);
		}
	}
}
