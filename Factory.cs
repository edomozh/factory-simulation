namespace Ilka
{
    using Ilka.Extensions;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Windows.Forms;

    public partial class Factory : Form
    {
        internal List<object> Log;
        internal ListBoxLogger Logger;
        internal Timer Timer;
        internal Random Random;

        internal Workshop1 Workshop1;
        internal Workshop2 Workshop2;
        internal Workshop3 Workshop3;
        internal Workshop4 Workshop4;
        internal Workshop5 Workshop5;

        public Factory()
        {
            InitializeComponent();
            Log = new List<object>();
            Logger = new ListBoxLogger(listBox2);
            Timer = new Timer();
            Random = new Random();

            progressBar1.SetState(2);
            progressBar2.SetState(2);
            progressBar3.SetState(2);
            progressBar4.SetState(2);
            progressBar5.SetState(2);
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            Workshop1 = new Workshop1(this, groupBox1.Text);
            Workshop1.Progress += i => progressBar1.Value = i;

            Workshop2 = new Workshop2(this, groupBox2.Text);
            Workshop2.Progress += i => progressBar2.Value = i;

            Workshop3 = new Workshop3(this, groupBox3.Text);
            Workshop3.Progress += i => progressBar3.Value = i;

            Workshop4 = new Workshop4(this, groupBox4.Text);
            Workshop4.Progress += i => progressBar4.Value = i;

            Workshop5 = new Workshop5(this, groupBox5.Text);
            Workshop5.Progress += i => progressBar5.Value = i;

            foreach (var c in Controls)
            {
                if (c is GroupBox)
                    foreach (var t in (c as GroupBox).Controls)
                        if (t is TextBox) (t as TextBox).Text = Random.Next(10, 60).ToString();
            }

            foreach (var i in Enum.GetValues(typeof(DetailOperation)))
                listBox4.Items.Add(new Stage((DetailOperation)i, 0));
        }


        private void button1_Click(object sender, EventArgs e)
        {
            var number = int.Parse(textBoxDetail1.Text);
            for (var i = 0; i < number; i++)
            {
                var det = new Detail(1);
                listBox1.Items.Add(det);
            }
            Logger.WriteLine($"В очередь на производство добавлены детали. {number} шт.");
            listBox1.SelectedIndex = listBox1.Items.Count - 1;
            listBox2.SelectedIndex = listBox2.Items.Count - 1;
        }

        private void textBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!Char.IsDigit(e.KeyChar)) e.Handled = true;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            var number = int.Parse(textBoxDetail1.Text);
            for (var i = 0; i < number; i++)
            {
                var det = new Detail(2);
                listBox1.Items.Add(det);
            }
            Logger.WriteLine($"В очередь на производство добавлены детали. {number} шт.");
            listBox1.SelectedIndex = listBox1.Items.Count - 1;
            listBox2.SelectedIndex = listBox2.Items.Count - 1;
        }

        private void button6_Click(object sender, EventArgs e)
        {
            if (listBox3.Items.Count < 1)
            {
                MessageBox.Show("Для детали должна быть задана как минимум одна стадия.");
                return;
            }

            var stages = new Dictionary<int, DetailOperation>();
            foreach (Stage stage in listBox3.Items)
                stages.Add(stage.Number, stage.Operation);

            var number = int.Parse(textBoxDetail1.Text);
            for (var i = 0; i < number; i++)
            {
                var det = new Detail(stages, textBox14.Text);
                listBox1.Items.Add(det);
            }
            Logger.WriteLine($"В очередь на производство добавлены детали. {number} шт.");
            listBox1.SelectedIndex = listBox1.Items.Count - 1;
            listBox2.SelectedIndex = listBox2.Items.Count - 1;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            listBox2.Items.Clear();
            foreach (var c in Controls)
            {
                if (c is TextBox) (c as TextBox).Enabled = false;
                if (c is GroupBox)
                    foreach (var t in (c as GroupBox).Controls)
                        if (t is TextBox) (t as TextBox).Enabled = false;
            }
            button1.Enabled = false;
            button2.Enabled = false;
            button3.Enabled = false;
            button4.Enabled = true;

            WorkTime.Start(this, int.Parse(textBox0.Text));
            Logger.WriteLine("Производство запущенно.");
            WorkTime.WorkTimeTick += ShowWorkTime;
            WorkTime.FreeTimeTick += ShowFreeTime;
            WorkTime.WorkTimeTick += CheckListBox;
            Workshop1.Start();
            Workshop2.Start();
            Workshop3.Start();
            Workshop4.Start();
            Workshop5.Start();
        }

        private void CheckListBox()
        {
            if (Workshop.GetDetails(listBox1).Any(d => !d.Completed) ||
                !(Workshop1.Detail == null && Workshop2.Detail == null && Workshop3.Detail == null &&
                Workshop4.Detail == null && Workshop5.Detail == null)) return;
            {
                Logger.WriteLine("Все детали на складе готовы. Все цеха свободны.");
                button4_Click(this, new EventArgs());
            }
        }

        private void ShowFreeTime()
        {
            Text = WorkTime.GetTimeString();
        }

        private void ShowWorkTime()
        {
            Text = WorkTime.GetTimeString();
            if (WorkTime.Time.Hour <= 16 && WorkTime.Time.Hour >= 7)
                ProgressBar.Value = (WorkTime.Time.Hour - 7) * 60 + WorkTime.Time.Minute;
        }



        private void button4_Click(object sender, EventArgs e)
        {
            Logger.WriteLine("Производство остановлено.");
            WorkTime.Stop();
            button1.Enabled = true;
            button2.Enabled = true;
            button3.Enabled = true;
            button4.Enabled = false;
            foreach (var c in Controls)
            {
                if (c is TextBox) (c as TextBox).Enabled = true;
                if (c is GroupBox)
                    foreach (var t in (c as GroupBox).Controls)
                        if (t is TextBox) (t as TextBox).Enabled = true;
            }
            WorkTime.WorkTimeTick -= ShowWorkTime;
            WorkTime.FreeTimeTick -= ShowFreeTime;
            WorkTime.WorkTimeTick -= CheckListBox;
        }

        private void button5_Click(object sender, EventArgs e)
        {
            listBox1.Items.Clear();
            listBox2.Items.Clear();
        }


        private void listBox1_DoubleClick(object sender, EventArgs e)
        {
            if (Log.Count == 0)
                foreach (var i in listBox2.Items) Log.Add(i);
            listBox2.Items.Clear();

            foreach (var i in (listBox1.SelectedItem as Detail).Log)
                listBox2.Items.Add(i);
        }

        private void listBox1_MouseLeave(object sender, EventArgs e)
        {
            if (Log.Count == 0) return;
            listBox2.Items.Clear();
            foreach (var i in Log) listBox2.Items.Add(i);
            Log.Clear();
        }

        private void listBox4_DoubleClick(object sender, EventArgs e)
        {
            var select1 = listBox4.Items[listBox4.SelectedIndex] as Stage;
            select1.Number = listBox3.Items.Count + 1;
            listBox3.Items.Add(new Stage(select1.Operation, select1.Number));
        }

    }
}
