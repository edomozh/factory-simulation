using System;
using System.Windows.Forms;

namespace Ilka
{
	public static class WorkTime
	{
		internal static Factory Factory;
		internal static event Action WorkTimeTick;
		internal static event Action FreeTimeTick;

		internal static DateTime Time { get; private set; }
		private static Timer Timer { get; set; }

		public static void Start(Factory factory, int speed)
		{
			Factory = factory;
			Timer = new Timer();
			Timer.Interval = 60000 / speed;
			Timer.Start();
			Time = new DateTime(2018, 1, 1, 7, 0, 0);
			Timer.Tick += Tick;
		}

		public static void Stop()
		{
			Timer.Tick -= Tick;
		}

		private static void Tick(object sender, EventArgs e)
		{
			Time = Time.AddMinutes(1);
			if ((Time.Hour >= 7 && Time.Hour < 11) ||
				(Time.Hour == 11 && Time.Minute < 30) ||
				(Time.Hour == 12 && Time.Minute > 30) ||
				(Time.Hour >= 13 && Time.Hour < 16))
				WorkTimeTick?.Invoke();
			else
				FreeTimeTick?.Invoke();

			
		}

		internal static string GetTimeString()
		{
			var what = string.Empty;
			if ((Time.Hour >= 7 && Time.Hour < 11) ||
				(Time.Hour == 11 && Time.Minute < 30) ||
				(Time.Hour == 12 && Time.Minute > 30) ||
				(Time.Hour >= 13 && Time.Hour < 16))
				what = "Рабочее время";
			else
				what = "Нерабочее время";
			return $"{what} - {Time:hh:mm}";
		}
	}
}