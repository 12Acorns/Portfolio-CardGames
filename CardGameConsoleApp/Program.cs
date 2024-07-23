
using CardGameConsoleApp.Deck.Card;
using CardGameConsoleApp.Deck;
using CardGameConsoleApp.Deck.Randomizer;

var _deck =
	new CardDeckBuilder()
		.WithCustomDeckOptions(DeckOptions.Default)
		.WithCustomRandomizeOptions(new NoRandomizer())
		.Build();

int _sum = 0;
while(_deck.RemainingCards >= 1)
{
	if(_deck.Peek().Description.Type is CardType.Numeric)
	{
		_sum += _deck.Peek().Data.Score;
	}

	Console.WriteLine(_deck.RemainingCards);
	Console.WriteLine(Print(ref _deck));
	Console.WriteLine(_deck.RemainingCards);
}

Console.WriteLine("Sum: {0}", _sum);

Console.ReadLine();

static string Print(ref CardDeck _deck)
{
	_deck.TryNext(out var _card);
	return string.Format("Type: {0}\nColour: {1}\nScore: {2}",
		_card.Description.Type, _card.Description.Colour, _card.Data.Score);
}