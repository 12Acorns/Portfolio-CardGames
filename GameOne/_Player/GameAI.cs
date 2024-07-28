using Deck.Deck.Card;
using GameOne.Game;

namespace GameOne._Player;

public sealed class GameAI : Player
{
	public GameAI(string _name, IEnumerable<GameCard> _startingCards, RoundManager _manager) 
		: base(_startingCards, _manager)
	{
		Name = _name;
	}

	public override string Name { get; }

	protected override void PlayImpl(Action<Player> _render)
	{
	}
}
