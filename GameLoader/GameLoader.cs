using System.Globalization;
using System.Reflection;
using System.Text;

namespace GameLoader;

public sealed class GameLoader
{
	public GameLoader()
	{
		using var _logger = new Logger("GameLoaderLog", 4096);

		try
		{
			Directory.CreateDirectory(gamesDirectory);
		}
		catch(Exception _ex)
		{
			_logger.AppendOrLog("\n\nERROR: Could not create directory ->\n    Target Path: " +
				$"{gamesDirectory}\n    Exception: {_ex.Message}");

			Console.WriteLine("ERROR");
			Console.WriteLine($"Log details: {_logger.filePath}");

			throw;
		}


		IEnumerable<Assembly> _gameAssemblies = [];

		try
		{
			_gameAssemblies = Directory.GetFiles(gamesDirectory, "*.dll")
				.Select(Assembly.LoadFrom);
		}
		catch(Exception _ex)
		{
			_logger.AppendOrLog("\n\nERROR: Could not get files ->\n    " + _ex.Message);

			Console.WriteLine("ERROR");
			Console.WriteLine($"Log details: {_logger.filePath}");

			throw;
		}

		_logger.AppendOrLog($"Assemblies found:\n" +
			string.Join('\n', _gameAssemblies.Select(x => "    " + x.GetName().FullName)));

		_logger.AppendOrLog($"\n\nAssembly locations:\n" +
			string.Join('\n', _gameAssemblies.Select(x => "    " + x.Location)));

		var _types = _gameAssemblies.SelectMany(assembly => assembly.GetTypes());

		_logger.AppendOrLog($"\n\nTypes found:\n" +
			string.Join('\n', _types.Select(x => $"    {x.FullName}")));

		var _iGameTypes = _types.Where(type => typeof(IGame).IsAssignableFrom(type));

		_logger.AppendOrLog($"\n\nTypes that match {typeof(IGame).Name} found:\n" +
			string.Join('\n', _iGameTypes.Select(x => $"    {x.FullName}")));

		games = _iGameTypes.Select(type => (IGame)Activator.CreateInstance(type)!);
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
