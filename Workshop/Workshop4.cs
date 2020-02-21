using System;
using System.Collections.Generic;

namespace Ilka
{
    /// <summary>
    /// 4 цех грунтовка, покраска, сушка
    /// </summary>
    internal class Workshop4 : Workshop
    {
        /// <summary>
        /// Сколько минут занимает грунтовка.
        /// </summary>
        private int Priming { get; set; }

        /// <summary>
        /// Сколько минут занимает сушка.
        /// </summary>
        private int Painting { get; set; }

        /// <summary>
        /// Сколько минут сварочная операция.
        /// </summary>
        private int Drying { get; set; }

        internal override void Start()
        {
            Priming = int.Parse(Factory.textBox9.Text);
            Painting = int.Parse(Factory.textBox10.Text);
            Drying = int.Parse(Factory.textBox11.Text);

            WorkTime.WorkTimeTick += WorkMinute;
            Factory.Logger.WriteLine(
                $"{Name} запущен. Грунтовка занимает {Priming} мин." +
                $"Покраска занимает {Painting} мин. Сушка занимает {Drying} мин.");
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
                    case DetailOperation.Priming: Leftover = DurOperation = Priming; Factory.checkBox9.Checked = true; break;
                    case DetailOperation.Painting: Leftover = DurOperation = Painting; Factory.checkBox10.Checked = true; break;
                    case DetailOperation.Drying: Leftover = DurOperation = Drying; Factory.checkBox11.Checked = true; break;
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
                    Factory.checkBox9.Checked = false;
                    Factory.checkBox10.Checked = false;
                    Factory.checkBox11.Checked = false;

                    Detail.FinishOperation(this);
                    Factory.listBox1.Items.Add(Detail);
                    Detail = null;
                }
            }
        }

        public Workshop4(Factory factory, string name) : base(factory, name)
        {
            //грунтовка, покраска, сушка
            Operations = new List<DetailOperation>
            {
                DetailOperation.Priming,
                DetailOperation.Painting,
                DetailOperation.Drying
            };
        }
    }
}