using Deck.Deck.Card;
using Deck.Extensions;
using GameOne.Game;

namespace GameOne._Player;

internal abstract class Player
{
	public Player(IEnumerable<GameCard> _startingCards, RoundManager _manager)
	{
		// Crude way to sort, in future implement IComparer in GameCard and use Sort() for that
		var _groups = _startingCards.GroupBy(x => x.Description.Colour);
		var _whole = _groups.SelectMany(x => x.Select(x => x).OrderBy(x => x.Data.SubType));
		cards = _whole.ToList();

		currentCardHandle = cards.Count / 2;

		manager = _manager;
	}

	private int currentCardHandleVal;

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
	protected List<GameCard> cards { get; }
	protected string playIndicator => $"{Name}'s turn.\n";
	protected RoundManager manager { get; }


	public GameCard CurrentCard => cards[currentCardHandle];
	public bool IsTurn => manager.CurrentPlayer == this;
	public GameCard[] Cards => [.. cards];
	public abstract string Name { get; }
	public int TotalCards => cards.Count;
	public int Score
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
			Console.WriteLine("Can not play any cards, auto-picked up");
			GiveCard(manager.GetTopCard());
			return;
		}

		PlayImpl(_render);
	}

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
		out bool _colourChange,
		out bool _won)
	{
		_colourChange = false;
		_won = false;

		switch(_option)
		{
			case UserCardOptions.PlayCard:
				if(PlayCard(CurrentCard, out _colourChange))
				{
					manager.OnPlay(this);
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
		_won = cards.Count == 0;
	}
	protected bool HasPlayableCard(GameCard _compareTo)
	{
		return !cards.TrueForAll(x => !x.CanPlay(_compareTo, false));
	}
	public override string ToString()
	{
		return $"Name: {Name}\nScore: {Score}\n";
	}
}
