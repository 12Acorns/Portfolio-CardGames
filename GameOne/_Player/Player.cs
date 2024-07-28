using Deck.Deck.Card;
using GameOne.Game;
using Microsoft.VisualBasic.FileIO;

namespace GameOne._Player;

public abstract class Player
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
		set => currentCardHandleVal = value % cards.Count;
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

		PlayImpl(_render);
	}

	public void GiveGard(GameCard _card) => cards.Add(_card);
	public bool PlayCards(ReadOnlySpan<GameCard> _desiredCards)
	{
		if(!IsTurn)
		{
			return false;
		}

		foreach(var _card in _desiredCards)
		{
			if(!PlayCardImpl(_card))
			{
				return false;
			}
		}

		return true;
	}
	public bool PlayCard(GameCard _desiredCard)
	{
		if(!IsTurn)
		{
			return false;
		}

		return PlayCardImpl(_desiredCard);
	}
	private bool PlayCardImpl(GameCard _card)
	{
		if(!cards.Remove(_card))
		{
			throw new Exception("Specified card does not exist in current players hand");
		}

		return manager.AddToDiscard(_card);
	}
	protected abstract void PlayImpl(Action<Player> _render);
	protected void CardAction(UserOptions _option, Action<Action<Player>> _onBufferMove, Action<Player> _render)
	{
		switch(_option)
		{
			case UserOptions.PlayCard:
				if(manager.AddToDiscard(CurrentCard))
				{
					CurrentCard.Data.Behaviour();
					cards.Remove(CurrentCard);
					break;
				}

				Console.Write("\nCan not place card, try another.");
				_onBufferMove(_render);
				break;
			case UserOptions.DrawCard:
				GiveGard(manager.GetTopCard());
				break;
			case UserOptions.Quit:
				Environment.Exit(0);
				break;
			case UserOptions.NextCard:
				currentCardHandle++;
				_render(this);
				_onBufferMove(_render);
				break;
			case UserOptions.PreviousCard:
				currentCardHandle--;
				_render(this);
				_onBufferMove(_render);
				break;
		}
	}
	public override string ToString()
	{
		return $"Name: {Name}\nScore: {Score}\n";
	}
}
