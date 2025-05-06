using Deck.Deck.Card;
using Deck.Extensions;
using GameOne.Extensions;
using NEG.CTF2.Core;
using System.Text;

namespace Renderer;

internal static class CardRender
{
	private const char CARDSIDEBORDERLINE = '│';
	private const char CARDTOPORBOTTOMBORDERLINE = '─';
	private const char CARDEMPTYSPACE = ' ';

	private static readonly string card = BuildCard();

	private const int CARDWIDTH = 19;
	private const int CARDHEIGHT = 11;

	public static string Render(GameCard _card)
	{
		var _cardWithColour = QuickFormat.Format(_card.Description.Colour.ToFormattingColour() + card);
		var _diff = _cardWithColour.Length - card.Length;

		// Middle Row - Offset to previous row + Offset to start of middle row
		var _cardType = _card.Data.SubType.ReadableNameMapping();
		var _cardTypeLength = _cardType.Length;

		var _centreIndex = (CARDWIDTH * CARDHEIGHT) / 2 + _diff - (_cardTypeLength / 2);

		var _beginIndex = (CARDWIDTH - _cardTypeLength) / 2;

		var _builder = new StringBuilder(_cardWithColour.Length);
		_builder.Append(_cardWithColour);

		_builder.Insert(_centreIndex + _beginIndex, _cardType);
		_builder.Remove(_centreIndex + _beginIndex + _cardTypeLength, _cardTypeLength);
		var _colourString = $"\n Colour: {QuickFormat.Format(_card.Description.Colour.ToFormattingColour() + _card.Description.Colour.Name)}";
		_builder.Append(_colourString);

		return _builder.ToString();
	}
	private static string BuildCard()
	{
		if(CARDWIDTH <= 2)
		{
			throw new Exception(nameof(CARDWIDTH) + " is too short, increase width to be > 2");
		}

		var _topBottomBorder = $"{CARDSIDEBORDERLINE}{new string(CARDTOPORBOTTOMBORDERLINE, CARDWIDTH - 2)}{CARDSIDEBORDERLINE}";

		var _card = new StringBuilder(CARDWIDTH * CARDHEIGHT + (2 * CARDHEIGHT));
		_card.AppendLine(_topBottomBorder);
		for(int i = 0; i < CARDHEIGHT - 2; i++)
		{
			_card.AppendLine($"{CARDSIDEBORDERLINE}{new string(CARDEMPTYSPACE, CARDWIDTH - 2)}{CARDSIDEBORDERLINE}");
		}
		return _card
			.AppendLine(_topBottomBorder)
			.ToString();
	}
}
