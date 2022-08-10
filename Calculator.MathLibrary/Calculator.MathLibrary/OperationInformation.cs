namespace Calculator.MathLibrary
{
    public class OperationInformation
    {
        public Operation OperatorObject;
        public int Precedence;

        public OperationInformation(Operation operatorObject, int precedence){
            this.OperatorObject = operatorObject;
            this.Precedence = precedence;
        }
            
    }
}
