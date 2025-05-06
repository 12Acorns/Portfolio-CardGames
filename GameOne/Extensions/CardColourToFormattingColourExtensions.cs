using Deck.Deck.Card.Colour;

namespace GameOne.Extensions;

internal static class CardColourToFormattingColourExtensions
{
	public static string ToFormattingColour(this RGBColour _colour)
	{
		if(_colour == RGBColour.None) return "[No UnderLine, No Invert, No Bold, FG: Default, BG: Default]";
		if(_colour == RGBColour.Green) return "[FG: Green]";
		if(_colour == RGBColour.Red) return "[FG: Red]";
		if(_colour == RGBColour.Yellow) return "[FG: Yellow]";
		if(_colour == RGBColour.Blue) return "[FG: Blue]";
		if(_colour == RGBColour.White) return "[FG: White]";
		if(_colour == RGBColour.Black) return "[FG: Black]";
		return "[No UnderLine, No Invert, No Bold, FG: Default, BG: Default]";
	}
}
