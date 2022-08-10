using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Calculator.MathLibrary
{
    public class ReciprocalOperation : UnaryOperation
    {
        protected override double Calculate(double[] values)
        {
            if (values[0] == 0)
            {
                throw new DivideByZeroException(ExceptionMessages.DivideByZero);
            }
            else
            {
                return 1 / values[0];
            }

        }
    }
}
