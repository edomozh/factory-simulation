namespace Ilka
{
	public static class IdGenerator
	{
		private static int _count = 0;

		public static int GetId()
		{
			if (_count == int.MaxValue) _count = 0;
			return _count++;
		}
	}
}