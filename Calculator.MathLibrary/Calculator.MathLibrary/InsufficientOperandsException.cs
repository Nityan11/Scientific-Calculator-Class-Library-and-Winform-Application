using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Calculator.MathLibrary
{
    public class InsufficientOperandsException : Exception
    {
        public override string Message
        {
            get
            {
                return ExceptionMessages.InsufficientOperands;
            }
        }
    }
}
