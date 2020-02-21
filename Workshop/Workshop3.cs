using System;
using System.Collections.Generic;

namespace Ilka
{
    /// <summary>
    /// 3 цех шлифовка, промывка
    /// </summary>
    internal class Workshop3 : Workshop
    {
        /// <summary>
        /// Сколько минут занимает шлифовка.
        /// </summary>
        private int Grinding { get; set; }

        /// <summary>
        /// Сколько минут занимает промывка.
        /// </summary>
        private int Washing { get; set; }

        internal override void Start()
        {
            Grinding = int.Parse(Factory.textBox7.Text);
            Washing = int.Parse(Factory.textBox8.Text);

            WorkTime.WorkTimeTick += WorkMinute;
            Factory.Logger.WriteLine(
                $"{Name} запущен. Шлифовка занимает {Grinding} мин. Промывка занимает {Washing} мин.");
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
                    case DetailOperation.Grinding: Leftover = DurOperation = Grinding; Factory.checkBox7.Checked = true; break;
                    case DetailOperation.Washing: Leftover = DurOperation = Washing; Factory.checkBox8.Checked = true; break;
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
                    Factory.checkBox7.Checked = false;
                    Factory.checkBox8.Checked = false;

                    Detail.FinishOperation(this);
                    Factory.listBox1.Items.Add(Detail);
                    Detail = null;
                }
            }
        }

        public Workshop3(Factory factory, string name) : base(factory, name)
        {
            // шлифовка, промывка
            Operations = new List<DetailOperation>
            {
                DetailOperation.Grinding,
                DetailOperation.Washing
            };
        }
    }
}