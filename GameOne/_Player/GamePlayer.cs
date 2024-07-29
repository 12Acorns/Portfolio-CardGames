using Deck.Deck.Card.Colour;
using Deck.Deck.Card;
using GameOne.Game;
using System.Text;

namespace GameOne._Player;

internal sealed class GamePlayer : Player
{
	private const string PlAYOPTIONS =
		"""
		Options:
		    [P]lay Current Card
		    [D]raw From Deck
		    [N]ext Card
		    [Pr]evious Card
		    [Q]uit Game

		>
		""";

	public GamePlayer(string _name, IEnumerable<GameCard> _startingCards, RoundManager _manager)
		: base(_startingCards, _manager)
	{
		Name = _name;
		set = cards[0].Description.ColourSet;

		var _builder = new StringBuilder();

		var _colours =
			cards[0].Description.ColourSet.GetColours()
				.DistinctBy(x => x)
				.Select(x => x.Name)
				.Where(x => x != "Black");
		_builder.AppendJoin(", ", _colours);

		colours = _builder.ToString() + "\n\n>";
	}

	private readonly string colours;
	private readonly IColourSet set;

	public override string Name { get; }

	protected override void PlayImpl(Action<Player> _render)
	{
		AskOption(_render);
	}
	private void AskOption(Action<Player> _render)
	{
		Console.WriteLine();
		Console.Write(PlAYOPTIONS);

		var _optionString = Console.ReadLine() ?? "";

		if(!UserOptionMapper.MapCardOption(_optionString.ToLower(), out var _option))
		{
			Console.Write("\nPlease enter a valid option.");
			AskOption(_render);
			return;
		}

		CardAction(_option, AskOption, _render, out bool _colourChange, out bool _won);

		if(_won)
		{
			manager.OnWin(this);
			return;
		}

		if(!_colourChange)
		{
			return;
		}

		AskColourOptions(_render);
	}
	private void AskColourOptions(Action<Player> _render)
	{
		Console.WriteLine("Colour Options (Case-Sensitive):\n");
		Console.Write(colours);

		var _input = Console.ReadLine() ?? "";

		if(!UserOptionMapper.MapColourOption(set, _input, out var _colour))
		{
			Console.WriteLine("Please Enter A Valid Colour!");
			AskColourOptions(_render);
			return;
		}
		manager.SetWildColour(_colour);
	}
}
