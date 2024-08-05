using GameOne._Player;
using System.Text;

namespace Renderer;

internal static class PlayerRender
{
	private const string NAME =  " Name: ";
	private const string SCORE = "Score: ";
	private const int PADDINGBETWEENNAMES = 2;
	private const int PADDINGBETWEENSCORES = 2;

	public static string FormatPlayers(ReadOnlySpan<Player> _players, Player _currentPlayer)
	{
		var _upperOutput = new StringBuilder("|");
		var _lowerOutput = new StringBuilder("|");
		foreach(var _player in _players)
		{
			var _name = _player.Name;
			var _score = _player.SumOfCardsScores;

			var _nameLength = _name.Length;
			var _totalNameSegmentLength = NAME.Length + _nameLength;
			// Three is maximum length of a byte (in string form)
			var _totalScoreSegmentLength = _score.Length() + SCORE.Length;
			var _difference = _totalNameSegmentLength - _totalScoreSegmentLength;

			var _upperSegment = 
				new StringBuilder(_totalNameSegmentLength + PADDINGBETWEENNAMES);
			var _lowerSegment = 
				new StringBuilder(_totalScoreSegmentLength + _difference + PADDINGBETWEENSCORES);

			if(_player == _currentPlayer)
			{
				_upperSegment.Append(">>");
				_lowerSegment.Append(">>");
			}

			_upperSegment.AppendFormat("|  {0}{1}{2}|", 
					NAME, _name, new string(' ', PADDINGBETWEENNAMES));

			_lowerSegment.AppendFormat("|  {0}{1}{2}|",
				SCORE, _score, new string(' ', _difference + PADDINGBETWEENSCORES));

			if(_player == _currentPlayer)
			{
				_upperSegment.Append("<<");
				_lowerSegment.Append("<<");
			}

			_upperOutput.Append(_upperSegment);
			_lowerOutput.Append(_lowerSegment);
		}
		_upperOutput.Append('|');
		_lowerOutput.Append('|');
		return _upperOutput.ToString() + "\n" + _lowerOutput.ToString();
	}
}
