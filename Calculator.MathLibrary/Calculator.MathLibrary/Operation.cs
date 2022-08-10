using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Calculator.MathLibrary
{
    public abstract class Operation
    {
        public abstract int CountOfOperand { get; }

        public double Evaluate(double[] values)
        {
            if (values.Length != CountOfOperand)
            {
                throw new InsufficientOperandsException();
            }
            return Calculate(values);
        }

        protected abstract double Calculate(double[] values);
    }
}
