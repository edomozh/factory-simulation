using System;
using System.Collections.Generic;

namespace Ilka
{
    /// <summary>
    /// 2 цех токарные, сверлильные, сварочные
    /// </summary>
    internal class Workshop2 : Workshop
    {
        /// <summary>
        /// Сколько минут занимает токарная операция.
        /// </summary>
        private int Turning { get; set; }

        /// <summary>
        /// Сколько минут занимает сверлильная операция.
        /// </summary>
        private int Drilling { get; set; }

        /// <summary>
        /// Сколько минут сварочная операция.
        /// </summary>
        private int Welding { get; set; }

        internal override void Start()
        {
            Turning = int.Parse(Factory.textBox4.Text);
            Drilling = int.Parse(Factory.textBox5.Text);
            Welding = int.Parse(Factory.textBox6.Text);

            WorkTime.WorkTimeTick += WorkMinute;
            Factory.Logger.WriteLine(
                $"{Name} запущен. Токарная операция {Turning} мин." +
                $"Сверлильная операция {Drilling} мин. Сварочная операция {Welding} мин.");
        }

        internal override void WorkMinute()
        {
            if (Detail == null)
            {
                GetDetailFromFactory();
                if (Detail == null) return;

                var operation = Detail.GetCurrentOperation();
                switch (operation)
                {
                    case DetailOperation.Lathe: Leftover = DurOperation = Turning; Factory.checkBox4.Checked = true; break;
                    case DetailOperation.Drilling: Leftover = DurOperation = Drilling; Factory.checkBox5.Checked = true; break;
                    case DetailOperation.Welding: Leftover = DurOperation = Welding; Factory.checkBox6.Checked = true; break;
                    default: throw new Exception("Недопустимая операция для цеха.");
                }
                Detail.StartOperation(this, operation);
            }
            else
            {
                OnProgress(Leftover, DurOperation);
                if (Leftover > 1) Leftover--;
                else
                {
                    Factory.checkBox4.Checked = false;
                    Factory.checkBox5.Checked = false;
                    Factory.checkBox6.Checked = false;

                    Detail.FinishOperation(this);
                    Factory.listBox1.Items.Add(Detail);
                    Detail = null;
                }
            }
        }

        public Workshop2(Factory factory, string name) : base(factory, name)
        {
            //токарные, сверлильные, сварочные
            Operations = new List<DetailOperation>
            {
                DetailOperation.Lathe,
                DetailOperation.Drilling,
                DetailOperation.Welding
            };
        }
    }
}