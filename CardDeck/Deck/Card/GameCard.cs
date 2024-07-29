namespace Deck.Deck.Card;

public record struct GameCard
{
	public GameCard(CardDescription _description, CardData _data)
	{
		Description = _description;
		Data = _data;

		if(Description.Type is CardType.Numeric &&
			_data.SubType is not (CardSubType.Zero or CardSubType.One
			   or CardSubType.Two or CardSubType.Three
			   or CardSubType.Four or CardSubType.Five
			   or CardSubType.Six or CardSubType.Seven
			   or CardSubType.Eight or CardSubType.Nine))
		{
			throw new ArgumentException("subType is not a valid numeric type");
		}
		if(_description.Type is CardType.Special &&
			_data.SubType is not (CardSubType.Skip or CardSubType.Reverse
						 or CardSubType.PlusTwo or CardSubType.WildPlusFour
						 or CardSubType.Wild))
		{
			throw new ArgumentException("subType is not a valid special type");
		}

		hash = HashCode.Combine(Description.Colour, Data.SubType, Data.Score);
	}

	private readonly int hash;

	/// <summary>
	/// Description of the card group
	/// </summary>
	public CardDescription Description { get; set; }
	/// <summary>
	/// Data of this card
	/// </summary>
	public readonly CardData Data { get; }
	public bool InUse { get; private set; }

	public void PickUp()
	{
		InUse = true;
	}
	public void PutDown()
	{
		InUse = false;
	}

	public override readonly int GetHashCode() => hash;
}
