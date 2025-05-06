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
	}

	/// <summary>
	/// Description of the card group
	/// </summary>
	public CardDescription Description { get; set; }
	/// <summary>
	/// Data of this card
	/// </summary>
	public readonly CardData Data { get; }
	public bool InUse { get; private set; }

	/// <summary>
	/// Set card to be in use
	/// </summary>
	public void PickUp()
	{
		InUse = true;
	}
	/// <summary>
	/// Set card to be no longer in use
	/// </summary>
	public void PutDown()
	{
		InUse = false;
	}

	public override readonly int GetHashCode() => HashCode.Combine(Description.Colour, Data.SubType, Data.Score);
}
