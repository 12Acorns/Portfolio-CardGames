using System.Globalization;
using System.Reflection;
using System.Text;

namespace GameLoader;

public sealed class GameLoader
{
	private static readonly string baseDirectory = AppDomain.CurrentDomain.BaseDirectory;
	private static readonly string libDirectory = Path.Combine(baseDirectory, "Lib");
	private static readonly string gamesDirectory = Path.Combine(baseDirectory, "Games");

	public GameLoader()
	{
		var _logger = new Logger("GameLoaderLog", 4096);

		var _libAssemblies = LoadAssemblies(libDirectory, _logger);
		var _gameAssemblies = LoadAssemblies(gamesDirectory, _logger);

		LogAssemblies(_libAssemblies, _logger);
		LogAssemblies(_gameAssemblies, _logger);

		var _types = GetTypes(_gameAssemblies, _logger);
		var _iGameTypes = _types.Where(type => typeof(IGame).IsAssignableFrom(type));

		LogTypes(_types, _iGameTypes, _logger);

		_logger.Flush();

		games = _iGameTypes.Select(type => (IGame)Activator.CreateInstance(type)!);
		gameNames = games.ToDictionary(x => x.Name, x => x);

		gameList = BuildGameList();
	}

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
	private string BuildGameList()
	{
		var _builder = new StringBuilder("Enter desired game (Case-Sensitive): \n");
		var _format = CompositeFormat.Parse("    -{0}\n");
		var _culture = CultureInfo.InvariantCulture;

		foreach(var _game in games)
		{
			_builder.AppendFormat(_culture, _format, _game.Name);
		}

		return _builder.ToString();
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
	private static IEnumerable<Assembly> LoadAssemblies(string _directory, Logger _logger)
	{
		var _dir = new DirectoryInfo(_directory);

		try
		{
			var _assemblies = Directory.GetFiles(_directory, "*.dll")
				.Select(Assembly.LoadFrom);

			_logger.AppendOrLog($"Successfully loaded {_dir.Name} assemblies:\n    " +
				string.Join("\n    ", _assemblies.Select(x => x.GetName().Name)) +
				"\n\n");

			return _assemblies;
		}
		catch(Exception _ex)
		{
			_logger.AppendOrLog("\n\nERROR: Could not get files ->\n    " + _ex.Message);

			Console.WriteLine("ERROR");
			Console.WriteLine($"Log details: {_logger.filePath}");

			_logger.Flush();

			throw;
		}
	}
	private static IEnumerable<Type> GetTypes(IEnumerable<Assembly> _gameAssemblies, Logger _logger)
	{
		return _gameAssemblies.SelectMany(assembly =>
		{
			try
			{
				return assembly.GetTypes();
			}
			catch(ReflectionTypeLoadException ex)
			{
				_logger.AppendOrLog("\n\nReflectionTypeLoadException:\n    " + ex.Message);

				foreach(var _loaderException in ex.LoaderExceptions)
				{
					_logger.AppendOrLog($"\n    Loader Exception: \n        {_loaderException?.Message}");
				}

				Console.WriteLine("ERROR");
				Console.WriteLine($"Log details: {_logger.filePath}");

				_logger.Flush();

				throw;
			}
		});
	}
	private static void LogTypes(IEnumerable<Type> _types, IEnumerable<Type> _gameTypes, Logger _logger)
	{
		_logger.AppendOrLog($"\n\nTypes found:\n" +
			string.Join('\n', _types.Select(type => $"    {type.FullName}")));

		_logger.AppendOrLog($"\n\nTypes that match {typeof(IGame).Name} found:\n" +
			string.Join('\n', _gameTypes.Select(type => $"    {type.FullName}")));
	}
	private static void LogAssemblies(IEnumerable<Assembly> _assemblies, Logger _logger)
	{
		_logger.AppendOrLog($"Assemblies found:\n" +
			string.Join('\n', _assemblies.Select(assembly => "    " + assembly.GetName().FullName)));

		_logger.AppendOrLog($"\n\nAssembly locations:\n" +
			string.Join('\n', _assemblies.Select(assembly => "    " + assembly.Location)));
	}
}
