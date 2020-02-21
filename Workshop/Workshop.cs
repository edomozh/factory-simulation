using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace Ilka
{
    /// <summary>
    /// Базовый класс цех.
    /// </summary>
    internal abstract class Workshop
    {
        /// <summary>
        /// Каждый тик. сколько сделано по текущей операции.
        /// </summary>
        internal event Action<int> Progress;

        internal void OnProgress(int leftover, int durOperation)
        {
            var percent = (int)((float)(durOperation -leftover) / durOperation * 100);
            Progress?.Invoke(percent);
        }

        /// <summary>
        /// В класс производство выводится информация о работе цехов.
        /// </summary>
        internal Factory Factory;

        /// <summary>
        /// Операции цеха.
        /// </summary>
        protected List<DetailOperation> Operations;

        /// <summary>
        /// Флаг обозначающий занятость цеха;
        /// </summary>
        // internal bool Work { get; set; }

        /// <summary>
        /// Текущая деталь в цеху.
        /// </summary>
        internal Detail Detail { get; set; }

        /// <summary>
        /// Длительность текущей операции.
        /// </summary>
        internal int DurOperation { get; set; }

        /// <summary>
        /// Остаток по операции.
        /// </summary>
        internal int Leftover { get; set; }

        /// <summary>
        /// Имя цеха.
        /// </summary>
        internal string Name { get; }

        /// <summary>
        /// Конструктор.
        /// </summary>
        /// <param name="factory">Производство.</param>
        /// <param name="name">Название цеха.</param>
        internal Workshop(Factory factory, string name)
        {
            Name = name;
            Factory = factory;
        }

        /// <summary>
        /// Конвертирует объекты из listbox'a в List Details.
        /// </summary>
        /// <param name="lb"></param>
        /// <returns>Лист деталей.</returns>
        internal static List<Detail> GetDetails(ListBox lb)
        {
            var list = new List<Detail>();
            foreach (var i in lb.Items)
                list.Add(i as Detail);
            return list;
        }

        /// <summary>
        /// Начинает работу цеха.
        /// </summary>
        internal virtual void Start()
        {
            WorkTime.WorkTimeTick += WorkMinute;
        }

        /// <summary>
        /// Заканчивает работу цеха.
        /// </summary>
        internal virtual void Stop()
        {
            WorkTime.WorkTimeTick -= WorkMinute;
            Factory.Logger.WriteLine($"{Name} остановлен.");
        }

        /// <summary>
        /// Взять деталь из листа в работу.
        /// </summary>
        protected void GetDetailFromFactory()
        {
            var list = GetDetails(Factory.listBox1);
            var details = list.FindAll(d => Operations.Contains(d.GetCurrentOperation()) && !d.Completed);
            if (details.Count == 0) return;
            var choice = details.Max(d => (int)d.GetCurrentOperation());

            Detail = details.FirstOrDefault(d => (int)d.GetCurrentOperation() == choice);
            if (Detail == null) return;
            Factory.listBox1.Items.Remove(Detail);
        }

        /// <summary>
        /// Обрабатывает рабочую минуту.
        /// </summary>
        /// <param name="sender">То откуда посылается метод.</param>
        /// <param name="e">Аргументы.</param>
        internal abstract void WorkMinute();

    }
}