using FastEnumUtility;
using Deck.Deck.Card;
using System.Text;

namespace Renderer;

internal static class CardRender
{
	private const string CARD =
		"""
		│─────────────────│
		│                 │
		│                 │
		│                 │
		│                 │
		│                 │
		│                 │
		│                 │
		│                 │
		│                 │
		│                 │
		│─────────────────│
		""";
	private const int CARDLENGTH = 19;
	private static readonly int cardHeight = CARD.Length / CARDLENGTH;

	public static string Render(GameCard _card)
	{
						 // Middle Row - Offset to previous row + Offset to start of middle row
		var _centreIndex = CARDLENGTH * cardHeight / 2 - CARDLENGTH + 1;

		var _cardType = _card.Data.SubType;
		var _cardTypeLength = _cardType.FastToString().Length;

		var _beginIndex = (CARDLENGTH - _cardTypeLength) / 2;

		var _builder = new StringBuilder(CARD.Length);
		_builder.Append(CARD);

		_builder.Insert(_centreIndex + _beginIndex, _cardType);
		_builder.Remove(_centreIndex + _beginIndex + _cardTypeLength, _cardTypeLength);

		return _builder.ToString();
	}
}
