using System;
using System.Collections.Generic;

namespace Ilka
{
    /// <summary>
    /// 1 цех токарные, фрезеровочные, гибка
    /// </summary>
    internal class Workshop1 : Workshop
    {
        /// <summary>
        /// Сколько минут занимает токарная операция.
        /// </summary>
        private int Turning { get; set; }

        /// <summary>
        /// Сколько минут занимает фрезеровочная операция.
        /// </summary>
        private int Milling { get; set; }

        /// <summary>
        /// Сколько минут занимает гибка.
        /// </summary>
        private int Bending { get; set; }

        internal override void Start()
        {
            Turning = int.Parse(Factory.textBox1.Text);
            Milling = int.Parse(Factory.textBox2.Text);
            Bending = int.Parse(Factory.textBox3.Text);

            base.Start();
            Factory.Logger.WriteLine(
                $"{Name} запущен. Токарная операция {Turning} мин." +
                $"Фрезеровочная операция {Milling} мин. Гибка {Bending} мин.");
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
                    case DetailOperation.Lathe: Leftover = DurOperation = Turning; Factory.checkBox1.Checked = true; break;
                    case DetailOperation.Milling: Leftover = DurOperation = Milling; Factory.checkBox2.Checked = true; break;
                    case DetailOperation.Bending: Leftover = DurOperation = Bending; Factory.checkBox3.Checked = true; break;
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
                    Factory.checkBox1.Checked = false;
                    Factory.checkBox2.Checked = false;
                    Factory.checkBox3.Checked = false;

                    Detail.FinishOperation(this);
                    Factory.listBox1.Items.Add(Detail);
                    Detail = null;
                }
            }
        }

        public Workshop1(Factory factory, string name) : base(factory, name)
        {
            Operations = new List<DetailOperation>
            {
                DetailOperation.Lathe,
                DetailOperation.Milling,
                DetailOperation.Bending
            };
        }
    }
}
