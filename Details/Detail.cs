using System;
using System.Collections.Generic;

namespace Ilka
{
    /// <summary>
    /// Базовый класс детали.
    /// </summary>
    internal class Detail
    {
        internal int Id;
        internal string Name;
        internal string Value;

        internal bool Completed { get; set; }
        internal List<string> Log;
        internal DetailType Type;

        private int Marker;
        private Dictionary<int, DetailOperation> Stages;

        internal Detail(int type)
        {
            if (type == 1) InitDetailOne();
            else if (type == 2) InitDetailTwo();
            else throw new ArgumentException("Нет такого типа детали.");

            Id = IdGenerator.GetId();
            Log = new List<string>();
            Marker = 1;
        }

        internal Detail(Dictionary<int, DetailOperation> stages, string name)
        {
            Type = DetailType.Custom;
            Stages = stages;
            Name = name;

            Id = IdGenerator.GetId();
            Log = new List<string>();
            Marker = 1;
        }

        private void InitDetailOne()
        {
            Name = "Деталь 1";
            Type = DetailType.One;
            Stages = new Dictionary<int, DetailOperation>
            {
                { 1, DetailOperation.Lathe},
                { 2, DetailOperation.Milling},
                { 3, DetailOperation.Bending},
                { 4, DetailOperation.Drilling},
                { 5, DetailOperation.Grinding},
                { 6, DetailOperation.Washing},
                { 7, DetailOperation.Drying}
            };
        }

        private void InitDetailTwo()
        {
            Name = "Деталь 2";
            Type = DetailType.Two;
            Stages = new Dictionary<int, DetailOperation>
            {
                { 1, DetailOperation.Lathe},
                { 2, DetailOperation.Welding},
				//{ 3, DetailOperation.Polishing},
				{ 3, DetailOperation.Washing},
                { 4, DetailOperation.Drying},
                { 5, DetailOperation.Priming},
                { 6, DetailOperation.Painting},
                { 7, DetailOperation.Drying}
            };
        }

        internal DetailOperation GetCurrentOperation()
        {
            if (Stages.Count < Marker) return 0;
            return Stages[Marker];
        }

        internal void StartOperation(Workshop workshop, DetailOperation operation)
        {
            if (Completed)
                throw new Exception("Произошла попытка начать операцию с обработанной деталью.");
            if (GetCurrentOperation() != operation)
                throw new Exception("Произошла попытка начать некоректную операцию.");

            var log = $"{workshop.Name} начал операцию {Stage.GetDetailOperationName(GetCurrentOperation())} с деталью {Id}.";
            Value += $"->{workshop.Name}";
            Log.Add(log);
            workshop.Factory.Logger.WriteLine(log);
        }

        internal void FinishOperation(Workshop workshop)
        {

            string log;
            if (Marker == Stages.Count)
            {
                Completed = true;
                Value += "->End";
                log = $"{workshop.Name} закончил последнюю операцию {Stage.GetDetailOperationName(GetCurrentOperation())} с деталью {Id}.";
            }
            else
            {
                log = $"{workshop.Name} закончил операцию {Stage.GetDetailOperationName(GetCurrentOperation())} с деталью {Id}.";
                Marker++;
            }

            Log.Add(log);
            workshop.Factory.Logger.WriteLine(log);
        }


        public override string ToString()
        {
            return $"{Id}: {Name}: {Value}";
        }
    }
}