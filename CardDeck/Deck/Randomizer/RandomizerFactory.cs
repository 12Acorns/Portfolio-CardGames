using Deck.Deck.Randomizer;

namespace Deck.Deck.Randomizer;

public static class RandomizerFactory
{
	private static readonly Dictionary<RandomizerType, IRandomizer> mappingTable = [];

	public static IRandomizer Get(RandomizerType _type) => GetAndCache(_type);

	private static IRandomizer GetAndCache(RandomizerType _type)
	{
		if(mappingTable.TryGetValue(_type, out var _randomizer))
		{
			return _randomizer;
		}

		switch(_type)
		{
			case RandomizerType.None:
				_randomizer = new NoRandomizer();
				break;
			case RandomizerType.DefualtRandom:
				_randomizer = new DefaultRandomizer();
				break;
			case RandomizerType.KnuthFisher:
				_randomizer = new KnuthFisherRandomizer();
				break;
			default:
				throw new NotSupportedException($"{_type} is not a supported randomizer option");
		}
		mappingTable.Add(_type, _randomizer);
		return _randomizer;
	}
}