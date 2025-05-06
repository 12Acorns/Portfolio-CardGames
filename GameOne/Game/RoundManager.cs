﻿using System.Runtime.CompilerServices;
using Deck.Deck.Card.Colour;
using Deck.Deck.Randomizer;
using GameOne._Player;
using Deck.Extensions;
using Deck.Deck.Card;
using Deck.Deck;

namespace GameOne.Game;

internal sealed class RoundManager
{
	public RoundManager(CardDeck _deck, int _playerCount, int _startingCards, bool _aiOnly = false)
	{
		PickupDeck = _deck;
		AiOnlyGame = _aiOnly;
		DiscardPile = new CardDeck([], RandomizerFactory.Get(RandomizerType.None), PickupDeck.Cards.Length);
		players = CreateAndGivePlayersCards(_playerCount, _startingCards);

		if(!AiOnlyGame)
		{
			NonAI = players.FirstOrDefault(x => x.GetType() == typeof(GamePlayer)) ?? throw new NullReferenceException("No Non-Ai player");
		}
		else
		{
			NonAI = default!;
		}
		CurrentPlayer = players[0];
		playersLength = (byte)players.Length;

		if(!PickupDeck.TryNextFree(out var _card))
		{
			throw new Exception("Failed to init, try adding more cards");
		}
		DiscardPile.Add(_card);
		// Make sure first playing card is NUMERICTYPE
		while(_card.Description.Type == Globals.SpecialType)
		{
			if(!PickupDeck.TryNextFree(out _card))
			{
				throw new Exception("Failed to init, try adding more cards");
			}
			DiscardPile.Add(_card);
		}
	}

	public bool AiOnlyGame { get; }
	public Action<Player> OnPlay { get; set; } = new(x => { });
	public bool GameOver { get; private set; } = false;
	public bool RoundOver { get; private set; } = false;
	public Player CurrentPlayer { get; private set; }
	public Player NonAI { get; private set; }

	private readonly CardDeck DiscardPile;
	private readonly CardDeck PickupDeck;
	private readonly Player[] players;

	private readonly byte playersLength;
	private byte playerIndex;

	private int skipPlayerCount;
	private bool reverseOrder;

	public void NewRound(int _cardsPerPlayer)
	{
		playerIndex = 0;

		for(int i = PickupDeck.Cards.Length; i > 0; i++)
		{
			PickupDeck.TryNextFree(out _);
			ResetWildColour();
		}

		PickupDeck.ShufflePutDown();
		DiscardPile.Clear();
		for(int i = 0; i < players.Length; i++)
		{
			players[i].Clear();
			players[i].GiveCards(CreatePlayersCards(_cardsPerPlayer));
		}

		PickupDeck.TryNextFree(out var _card);
		DiscardPile.Add(_card);
		// Make sure first playing card is NUMERICTYPE
		while(_card.Description.Type == Globals.SpecialType)
		{
			if(!PickupDeck.TryNextFree(out _card))
			{
				throw new Exception("Failed to init, try adding more cards");
			}
			DiscardPile.Add(_card);
		}

		RoundOver = false;
	}

