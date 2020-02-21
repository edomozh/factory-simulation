using System;
using System.Collections.Generic;

namespace Ilka
{
    /// <summary>
    /// 5 цех покраска, сушка 
    /// </summary>
    internal class Workshop5 : Workshop
    {
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
            Painting = int.Parse(Factory.textBox12.Text);
            Drying = int.Parse(Factory.textBox13.Text);

            WorkTime.WorkTimeTick += WorkMinute;
            Factory.Logger.WriteLine(
                $"{Name} запущен. Покраска занимает {Painting} мин. Сушка занимает {Drying} мин.");
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
                    case DetailOperation.Painting: Leftover = DurOperation = Painting; Factory.checkBox12.Checked = true; break;
                    case DetailOperation.Drying: Leftover = DurOperation = Drying; Factory.checkBox13.Checked = true; break;
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
                    Factory.checkBox12.Checked = false;
                    Factory.checkBox13.Checked = false;

                    Detail.FinishOperation(this);
                    Factory.listBox1.Items.Add(Detail);
                    Detail = null;
                }
            }
        }

        public Workshop5(Factory factory, string name) : base(factory, name)
        {
            // покраска, сушка
            Operations = new List<DetailOperation>
            {
                DetailOperation.Painting,
                DetailOperation.Drying
            };
        }
    }
}