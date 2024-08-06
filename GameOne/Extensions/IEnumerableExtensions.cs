namespace GameOne.Extensions;

internal static class IEnumerableExtensions
{
	public static IEnumerable<(T _self, int _index)> WithIndex<T>(this IEnumerable<T> _self)
	{
		if(_self == null)
		{
			throw new NullReferenceException($"{nameof(_self)} is null");
		}

		return _self.Select((T _element, int _index) => (_element, _index));
	}
	public static IEnumerable<(T1 _first, T2 _second)> EnumerateMany<T1, T2>
		(this IEnumerable<T1> _first, IEnumerable<T2> _second)
	{
		if(_first == null)
		{
			throw new NullReferenceException($"{nameof(_first)} is null");
		}
		if(_second == null)
		{
			throw new NullReferenceException($"{nameof(_second)} is null");
		}

		using var _firstIE = _first.GetEnumerator();
		using var _secondIE = _second.GetEnumerator();

		while(_firstIE.MoveNext() && _secondIE.MoveNext())
		{
			yield return (_firstIE.Current, _secondIE.Current);
		}
	}
}
