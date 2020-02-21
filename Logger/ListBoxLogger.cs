using System.Windows.Forms;

namespace Ilka
{
	public class ListBoxLogger
	{
		ListBox lb;

		public ListBoxLogger(ListBox listBox)
		{
			lb = listBox;
		}

		public void WriteLine(string str)
		{
			lb.Items.Add(WorkTime.Time.ToString("hh:mm") + " " +  str);
		}
	}
}