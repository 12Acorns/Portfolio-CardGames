using Deck.Deck.Card;
using Deck.Extensions;
using GameOne.Extensions;
using GameOne.Game;

namespace GameOne._Player;

internal abstract class Player
{
	public Player(IEnumerable<GameCard> _startingCards, RoundManager _manager)
	{
		var _groups = _startingCards.GroupBy(x => x.Description.Colour);
		var _whole = _groups.SelectMany(x => x.Select(x => x).OrderBy(x => x.Data.SubType, new CardComparison()));
		cards = _whole.ToList();

		currentCardHandle = cards.Count / 2;

		manager = _manager;
	}

	private int currentCardHandleVal;
	private int score;

	protected int currentCardHandle
	{
		get => currentCardHandleVal %= cards.Count;
		set
		{
			currentCardHandleVal = value;
			if(currentCardHandle < 0)
			{
				currentCardHandleVal = cards.Count - 1;
			}
			currentCardHandleVal %= cards.Count;
		}
	}
	protected string playIndicator => $"{Name}'s turn.\n";
	protected readonly List<GameCard> cards;
	protected readonly RoundManager manager;


	public GameCard CurrentCard => cards[currentCardHandle];
	public bool IsTurn => manager.CurrentPlayer == this;
	public List<GameCard> Cards => cards;
	public abstract string Name { get; }
	public int TotalCards => cards.Count;
	public int Score
	{
		get => score;
		set => score = value;
	}
	public int SumOfCardsScores
	{
		get
		{
			var _score = 0;
			foreach(var _card in cards)
			{
				_score += _card.Data.Score;
			}
			return _score;
		}
	}

	public void Play(Action<Player> _render)
	{
		if(!IsTurn)
		{
			return;
		}
		Console.Write(playIndicator);

		var _topCard = manager.PeekDiscardPileTopCard();
		if(!HasPlayableCard(_topCard))
		{
			if(this == manager.NonAI)
			{
				Console.WriteLine("Can not play any cards, auto-picked up");
			}
			GiveCard(manager.GetTopCard());
			return;
		}

		PlayImpl(_render);
	}

	public void GiveCards(ReadOnlySpan<GameCard> _cards) => cards.AddRange(_cards);
	public void GiveCard(GameCard _card) => cards.Add(_card);
	public bool PlayCards(ReadOnlySpan<GameCard> _desiredCards)
	{
		if(!IsTurn)
		{
			return false;
		}

		foreach(var _card in _desiredCards)
		{
			if(!PlayCardImpl(_card, out _))
			{
				return false;
			}
		}

		return true;
	}
	public bool PlayCard(GameCard _desiredCard, out bool _colourChange)
	{
		_colourChange = false;
		if(!IsTurn)
		{
			return false;
		}

		return PlayCardImpl(_desiredCard, out _colourChange);
	}
	private bool PlayCardImpl(GameCard _card, out bool _colourChange)
	{
		_colourChange = false;
		if(!manager.AddToDiscard(_card))
		{
			return false;
		}
		if(!cards.Remove(_card))
		{
			throw new Exception("Specified card does not exist in current players hand");
		}

		_colourChange = manager.ExecuteCardBehaviour(_card);

		return true;
	}
	protected abstract void PlayImpl(Action<Player> _render);
	protected void CardAction(
		UserCardOptions _option, 
		Action<Action<Player>> _onBufferMove, 
		Action<Player> _render,
		out bool _colourChange)
	{
		_colourChange = false;

		switch(_option)
		{
			case UserCardOptions.PlayCard:
				if(PlayCard(CurrentCard, out _colourChange))
				{
					manager.OnPlay(this);
					manager.EvaluatePostPlay();
					break;
				}

				Console.Write("\nCan not place card, try another.");
				_onBufferMove(_render);
				break;
			case UserCardOptions.DrawCard:
				GiveCard(manager.GetTopCard());
				break;
			case UserCardOptions.Quit:
				Environment.Exit(0);
				break;
			case UserCardOptions.NextCard:
				currentCardHandle++;
				_render(this);
				_onBufferMove(_render);
				break;
			case UserCardOptions.PreviousCard:
				currentCardHandle--;
				_render(this);
				_onBufferMove(_render);
				break;
		}
	}
	protected bool HasPlayableCard(GameCard _compareTo)
	{
		return !cards.TrueForAll(x => !x.CanPlay(_compareTo, false));
	}
	public override string ToString()
	{
		return $"Name: {Name}\nScore: {SumOfCardsScores}\n";
	}
}
