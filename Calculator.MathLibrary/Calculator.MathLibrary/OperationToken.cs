using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Calculator.MathLibrary
{
    public enum SymbolType
    {
        Value,
        OperatorSymbol
    }
    public class OperationToken
    {
        public string OperationSymbol { get; set; }
        public SymbolType Type { get; set; }

        public OperationToken(string operationSymbol, SymbolType type)
        {
            this.OperationSymbol = operationSymbol;
            this.Type = type;
        }
    }
}
