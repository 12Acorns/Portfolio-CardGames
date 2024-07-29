using Deck.Deck.Card;
using Deck.Deck.Card.Colour;
using Deck.Extensions;
using GameOne.Game;

namespace GameOne._Player;

internal sealed class GameAI : Player
{
	public GameAI(string _name, IEnumerable<GameCard> _startingCards, RoundManager _manager) 
		: base(_startingCards, _manager)
	{
		Name = _name;
		manager.OnPlay += OnCardPlace;
	}

	private Dictionary<Player, List<GameCard>> playerCardMap = new(3);

	public override string Name { get; }

	protected override void PlayImpl(Action<Player> _render)
	{
		var _topDiscardCard = manager.PeekDiscardPileTopCard();

		var _hasWild = cards.Any(x => x.Data.SubType is CardSubType.Wild);
		var _hasWildFour = cards.Any(x => x.Data.SubType is CardSubType.WildPlusFour);

		var _player = manager.PeekPlayer(1);

		// TODO Implement better behaviour
		if(!playerCardMap.TryGetValue(_player, out var _cardHistory))
		{
			var _playableCard = cards.First(x => x.CanPlay(_topDiscardCard, false));

			currentCardHandle = cards.IndexOf(_playableCard);

			CardAction(UserCardOptions.PlayCard, x => { }, x => { }, 
				out bool _colourChange, out bool _won);
			return;
		}

		var _cardColourMap = _cardHistory
			.GroupBy(x => x.Description.Colour)
			.ToDictionary(x => x.Key, x => x.Count());

		var _ordered = _cardColourMap.OrderBy(x => x.Value);


		if(Wild(_player, _ordered, x => x.Data.SubType is CardSubType.WildPlusFour))
		{
			return;
		}
		if(Wild(_player, _ordered, x => x.Data.SubType is CardSubType.Wild))
		{
			return;
		}
		if(PlusTwo())
		{
			return;
		}
	}
	private bool Wild(Player _player, IOrderedEnumerable<KeyValuePair<IColour, int>> _ordered,
		Predicate<GameCard> _findPredicate)
	{
		if(_player.Cards.Length >= 4)
		{
			return false;
		}
		var _playingCardIndex = cards.FindIndex(_findPredicate);

		if(_playingCardIndex == -1)
		{
			return false;
		}

		var _selfMostCommonColour = cards
			.GroupBy(x => x.Description.Colour)
			.ToDictionary(x => x.Key, x => x.Count());
		var _selfOrdered = _selfMostCommonColour.OrderBy(x => x.Value);

		int _index = 0;
		foreach(var _pair in _ordered)
		{
			var (_colour, _count) = _ordered.ElementAt(_index);
			var (_selfColour, _selfCount) = _selfOrdered.ElementAt(_index);

			if(_selfColour == _colour)
			{
				_index++;
				continue;
			}
			currentCardHandle = _playingCardIndex;

			CardAction(UserCardOptions.PlayCard, x => { }, x => { },
				out bool _colourChange, out bool _won);

			manager.SetWildColour(_colour);

			return true;
		}


		return true;
	}
	private bool PlusTwo()
	{
		return false;
	}
	private void OnCardPlace(Player _player)
	{
		if(_player == this)
		{
			return;
		}

		var _playedCard = manager.PeekDiscardPileTopCard();

		if(!playerCardMap.TryGetValue(_player, out _))
		{
			playerCardMap.Add(_player, [_playedCard]);
			return;
		}
		playerCardMap[_player].Add(_playedCard);
	}
}
