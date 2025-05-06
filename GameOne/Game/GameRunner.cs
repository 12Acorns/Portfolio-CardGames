using GameLoader;

namespace GameOne.Game;

public sealed class GameRunner : IGame
{
	public string Name { get; } = "Card Game One";

	public void Run()
	{
		var _manager = new GameManager();
		

		_manager.Run();
	}
}
