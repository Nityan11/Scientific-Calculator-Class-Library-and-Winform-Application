using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Calculator.MathLibrary
{
    public class DivideOperation : BinaryOperation
    {
        protected override double Calculate(double[] values)
        {
            if (values[1] == 0)
            {
                throw new DivideByZeroException(ExceptionMessages.DivideByZero);
            }
            else
            {
                return values[0] / values[1];
            }
        }
    }
}
