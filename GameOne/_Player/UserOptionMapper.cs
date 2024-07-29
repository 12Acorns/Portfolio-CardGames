using Deck.Deck.Card.Colour;

namespace GameOne._Player;

internal static class UserOptionMapper
{
	private static readonly Dictionary<IColourSet, Dictionary<string, RGBColour>> colourMap = [];
	private static readonly Dictionary<string, UserCardOptions> optionMap = new()
	{
		{ "p", UserCardOptions.PlayCard },
		{ "d", UserCardOptions.DrawCard },
		{ "q", UserCardOptions.Quit },
		{ "n", UserCardOptions.NextCard },
		{ "pr", UserCardOptions.PreviousCard },
	};

	public static bool MapCardOption(string _input, out UserCardOptions _option) =>
		optionMap.TryGetValue(_input, out _option);
	public static bool MapColourOption(IColourSet _setUsed, string _input, out RGBColour _colour)
	{
		if(colourMap.TryGetValue(_setUsed, out var _colourMap))
		{
			return _colourMap.TryGetValue(_input, out _colour);
		}

		colourMap.Add(_setUsed, _setUsed.GetColours().ToDictionary(x => x.Name, x => x));
		return colourMap[_setUsed].TryGetValue(_input, out _colour);
	}
}
