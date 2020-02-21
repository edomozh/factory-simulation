using System.IO;

namespace Ilka
{
	public class FileLogger
	{
		StreamWriter sw;

		public FileLogger(string path)
		{
			sw = new StreamWriter(path);
		}

		public void WriteLine(string str)
		{
			sw.WriteLine(str);
			sw.Flush();
		}
	}
}