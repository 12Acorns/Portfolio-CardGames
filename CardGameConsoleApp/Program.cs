using GameLoader;

var _loader = new GameLoader.GameLoader();

var _games = _loader.games;

Console.WriteLine("Enter desired game: ");

foreach(var _game in _games)
{
	Console.Write($"    {_game.Name}\n");
}
Console.Write('>');

var _desiredGame = Console.ReadLine() ?? "";

IGame? _runGame = null;

foreach(var _game in _games)
{
	if(_desiredGame != _game.Name)
	{
		continue;
	}
	_runGame = _game;
	break;
}

if(_runGame == null)
{
	throw new NullReferenceException();
}

_runGame.Run();