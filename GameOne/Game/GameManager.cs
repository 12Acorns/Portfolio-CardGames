using System.Runtime.CompilerServices;
using Deck.Deck.Randomizer;
using Deck.Extensions;
using GameOne._Player;
using Deck.Deck.Card;
using Deck.Deck;
using Renderer;

namespace GameOne.Game;

internal sealed class GameManager
{
	private const int PLAYERS = 4;
	private const int STARTINGCARDS = 7;
	private const bool AIONLY = true;

	private static readonly Lock _lockObject = new();

	private readonly ManualResetEvent _threadEvent = new(false);

	public GameManager()
	{
		var _options = new DeckOptions(
		[
			DeckFactory.GetDefaultNumericDescription(), 
			DeckFactory.GetDefaultSpecialDescription()
		]);

		var _deck = new CardDeckBuilder(_options, CardExtensions.MapToScore)
			.WithCustomShuffleOptions(RandomizerFactory.Get(RandomizerType.KnuthFisher))
			.Build();

		_manager = new(_deck, PLAYERS, STARTINGCARDS, AIONLY);

		_players = new Player[PLAYERS];

		// Once fully iterated, will point back to first player
		for(int i = 0; i < PLAYERS; i++)
		{
			_players[i] = _manager.NextPlayer();
		}

		_renderer = new(_players, _manager.NonAI, AIONLY);

		_gameThread = new(Game);
		_gameThread.Start();
	}

	private bool playing;
	private readonly Player[] _players;
	private readonly Thread _gameThread;
	private readonly RoundManager _manager;
	private readonly GameRenderer _renderer;

	public void Run()
	{
		var winner = _players.FirstOrDefault(x => x.Score >= 500);
		if(winner != null)
		{
			Console.WriteLine($"{_manager.CurrentPlayer} has won the game");
			return;
		}

		if(playing)
		{
			return;
		}
		playing = true;

		while(!_manager.RoundOver) 
		{
			_threadEvent.Set();
		}

		_threadEvent.Reset();

		var winningPlayer = _manager.CurrentPlayer;

		Console.WriteLine(winningPlayer.Name + $" Won\nPoints: {winningPlayer.Score}");
		playing = false;
		_manager.NewRound(STARTINGCARDS);
		Run();
	}
	public void Render(Player? _player)
	{
		_renderer.Render(
			_manager.CurrentPlayer,
			_player?.CurrentCard,
			_manager.PeekDiscardPileTopCard());
	}

	private void Game()
	{
		_threadEvent.WaitOne();

		var nonAI = _manager.NonAI;
		lock(_lockObject)
		{
			while(true)
			{
				_threadEvent.WaitOne();

				Render(nonAI);
				_manager.CurrentPlayer.Play(Render);

				if(_manager.RoundOver)
				{
					break;
				}

				_manager.NextPlayer();
			}
		}
		if(_manager.AllPlayers().Any(x => x.Score >= 500))
		{
			return;
		}
		Game();
	}
}
