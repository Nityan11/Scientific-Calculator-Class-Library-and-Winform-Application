using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Calculator.MathLibrary
{
    public abstract class UnaryOperation : Operation
    {
        public override int CountOfOperand
        {
            get
            {
                return 1;
            }
        }
    }
}
