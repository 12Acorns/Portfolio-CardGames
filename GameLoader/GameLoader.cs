using System.Globalization;
using System.Reflection;
using System.Text;

namespace GameLoader;

public sealed class GameLoader
{
	public GameLoader()
	{
		Directory.CreateDirectory(gamesDirectory);

		var _gameAssemblies = Directory.GetFiles(gamesDirectory, "*.dll")
			 .Select(Assembly.LoadFrom);

		var _assemblies = _gameAssemblies.SelectMany(assembly => assembly.GetTypes());
		var _assignable = _assemblies.Where(type => typeof(IGame).IsAssignableFrom(type));

		games = _assignable.Select(type => (IGame)Activator.CreateInstance(type)!);
		gameNames = games.ToDictionary(x => x.Name, x => x);

		var _builder = new StringBuilder("Enter desired game (Case-Sensitive): \n");
		var _format = CompositeFormat.Parse("    -{0}\n");
		var _culture = CultureInfo.InvariantCulture;

		foreach(var _game in games)
		{
			_builder.AppendFormat(_culture, _format, _game.Name);
		}

		gameList = _builder.ToString();
	}
	
	private static readonly string gamesDirectory =
		Path.Combine(
		AppDomain.CurrentDomain.BaseDirectory,
		"Games");

	private readonly Dictionary<string, IGame> gameNames;
	private readonly IEnumerable<IGame> games;
	private readonly string gameList;


	public void Run()
	{
		Console.Write(gameList);
		Console.Write('>');

		var _runGame = AskGame();
		_runGame.Run();
	}

	private IGame AskGame()
	{
		var _desiredGame = Console.ReadLine() ?? "";

		if(!gameNames.TryGetValue(_desiredGame, out var _game))
		{
			Console.WriteLine("\nPlease enter a valid game name!\n");
			Console.Write(gameList);
			Console.Write('>');
			return AskGame();
		}
		return _game;
	}
}
