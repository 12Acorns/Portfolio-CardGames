namespace CardGameConsoleApp.Deck.Card.Colour;

internal interface IColourSet
{
	public static readonly IColourSet Default = new ClassicColours();

	public IEnumerable<RGBColour> GetColours();
}
