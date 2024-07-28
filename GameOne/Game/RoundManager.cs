using System.Runtime.CompilerServices;
using Deck.Deck.Randomizer;
using GameOne._Player;
using Deck.Deck.Card;
using Deck.Deck;
using Deck.Extensions;

namespace GameOne.Game;

public sealed class RoundManager
{
	public RoundManager(CardDeck _deck, int _playerCount, int _startingCards)
	{
		PickupDeck = _deck;
		DiscardPile = new CardDeck([], RandomizerFactory.Get(RandomizerType.None), PickupDeck.Cards.Length);
		players = CreateAndGivePlayersCards(_playerCount, _startingCards);

		NonAI = players.FirstOrDefault(x => x.GetType() == typeof(GamePlayer)) ?? throw new NullReferenceException("No Non-Ai player");
		CurrentPlayer = players[0];

		playersLength = (byte)players.Length;

		if(!PickupDeck.TryNextFree(out var _card))
		{
			throw new Exception("Failed to init, try adding more cards");
		}

		DiscardPile.Add(_card);

		while(_card.Description.Type is CardType.Special)
		{
			if(!PickupDeck.TryNextFree(out _card))
			{
				throw new Exception("Failed to init, try adding more cards");
			}

			DiscardPile.Add(_card);
		}
	}

	public Player CurrentPlayer { get; private set; }
	public Player NonAI { get; private set; }

	private readonly CardDeck DiscardPile;
	private readonly CardDeck PickupDeck;
	private readonly Player[] players;

	private readonly byte playersLength;
	private byte playerIndex;

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
		_card.PutDown();

		if(!_card.CanPlay(DiscardPile))
		{
			return false;
		}

		DiscardPile.Add(_card);
		return true;
	}
	public Player NextPlayer()
	{
		var _player = CurrentPlayer!;
		playerIndex = (byte)((playerIndex + 1) % playersLength);
		CurrentPlayer = players![playerIndex];

		return _player;
	}
	private void Shuffle()
	{
		PickupDeck.Shuffle();
		DiscardPile.Clear();
	}
	private Player[] CreateAndGivePlayersCards(int _playerCount, int _cardsPerPlayer)
	{
		var _players = new Player[_playerCount];

		for(int i = 0; i < _players.Length; i++)
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
			_players[i] = i switch
			{
				0 => new GamePlayer("Player", _cards, this),
				> 0 => new GameAI("AI " + i, _cards, this),
				_ => throw new IndexOutOfRangeException(nameof(i))
			};
		}
		return _players;
	}
}