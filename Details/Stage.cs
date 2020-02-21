using System.Collections.Generic;

namespace Ilka
{
    internal class Stage
    {
        internal int Number { get; set; }
        internal string OperationName { get; set; }
        internal DetailOperation Operation { get; set; }

        internal Stage(DetailOperation operation, int number)
        {
            Number = number;
            Operation = operation;
            OperationName = GetDetailOperationName(operation);
        }

        private static Dictionary<int, string> OperationNames = new Dictionary<int, string>
        {
            {1, "токарную"},
            {2, "фрезеровочную"},
            {3, "гибки"},
            {4, "сверлильную"},
            {5, "шлифовки"},
            {6, "промывки"},
            {7, "сушки"},
            {8, "сварочную"},
            {9, "полировки"},
            {10, "грунтовки"},
            {11, "покраски"}
        };

        internal static string GetDetailOperationName(DetailOperation operation)
        {
            return OperationNames[(int)operation];
        }

        public override string ToString()
        {
            var number = (Number != 0) ? Number.ToString() + " " : "";
            return $"{number}{OperationName}";
        }

    }
}
