namespace Calculator.MathLibrary
{
    public abstract class BinaryOperation : Operation
    {
        public override int CountOfOperand
        {
            get
            {
                return 2;
            }
            
        }
    }
}