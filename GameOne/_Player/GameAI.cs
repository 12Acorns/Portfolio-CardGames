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

		defaultCardAction = (Predicate<GameCard> _indexPredicate, out bool _colourChange) =>
		{
			ArgumentNullException.ThrowIfNull(_indexPredicate);

			_colourChange = false;
			var _index = cards.FindIndex(_indexPredicate);
			if(_index == -1)
			{
				return false;
			}

			currentCardHandle = _index;
			CardAction(UserCardOptions.PlayCard, x => { }, x => { }, out _colourChange);
			return true;
		};
	}

	private delegate bool defaultAction(Predicate<GameCard> _indexPredicate, out bool _colourChange);
	private readonly defaultAction defaultCardAction;

	private readonly Dictionary<Player, List<GameCard>> playerCardMap = new(3);

	public override string Name { get; }

	protected override void PlayImpl(Action<Player> _render)
	{
		var _discardCard = manager.PeekDiscardPileTopCard();
		var _nextPlayer = manager.PeekPlayer(1);

		var _selfOrdered = cards
			.Where(x => x.Description.Colour != RGBColour.None)
			.GroupBy(x => x.Description.Colour)
			.Select(x => (x.Key, x.Count()))
			.OrderBy(x => x.Item2);

		// Have no info on played cards for next player, so make a guess
		if(!playerCardMap.TryGetValue(_nextPlayer, out var _cardHistory))
		{
			defaultCardAction(x => x.CanPlay(_discardCard, false), out bool _colourChange);
			if(!_colourChange)
			{
				return;
			}
			var _first = _selfOrdered.FirstOrDefault().Key;
			manager.SetWildColour(_first);
			return;
		}

		var _cardColourAndCount = _cardHistory
			.Where(x => x.Description.Colour != RGBColour.None)
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
		return defaultCardAction(x => x.CanPlay(_discardCard, false), out _);
	}
	#endregion
	#region Special
	private bool PlaySpecial(Player _nextPlayer, GameCard _discardCard, 
		IOrderedEnumerable<(RGBColour, int)> _ordered, IOrderedEnumerable<(RGBColour, int)> _selfOrdered)
	{
		return Wild(_nextPlayer, _ordered, _selfOrdered, x => x.Data.SubType == Globals.WildPlusFourSubType)
			|| Wild(_nextPlayer, _ordered, _selfOrdered, x => x.Data.SubType == Globals.WildSubType)
			|| Reverse(_nextPlayer, _discardCard) || MinorSpecial(_nextPlayer, _discardCard, Globals.PlusTwoSubType, 3)
			|| MinorSpecial(_nextPlayer, _discardCard, Globals.SkipSubType, 4);
	}
	private bool Wild(Player _nextPlayer, IOrderedEnumerable<(RGBColour, int)> _ordered,
		IOrderedEnumerable<(RGBColour, int)> _selfOrdered, Predicate<GameCard> _findPredicate)
	{
		if(_nextPlayer.TotalCards >= 4)
		{
			return false;
		}

		var _playingCardIndex = cards.FindIndex(_findPredicate);
		if(_playingCardIndex == -1)
		{
			return false;
		}

		currentCardHandle = _playingCardIndex;
		RGBColour? _finalColour = null;
		foreach(var (_otherPair, _selfPair) in _ordered.EnumerateMany(_selfOrdered))
		{
			var (_colour, _count) = _otherPair;
			var (_selfColour, _selfCount) = _selfPair;

			if(_selfColour == _colour)
			{
				continue;
			}

			_finalColour = _selfColour;
			break;
		}
		if(!_finalColour.HasValue)
		{
			_finalColour = _selfOrdered.First().Item1;
		}

		CardAction(UserCardOptions.PlayCard, x => { }, x => { }, out _);
		manager.SetWildColour(_finalColour.Value);
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
			.Sum(x =>   (x.Data.SubType == Globals.PlusTwoSubType
					  || x.Data.SubType == Globals.SkipSubType
					  || x.Data.SubType == Globals.ReverseSubType
					  || x.Data.SubType == Globals.WildPlusFourSubType)
					   ? 1 : 0);
		var _nextPlayerDangerCards = _nextPlayer.Cards
			.Sum(x =>   (x.Data.SubType == Globals.PlusTwoSubType
					  || x.Data.SubType == Globals.SkipSubType
					  || x.Data.SubType == Globals.ReverseSubType
					  || x.Data.SubType == Globals.WildPlusFourSubType)
					   ? 1 : 0);

		if(_nextPlayerDangerCards > _previousPlayerDangerCards)
		{
			return false;
		}

		// Next player played less danger cards, so reverse as likely to not
		// recieve a affect from danger card
		return defaultCardAction(x => x.Data.SubType == Globals.ReverseSubType
								   && x.CanPlay(_discardCard, false), out _);
	}
	private bool MinorSpecial(Player _nextPlayer, GameCard _discardCard, 
		CardSubType _typeComparison, int _minCardsToPlay)
	{
		if(_nextPlayer.TotalCards > _minCardsToPlay)
		{
			return false;
		}

		return defaultCardAction(x => x.Data.SubType == _typeComparison
								   && x.CanPlay(_discardCard, false), out _);
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
