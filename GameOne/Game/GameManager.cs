using Deck.Deck.Randomizer;
using GameOne._Player;
using Deck.Deck;
using Renderer;

namespace GameOne.Game;
public sealed class GameManager
{
	private const int PLAYERS = 4;
	private const int STARTINGCARDS = 7;

	public GameManager()
	{
		var _deck = new CardDeckBuilder()
			.WithCustomShuffleOptions(RandomizerFactory.Get(RandomizerType.KnuthFisher))
			.WithCustomDeckOptions(DeckOptions.Default)
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
		if(playing)
		{
			return;
		}
		playing = true;

		bool _quit = false;

		var _nonAI = manager.NonAI!;

		while(!_quit)
		{
			Render(_nonAI);

			manager.CurrentPlayer.Play(Render);

			manager.NextPlayer();
		}

		playing = false;
	}
	public void Render(Player _player)
	{
		renderer.Render(
			manager.CurrentPlayer,
			_player.CurrentCard,
			manager.PeekDiscardPileTopCard());
	}
}