	public void EvaluatePostPlay()
	{
		OnWin(CurrentPlayer);
	}
	private void OnWin(Player _player)
	{
		if(_player.TotalCards != 0)
		{
			return;
		}
		_player.Score += AllPlayers().Sum(x => x.SumOfCardsScores);
		RoundOver = true;
	}
	public ReadOnlySpan<GameCard> GetMultipleCards(int _amount)
	{
		var _cards = new GameCard[_amount];

		for(int i = 0; i < _amount; i++)
		{
			if(!PickupDeck.TryNextFree(out var _card))
			{
				Shuffle();
				PickupDeck.TryNextFree(out _card);
			}

			_cards[i] = _card;
		}
		return _cards;
	}
	public GameCard GetTopCard()
	{
		if(!PickupDeck.TryNextFree(out var _card))
		{
			Shuffle();
			PickupDeck.TryNextFree(out _card);
		}
		return _card;
	}
	public void SetWildColour(RGBColour _colour)
	{
		if(_colour == RGBColour.None)
		{
			_colour = RGBColour.GetClassicColour((CardColour)Random.Shared.Next(4));
			Console.WriteLine($"Invalid Colour, Got: {RGBColour.None.Name}, Now: {_colour.Name}");
		}
		var _card = DiscardPile.Peek();
		if(!_card.Data.SubType.IsWild())
		{
			return;
		}

		_card.Description = _card.Description with { Colour = _colour };
		DiscardPile.SetCurrentCard(_card);
	}
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public GameCard PeekDiscardPileTopCard() => 
		DiscardPile.Peek();
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public CardDeck GetDiscardPile() =>
		DiscardPile;
	public void AddRangeToDiscard(GameCard[] _cards)
	{
		for(int i = 0; i < _cards.Length; i++)
		{
			_cards[i].PutDown();
		}
		DiscardPile.AddRange(_cards);
	}
	public bool AddToDiscard(GameCard _card)
	{
		if(!_card.CanPlay(DiscardPile, false))
		{
			return false;
		}
		_card.PutDown();
		DiscardPile.Add(_card);
		return true;
	}
	/// <summary>
	/// Returns current player and increments player handle
	/// </summary>
	public Player NextPlayer()
	{
		var _player = CurrentPlayer;
		var unwrappedIndex = reverseOrder ? playerIndex + playersLength - 1 : playerIndex + 1;
		playerIndex = (byte)(unwrappedIndex % playersLength);
		if(skipPlayerCount > 0)
		{
			skipPlayerCount--;
			_player = NextPlayer();
		}
		CurrentPlayer = players[playerIndex];
		return _player;
	}
	/// <summary>
	/// Return nth player in direction of play
	/// </summary>
	/// <param name="_offset"></param>
	/// <returns></returns>
	public Player PeekPlayer(int _offset)
	{
		int _index;
		int _modValue;

		var _checkReverse = reverseOrder ^ (_offset < 0);

		if(_checkReverse)
		{
			_offset = Math.Abs(_offset);
			_modValue = playerIndex - _offset;
		}
		else
		{
			_modValue = playerIndex + _offset;
		}
		while(_modValue < 0)
		{
			_modValue = playersLength + _modValue;
		}
		_index = ((byte)_modValue) % playersLength;
		return players[_index];
	}
	public bool ExecuteCardBehaviour(GameCard _card)
	{
		var _subType = _card.Data.SubType;

		if(_subType.IsNumercic())
		{
			return false;
		}

		if(_subType == Globals.PlusTwoSubType)
		{
			var _nextPlayer = PeekPlayer(1);
			_nextPlayer.GiveCards(GetMultipleCards(2));
			IncrementSkipCount();
			return false;
		}
		if(_subType == Globals.SkipSubType)
		{
			IncrementSkipCount();
			return false;
		}
		if(_subType == Globals.ReverseSubType)
		{
			reverseOrder = !reverseOrder;
			return false;
		}
		if(_subType == Globals.WildSubType)
		{
			return true;
		}
		if(_subType == Globals.WildPlusFourSubType)
		{
			var _nextPlayer = PeekPlayer(1);
			_nextPlayer.GiveCards(GetMultipleCards(4));
			IncrementSkipCount();
			return true;
		}
		throw new NotSupportedException(nameof(_subType) + " is not a supported type");
	}
	public IEnumerable<Player> AllPlayers() => players;
	private void IncrementSkipCount() => skipPlayerCount++;
	private void Shuffle()
	{
		PickupDeck.Shuffle();
		DiscardPile.Clear();

		if(!PickupDeck.TryNextFree(out var _card))
		{
			throw new Exception("Failed to shuffle");
		}

		DiscardPile.Add(_card);

		// Make sure first playing card is NUMERICTYPE
		while(_card.Description.Type == Globals.SpecialType)
		{
			if(!PickupDeck.TryNextFree(out _card))
			{
				throw new Exception("Failed to init, try adding more cards");
			}

			DiscardPile.Add(_card);
		}
	}
	private Player[] CreateAndGivePlayersCards(int _playerCount, int _cardsPerPlayer)
	{
		var _players = new Player[_playerCount];
		for(int i = 0; i < _players.Length; i++)
		{
			var _cards = CreatePlayersCards(_cardsPerPlayer);
			_players[i] = i switch
			{
				0 => AiOnlyGame ? new GameAI($"AI {i}", _cards, this) :  new GamePlayer("Player", _cards, this),
				> 0 => new GameAI($"AI {i}", _cards, this),
				_ => throw new IndexOutOfRangeException(nameof(i))
			};
		}
		return _players;
	}
	private GameCard[] CreatePlayersCards(int _cardsPerPlayer)
	{
		var _cards = new GameCard[_cardsPerPlayer];
		for(int j = 0; j < _cardsPerPlayer; j++)
		{
			if(!PickupDeck.TryNextFree(out var _card))
			{
				throw new Exception("Reached end of deck before game could start, try increasing deck size");
			}
			_cards[j] = _card;
		}
		return _cards;
	}
	private void ResetWildColour()
	{
		var _card = DiscardPile.Peek();
		if(!_card.Data.SubType.IsWild())
		{
			return;
		}
		_card.Description = _card.Description with { Colour = RGBColour.None };
		DiscardPile.SetCurrentCard(_card);
	}
}