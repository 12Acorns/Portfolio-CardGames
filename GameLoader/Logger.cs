using System;
using System.Reflection;
using System.Text;

namespace GameLoader;

internal sealed class Logger
{
	private static readonly string path = 
		Path.Combine(
		Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
		$"{Assembly.GetExecutingAssembly().GetName().Name}");

	public Logger(string _name, int _size)
	{
		maxCapacity = _size > 0 ? _size : throw new ArgumentOutOfRangeException(nameof(_size));

		buffer = new StringBuilder(maxCapacity);

		filePath = Path.Combine(path, _name + ".txt");

		Directory.CreateDirectory(path);
		File.Delete(filePath);
	}

	public readonly string filePath;

	private readonly StringBuilder buffer;
	private readonly int maxCapacity;

	public void Flush()
	{
		Log();
	}
	public void AppendOrLog(string contents)
	{
		if(string.IsNullOrEmpty(contents))
			return;

		var _contentsLength = contents.Length;
		var _contentsSpan = contents.AsSpan();

		// Buffer full, log contents
		if(buffer.Length >= maxCapacity)
		{
			Log();
		}

		if(_contentsLength <= maxCapacity - buffer.Length)
		{
			buffer.Append(_contentsSpan);
			return;
		}
		int _offset = 0;
		while(_offset < _contentsLength)
		{
			var _availableSpace = maxCapacity - buffer.Length;
			var lengthToCopy = Math.Min(_availableSpace, _contentsLength - _offset);

			buffer.Append(_contentsSpan.Slice(_offset, lengthToCopy));
			_offset += lengthToCopy;

			if(buffer.Length >= maxCapacity)
			{
				Log();
			}
		}
	}

	private void Log()
	{
		if(buffer.Length == 0)
		{
			return;
		}

		using var _stream = new FileStream(filePath, FileMode.Append, FileAccess.Write, FileShare.ReadWrite);
		var _bytes = Encoding.ASCII.GetBytes(buffer.ToString());
		_stream.Write(_bytes, 0, _bytes.Length);

		buffer.Clear();
	}
}
