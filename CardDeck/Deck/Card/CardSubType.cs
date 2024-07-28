namespace Deck.Deck.Card;

public enum CardSubType : byte
{
	// Numeric
	Zero,
	One,
	Two,
	Three,
	Four,
	Five,
	Six,
	Seven,
	Eight,
	Nine,

	// Special
	PlusTwo,
	Skip,
	Reverse,

	// Wild (Special
	Wild,
	WildPlusFour,
}
