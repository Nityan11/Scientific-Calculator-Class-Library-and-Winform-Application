using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Calculator.MathLibrary
{
    public class OperatorSymbolAlreadyExistsException : Exception
    {
        private string _symbol;

        public override string Message
        {
            get
            {
                return _symbol + " " + ExceptionMessages.OperatorSymbolAlreadyExists;
            }
        }
        public OperatorSymbolAlreadyExistsException(string symbol)
        {
            this._symbol = symbol;
        }
    }
}
