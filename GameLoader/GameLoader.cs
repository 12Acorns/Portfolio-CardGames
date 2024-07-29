using System.Reflection;

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
	}

	private static readonly string gamesDirectory =
		Path.Combine(
		AppDomain.CurrentDomain.BaseDirectory,
		"Games");

	public IEnumerable<IGame> games { get; }
}
