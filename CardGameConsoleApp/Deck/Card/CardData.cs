namespace CardGameConsoleApp.Deck.Card;

internal record CardData
{
    public CardData(CardType _type, CardColour _colour)
    {
        Type = _type;
        Colour = _colour;
    }

    public CardType Type { get; }
    public CardColour Colour { get; }
}
