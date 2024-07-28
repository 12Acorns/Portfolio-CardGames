namespace GameOne._Player;

internal static class UserOptionMapper
{
	private static readonly IReadOnlyDictionary<string, UserOptions> optionMap = new Dictionary<string, UserOptions>()
	{
		{ "p", UserOptions.PlayCard },
		{ "d", UserOptions.DrawCard },
		{ "q", UserOptions.Quit },
		{ "n", UserOptions.NextCard },
		{ "pr", UserOptions.PreviousCard },
	};

	public static bool Map(string _input, out UserOptions _option) =>
		optionMap.TryGetValue(_input, out _option);
}
