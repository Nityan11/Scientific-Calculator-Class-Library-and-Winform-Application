using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Calculator.MathLibrary
{
    public class NegationOperation : UnaryOperation
    {
        protected override double Calculate(double[] values)
        {
            return -1 * values[0];
        }
    }
}
