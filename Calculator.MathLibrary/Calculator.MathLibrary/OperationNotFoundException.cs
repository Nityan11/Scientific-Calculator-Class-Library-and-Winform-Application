using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Calculator.MathLibrary
{
    public class OperationNotFoundException : Exception
    {
        public string _symbol;

        public override string Message
        {
            get
            {
                return _symbol + " " + ExceptionMessages.OperationNotFound;
            }
        }
        public OperationNotFoundException(string symbol)
        {
            this._symbol = symbol;
        }
    }
}
