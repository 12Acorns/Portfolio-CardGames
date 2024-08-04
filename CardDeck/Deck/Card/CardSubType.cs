using System.Diagnostics.CodeAnalysis;

namespace Deck.Deck.Card;

public readonly struct CardSubType
{
	public CardSubType(string _name)
	{
		SubTypeName = _name;
		hash = SubTypeName.GetHashCode();
	}

	public string SubTypeName { get; }

	private readonly int hash;

	public override int GetHashCode()
	{
		return hash;
	}

	public static bool operator ==(CardSubType _this, CardSubType _other)
	{
		return _this.SubTypeName == _other.SubTypeName;
	}
	public static bool operator !=(CardSubType _this, CardSubType _other)
	{
		return !(_this == _other);
	}
	public override bool Equals([NotNullWhen(true)] object? obj)
	{
		if(obj == null)
		{
			return false;
		}
		if(obj is not CardSubType _subType)
		{
			return false;
		}
		return _subType == this;
	}
}
