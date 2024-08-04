using System.Collections.Frozen;
using System.Diagnostics.CodeAnalysis;

namespace Deck.Deck.Card;

public readonly struct CardType
{
	public CardType(string _name, FrozenSet<CardSubType> _typeMembers)
	{
		TypeName = _name;
		TypeMembers = _typeMembers;
		var _hash = new HashCode();
		_hash.Add(TypeName);

		for(int i = 0; i < TypeMembers.Count; i++)
		{
			_hash.Add(TypeMembers.ElementAt(i));
		}

		hash = _hash.ToHashCode();
	}
	public CardType(string _name, HashSet<CardSubType> _typeMembers) : this(_name, _typeMembers.ToFrozenSet()) { }
	public CardType(string _name, CardSubType[] _typeMembers) : this(_name, _typeMembers.ToFrozenSet()) { }

	public string TypeName { get; }
	public FrozenSet<CardSubType> TypeMembers { get; }

	private readonly int hash;

	public override int GetHashCode()
	{
		return hash;
	}

	public static bool operator ==(CardType _this, CardType _other)
	{
		return _this.hash == _other.hash;
	}
	public static bool operator !=(CardType _this, CardType _other)
	{
		return !(_this.hash == _other.hash);
	}
	public override bool Equals([NotNullWhen(true)] object? _obj)
	{
		if(_obj == null)
		{
			return false;
		}
		if(_obj is not CardType _cardType)
		{
			return false;
		}
		return _cardType == this;
	}
}