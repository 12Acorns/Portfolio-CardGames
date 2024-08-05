using Deck.Deck.Card.Colour;
using GameOne.Extensions;
using Deck.Extensions;
using Deck.Deck.Card;
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

	private readonly Dictionary<Player, List<GameCard>> playerCardMap = new(3);

	public override string Name { get; }

	protected override void PlayImpl(Action<Player> _render)
	{
		var _discardCard = manager.PeekDiscardPileTopCard();
		var _nextPlayer = manager.PeekPlayer(1);

		var _selfOrdered = cards
			.Where(x => x.Description.Colour != (RGBColour)RGBColour.Black)
			.GroupBy(x => x.Description.Colour)
			.Select(x => (x.Key, x.Count()))
			.OrderBy(x => x.Item2);

		// Have no info on played cards for next player, so make a guess-
		// TODO Implement better behaviour
		if(!playerCardMap.TryGetValue(_nextPlayer, out var _cardHistory))
		{
			var _playableCard = cards.First(x => x.CanPlay(_discardCard, false));

			currentCardHandle = cards.IndexOf(_playableCard);

			CardAction(UserCardOptions.PlayCard, x => { }, x => { },
				out bool _colourChange);

			if(!_colourChange)
			{
				return;
			}
			manager.SetWildColour(_selfOrdered.First().Key);
			return;
		}

		var _cardColourAndCount = _cardHistory
			.Where(x => x.Description.Colour != (RGBColour)RGBColour.Black)
			.GroupBy(x => x.Description.Colour)
			.Select(x => (x.Key, x.Count()))
			.OrderBy(x => x.Item2);

		if(PlaySpecial(_nextPlayer, _discardCard, _cardColourAndCount, _selfOrdered))
		{
			return;
		}
		if(PlayNumeric(_nextPlayer, _discardCard, _cardColourAndCount))
		{
			return;
		}
	}

	#region Behaviour
	#region Numeric
	private bool PlayNumeric(Player _nextPlayer,GameCard _discardCard, 
		IOrderedEnumerable<(RGBColour, int)> _ordered)
	{
		return Numeric(_nextPlayer, _discardCard, _ordered);
	}
	private bool Numeric(Player _nextPlayer, GameCard _discardCard, 
		IOrderedEnumerable<(RGBColour, int)> _ordered)
	{

		return true;
	}
	#endregion
	#region Special
	private bool PlaySpecial(Player _nextPlayer, GameCard _discardCard, 
		IOrderedEnumerable<(RGBColour, int)> _ordered, IOrderedEnumerable<(RGBColour, int)> _selfOrdered)
	{
		return Wild(_nextPlayer, _ordered, _selfOrdered, x => x.Data.SubType == Globals.WildPlusFourSubType)
			|| Wild(_nextPlayer, _ordered, _selfOrdered, x => x.Data.SubType == Globals.WildSubType)
			|| Reverse(_nextPlayer, _discardCard) || PlusTwo(_nextPlayer, _discardCard)
			|| Skip(_nextPlayer, _discardCard);
	}
	private bool Wild(Player _nextPlayer, IOrderedEnumerable<(RGBColour, int)> _ordered,
		IOrderedEnumerable<(RGBColour, int)> _selfOrdered,
		Predicate<GameCard> _findPredicate)
	{
		if(_nextPlayer.Cards.Length >= 4)
		{
			return false;
		}
		var _playingCardIndex = cards.FindIndex(_findPredicate);

		if(_playingCardIndex == -1)
		{
			return false;
		}

		int _index = 0;
		foreach(var (_otherPair, _selfPair) in _ordered.EnumerateMany(_selfOrdered))
		{
			var (_colour, _count) = _otherPair;
			var (_selfColour, _selfCount) = _selfPair;

			if(_selfColour == _colour)
			{
				_index++;
				continue;
			}
			currentCardHandle = _playingCardIndex;

			CardAction(UserCardOptions.PlayCard, x => { }, x => { },
				out _);

			manager.SetWildColour(_selfColour);
			return true;
		}
		manager.SetWildColour(_selfOrdered.First().Item1);
		return true;
	}
	private bool Reverse(Player _nextPlayer, GameCard _discardCard)
	{
		var _previousPlayer = manager.PeekPlayer(-1);

		if(!playerCardMap.TryGetValue(_previousPlayer, out var _cardHistory))
		{
			return false;
		}
		// IE, Plus Two, block, reverse, PlusFourWild
		var _previousPlayerDangerCards = _previousPlayer.Cards
			.Select(x => x.Data.SubType == Globals.PlusTwoSubType
					  || x.Data.SubType == Globals.SkipSubType
					  || x.Data.SubType == Globals.ReverseSubType
					  || x.Data.SubType == Globals.WildPlusFourSubType)
			.Sum(x => 1);
		var _nextPlayerDangerCards = _nextPlayer.Cards
			.Select(x => x.Data.SubType == Globals.PlusTwoSubType
					  || x.Data.SubType == Globals.SkipSubType
					  || x.Data.SubType == Globals.ReverseSubType
					  || x.Data.SubType == Globals.WildPlusFourSubType)
			.Sum(x => 1);



		if(_nextPlayerDangerCards > _previousPlayerDangerCards)
		{
			return false;
		}

		currentCardHandle = cards.FindIndex(x => x.Data.SubType == Globals.ReverseSubType
											  && x.CanPlay(_discardCard, false));
		CardAction(UserCardOptions.PlayCard, x => { }, x => { },
			out _);

		// Next player played less danger cards, so reverse as likely to not
		// recieve a affect from danger card
		return true;
	}
	private bool PlusTwo(Player _nextPlayer, GameCard _discardCard)
	{
		if(_nextPlayer.TotalCards > 4)
		{
			return false;
		}

		currentCardHandle = cards.FindIndex(x => x.Data.SubType == Globals.PlusTwoSubType
											  && x.CanPlay(_discardCard, false));
		CardAction(UserCardOptions.PlayCard, x => { }, x => { },
			out _);

		return true;
	}
	private bool Skip(Player _nextPlayer, GameCard _discardCard)
	{
		if(_nextPlayer.TotalCards > 3)
		{
			return false;
		}

		currentCardHandle = cards.FindIndex(x => x.Data.SubType == Globals.SkipSubType
											  && x.CanPlay(_discardCard, false));
		CardAction(UserCardOptions.PlayCard, x => { }, x => { },
			out _);

		return true;
	}
	#endregion
	#endregion
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
