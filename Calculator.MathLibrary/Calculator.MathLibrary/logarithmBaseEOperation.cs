using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Calculator.MathLibrary
{
    public class logarithmBaseEOperation:UnaryOperation
    {
        protected override double Calculate(double[] values)
        {
            if (values[0] == 0)
            {
                throw new ArgumentException(ExceptionMessages.LogOfZeroException);
            }
            return Math.Log(values[0]);
        }
    }
}
