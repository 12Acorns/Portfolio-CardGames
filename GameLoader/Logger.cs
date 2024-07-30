using System.Reflection;
using System.Text;

namespace GameLoader;

internal sealed class Logger : IDisposable
{
	private static readonly SemaphoreSlim lockObj = new(1, 1);
	private static readonly string path = 
		Path.Combine(
		Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
		$"{Assembly.GetExecutingAssembly().GetName().Name}");

	public Logger(string _name, int _size)
	{
		bufferSize = _size;
		buffer = new char[bufferSize];
		bufferHandle = 0;

		filePath = Path.Combine(path, _name + ".txt");

		Directory.CreateDirectory(path);
		File.Delete(filePath);
	}

	public readonly string filePath;

	private readonly int bufferSize;
	private readonly char[] buffer;
	private int bufferHandle;

	private bool disposedValue;

	public async void AppendOrLog(string _contents)
	{
		await lockObj.WaitAsync();

		var _contentsLength = _contents.Length;

		// Buffer full, log contents
		if(bufferHandle == buffer.Length - 1)
		{
			await Log(bufferSize);
		}

		// Trying to append more than supported contents
		int _offset = 0;
		while(bufferHandle - _contentsLength < 0)
		{
			var _bufferHandleStart = bufferHandle;
			for(; bufferHandle < buffer.Length; bufferHandle++, _contentsLength--)
			{
				if(_contentsLength == 0)
				{
					break;
				}

				buffer[bufferHandle] = _contents[bufferHandle - _bufferHandleStart + _offset];
			}
			await Log(bufferSize - (bufferSize - bufferHandle));
			_offset += bufferSize - 1;
		}

		lockObj.Release();
	}

	private async Task Log(int _size)
	{
		using var _steam = new FileStream(filePath, FileMode.Append, FileAccess.Write, FileShare.ReadWrite, bufferSize, false);

		var _bytes = Encoding.ASCII.GetBytes(buffer);

		await _steam.WriteAsync(_bytes.AsMemory(0, _size));

		bufferHandle = 0;

		return;
	}

	private void Dispose(bool disposing)
	{
		if(disposedValue)
		{
			return;
		}
		disposedValue = true;
		lockObj.Dispose();
	}

	~Logger()
	{
		Dispose(false);
	}

	public void Dispose()
	{
		Dispose(true);
		GC.SuppressFinalize(this);
	}
}
