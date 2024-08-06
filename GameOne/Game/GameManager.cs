using Deck.Deck.Randomizer;
using GameOne._Player;
using Deck.Deck;
using Renderer;
using Deck.Extensions;

namespace GameOne.Game;

internal sealed class GameManager
{
	private const int PLAYERS = 4;
	private const int STARTINGCARDS = 7;

	private static readonly object lockObject = new();

	private readonly ManualResetEvent threadEvent = new(false);

	public GameManager()
	{
		var _options = new DeckOptions([DeckFactory.GetDefaultNumericDescription(), DeckFactory.GetDefaultSpecialDescription()]);

		var _deck = new CardDeckBuilder(_options, CardExtensions.MapToScore)
			.WithCustomShuffleOptions(RandomizerFactory.Get(RandomizerType.KnuthFisher))
			.Build();

		manager = new(_deck, PLAYERS, STARTINGCARDS);

		players = new Player[PLAYERS];

		// Once fully iterated, will point back to first player
		for(int i = 0; i < PLAYERS; i++)
		{
			players[i] = manager.NextPlayer();
		}

		renderer = new(players, manager.NonAI);
	}

	private bool playing;
	private readonly Player[] players;
	private readonly RoundManager manager;
	private readonly GameRenderer renderer;

	public void Run()
	{
		var _winner = manager.AllPlayers().FirstOrDefault(x => x.Score > 500);
		if(_winner != null)
		{
			Console.WriteLine($"{manager.CurrentPlayer} has won the game");
			return;
		}

		if(playing)
		{
			return;
		}
		playing = true;

		var _gameThread = new Thread(Game);
		_gameThread.Start();

		while(!manager.GameOver) 
		{
			threadEvent.Set();
		}

		threadEvent.Reset();

		var _winningPlayer = manager.CurrentPlayer;

		Console.WriteLine(_winningPlayer.Name + $" Won\nPoints: {_winningPlayer.Score}");

		playing = false;
		Run();
	}
	public void Render(Player _player)
	{
		renderer.Render(
			manager.CurrentPlayer,
			_player.CurrentCard,
			manager.PeekDiscardPileTopCard());
	}

	private void Game()
	{
		var _nonAI = manager.NonAI;
		lock(lockObject)
		{
			while(true)
			{
				threadEvent.WaitOne();

				Render(_nonAI);
				manager.CurrentPlayer.Play(Render);

				if(manager.GameOver)
				{
					break;
				}

				manager.NextPlayer();
			}
		}
		if(manager.AllPlayers().Any(x => x.Score > 500))
		{
			return;
		}
		Game();
	}
}
