namespace Deck.Deck.Card;

public record struct GameCard
{
	public GameCard(CardDescription _description, CardData _data)
	{
		Description = _description;
		Data = _data;

		if(!_description.Type.TypeMembers.Contains(_data.SubType))
		{
			throw new ArgumentException($"Member ({_data.SubType}) does not inherit from {_description.Type.TypeName}.");
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
