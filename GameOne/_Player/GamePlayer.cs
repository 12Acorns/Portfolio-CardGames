using Deck.Deck.Card;
using Deck.Extensions;
using GameOne.Game;

namespace GameOne._Player;

public sealed class GamePlayer : Player
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
	}

	public override string Name { get; }

	protected override void PlayImpl(Action<Player> _render)
	{
		var _topCard = manager.PeekDiscardPileTopCard();
		// Cannot play card
		if(cards.TrueForAll(x => !x.CanPlay(_topCard, false)))
		{
			Console.WriteLine("Can not play any cards, auto-picked up");
			GiveGard(manager.GetTopCard());
			return;
		}

		AskOption(_render);
	}
	private void AskOption(Action<Player> _render)
	{
		Console.WriteLine();
		Console.Write(PlAYOPTIONS);

		var _optionString = Console.ReadLine() ?? "";

		if(!UserOptionMapper.Map(_optionString.ToLower(), out var _option))
		{
			Console.Write("\nPlease enter a valid option.");
			AskOption(_render);
			return;
		}

		CardAction(_option, AskOption, _render);
	}
}
