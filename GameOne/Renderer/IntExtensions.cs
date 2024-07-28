namespace Renderer;

internal static class IntExtensions
{
	public static int Length(this int _input)
	{
		ArgumentOutOfRangeException.ThrowIfNegative(_input);
		if(_input == 0)
		{
			return 1;
		}

		return (int)Math.Floor(Math.Log10(_input)) + 1;
	}
}
