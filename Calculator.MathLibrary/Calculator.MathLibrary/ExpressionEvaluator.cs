using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Calculator.MathLibrary
{
    public class ExpressionEvaluator
    {
        public Dictionary<string, OperationInformation> _symbolTable;
        private Dictionary<string, SymbolInformation> _operatorConfiguration;

        public ExpressionEvaluator()
        {
            SumOperation add = new SumOperation();
            SubtractOperation subtract = new SubtractOperation();
            ProductOperation multiply = new ProductOperation();
            DivideOperation divide = new DivideOperation();
            ReciprocalOperation reciprocal = new ReciprocalOperation();
            SquareRootOperation squareRoot = new SquareRootOperation();
            LogarithmBaseTenOperation logBaseten = new LogarithmBaseTenOperation();
            PercentOperation percent = new PercentOperation();
            logarithmBaseEOperation logBaseE = new logarithmBaseEOperation();
            SineOperation sin = new SineOperation();
            CosOperation cos = new CosOperation();
            TanOperation tan = new TanOperation();
            PowerOperation power = new PowerOperation();
            NegationOperation negation = new NegationOperation();

            //Parse the contents of Json file to dictionary _operatorConfiguration 
            _operatorConfiguration = JsonConvert.DeserializeObject<Dictionary<string, SymbolInformation>>(File.ReadAllText(Symbols.FILENAME));
            
            _symbolTable = new Dictionary<string, OperationInformation>();


            //Adding default operations from _operationConfiguration dictionary to the_symbolTable dictionary.
            //The if-condition check ensures flexibility, i.e, the user has the option to remove any operation from json file without affecting the other operations, else we could have thrown let it throw the exception.
            if (_operatorConfiguration.ContainsKey("ADD"))
            {
                _symbolTable.Add(_operatorConfiguration["ADD"].Symbol, new OperationInformation(add, _operatorConfiguration["ADD"].Precedence));
            }
            if (_operatorConfiguration.ContainsKey("SUBTRACT"))
            {
                _symbolTable.Add(_operatorConfiguration["SUBTRACT"].Symbol, new OperationInformation(subtract, _operatorConfiguration["SUBTRACT"].Precedence));
            }
            if (_operatorConfiguration.ContainsKey("DIVIDE"))
            {
                _symbolTable.Add(_operatorConfiguration["DIVIDE"].Symbol, new OperationInformation(divide, _operatorConfiguration["DIVIDE"].Precedence));
            }
            if (_operatorConfiguration.ContainsKey("MULTIPLY"))
            {
                _symbolTable.Add(_operatorConfiguration["MULTIPLY"].Symbol, new OperationInformation(multiply, _operatorConfiguration["MULTIPLY"].Precedence));
            }
            if (_operatorConfiguration.ContainsKey("RECIPROCAL"))
            {
                _symbolTable.Add(_operatorConfiguration["RECIPROCAL"].Symbol, new OperationInformation(reciprocal, _operatorConfiguration["RECIPROCAL"].Precedence));
            }
            if (_operatorConfiguration.ContainsKey("SQUAREROOT"))
            {
                _symbolTable.Add(_operatorConfiguration["SQUAREROOT"].Symbol, new OperationInformation(squareRoot, _operatorConfiguration["SQUAREROOT"].Precedence));
            }
            if (_operatorConfiguration.ContainsKey("LOG_E"))
            {
                _symbolTable.Add(_operatorConfiguration["LOG_E"].Symbol, new OperationInformation(logBaseE, _operatorConfiguration["LOG_E"].Precedence));
            }
            if (_operatorConfiguration.ContainsKey("LOG_10"))
            {
                _symbolTable.Add(_operatorConfiguration["LOG_10"].Symbol, new OperationInformation(logBaseten, _operatorConfiguration["LOG_10"].Precedence));
            }
            if (_operatorConfiguration.ContainsKey("NEGATE"))
            {
                _symbolTable.Add(_operatorConfiguration["NEGATE"].Symbol, new OperationInformation(negation, _operatorConfiguration["NEGATE"].Precedence));
            }
            if (_operatorConfiguration.ContainsKey("POWER"))
            {
                _symbolTable.Add(_operatorConfiguration["POWER"].Symbol, new OperationInformation(power, _operatorConfiguration["POWER"].Precedence));
            }
            if (_operatorConfiguration.ContainsKey("TAN"))
            {
                _symbolTable.Add(_operatorConfiguration["TAN"].Symbol, new OperationInformation(tan, _operatorConfiguration["TAN"].Precedence));
            }
            if (_operatorConfiguration.ContainsKey("COSINE"))
            {
                _symbolTable.Add(_operatorConfiguration["COSINE"].Symbol, new OperationInformation(cos, _operatorConfiguration["COSINE"].Precedence));
            }
            if (_operatorConfiguration.ContainsKey("SINE"))
            {
                _symbolTable.Add(_operatorConfiguration["SINE"].Symbol, new OperationInformation(sin, _operatorConfiguration["SINE"].Precedence));
            }
            if (_operatorConfiguration.ContainsKey("PERCENT"))
            {
                _symbolTable.Add(_operatorConfiguration["PERCENT"].Symbol, new OperationInformation(percent, _operatorConfiguration["PERCENT"].Precedence));
            }
        }


        //This method breaks the input string into tokens (operators and operands).
        private List<OperationToken> ExtractTokens(string inp)
        {
            List<OperationToken> tokens = new List<OperationToken>();
            double num = 0;
            bool afterDecimal = false;
            double countAfterDecimal = 0;
            string operatorToken = String.Empty;

            //Traverse over the complete string: 1. if the character is a number add it to double num
            //2. if it is open bracket then add it to tokens
            //3. if it is decimal point then set afterdecimal = true and keep incrementing the count of countAfterDecimal till you recieve a non digit character
            //4. otherwise, a. if it is a single character operator, then add num formed till now to tokens, add the current character, set afterdecimal = false & countAfterDecimal = 0
            //              b. if it is character from english alphabet then, keep adding it to string operatorToken. When a digit is encountered, add this string to tokens.
            for (int i = 0; i < inp.Length; i++)
            {
                if (inp[i] == ' ')
                {
                    continue;
                }
                if (inp[i] >= '0' && inp[i] <= '9' && !afterDecimal)
                {
                    if (operatorToken != "")
                    {
                        if (!_symbolTable.ContainsKey(operatorToken))
                        {
                            throw new OperationNotFoundException(operatorToken);
                        }
                        tokens.Add(new OperationToken(operatorToken, SymbolType.OperatorSymbol));
                        operatorToken = String.Empty;
                    }
                    //In line below, the part before decimal point is added to num, to form the operand
                    num = num * 10 + (inp[i] - '0');
                }
                else if (inp[i] >= '0' && inp[i] <= '9')
                {
                    countAfterDecimal++;
                    //In line below, the part after the decimal point is added to num, to form the operand
                    num += (inp[i] - '0') / (Math.Pow(10, countAfterDecimal));
                }
                else if (inp[i].ToString() == Symbols.DECIMAL_POINT_symbol)
                {
                    afterDecimal = true;
                }
                else if (inp[i].ToString() == Symbols.OPEN_BRACKET_symbol)
                {
                    if (operatorToken != "")
                    {
                        if (!_symbolTable.ContainsKey(operatorToken))
                        {
                            throw new OperationNotFoundException(operatorToken);
                        }
                        tokens.Add(new OperationToken(operatorToken, SymbolType.OperatorSymbol));
                        operatorToken = String.Empty;
                    }
                    tokens.Add(new OperationToken(inp[i].ToString(), SymbolType.OperatorSymbol));
                }
                else
                {
                    if (inp[i] >= 'a' && inp[i] <= 'z' || inp[i] >= 'A' && inp[i] <= 'Z')
                    {
                        operatorToken += inp[i];
                    }
                    else
                    {
                        if (tokens.Count() == 0 || tokens[tokens.Count - 1].OperationSymbol != Symbols.CLOSE_BRACKET_symbol && operatorToken == String.Empty)
                        {
                            tokens.Add(new OperationToken(num.ToString(), SymbolType.Value));
                        }
                        if (inp[i].ToString() != Symbols.CLOSE_BRACKET_symbol && !_symbolTable.ContainsKey(inp[i].ToString()))
                        {
                            throw new OperationNotFoundException(inp[i].ToString());
                        }
                        tokens.Add(new OperationToken(inp[i].ToString(), SymbolType.OperatorSymbol));
                    }

                    num = 0;
                    afterDecimal = false;
                    countAfterDecimal = 0;
                }

            }
            if (tokens.Count() > 0 && tokens[tokens.Count() - 1].OperationSymbol != Symbols.CLOSE_BRACKET_symbol)
                tokens.Add(new OperationToken(num.ToString(), SymbolType.Value));

            return tokens;
        }
 
        //This method converts the tokens List in infix to postfix List and it removes the brackets present in tokens.
        private List<OperationToken> InfixToPostfix(List<OperationToken> tokens)
        {
            List<OperationToken> postfix = new List<OperationToken>();
            Stack<OperationToken> helper = new Stack<OperationToken>();

            //Traverse over the tokens List: 1. if token type is of a digit/value then add it to the postfix List
            //2.else: a. if stack is empty or precedence(token)>precedence(stack.top) or token is an open bracket then push the current token to stack
            //        b. else if token is close bracket then pop the tokens from stack and add them to postfix list till open bracket is found.
            //        c. else pop and add the tokens to postfix list till the precedence(token)<=precedence(stack.top).
            //After traversing all the tokens, add the left tokens in stack to the postfix list
            for (int i = 0; i < tokens.Count; i++)
            {
                if (tokens[i].Type == SymbolType.Value)
                {
                    postfix.Add(tokens[i]);
                }
                else
                {
                    if (helper.Count == 0 || GetOperatorPrecedence(tokens[i].OperationSymbol) > GetOperatorPrecedence(helper.Peek().OperationSymbol) || tokens[i].OperationSymbol == Symbols.OPEN_BRACKET_symbol)
                    {
                        helper.Push(tokens[i]);
                    }
                    else if (tokens[i].OperationSymbol == Symbols.CLOSE_BRACKET_symbol)
                    {
                        while (helper.Peek().OperationSymbol != Symbols.OPEN_BRACKET_symbol)
                        {
                            postfix.Add(helper.Peek());
                            helper.Pop();
                        }
                        helper.Pop();
                    }
                    else
                    {
                        while (helper.Count != 0 && GetOperatorPrecedence(tokens[i].OperationSymbol) <= GetOperatorPrecedence(helper.Peek().OperationSymbol))
                        {
                            postfix.Add(helper.Peek());
                            helper.Pop();
                        }
                        helper.Push(tokens[i]);

                    }
                }
            }

            while (helper.Count != 0)
            {
                postfix.Add(helper.Peek());
                helper.Pop();
            }

            return postfix;
        }

        //This method gets precedence of different operators from _symbolTable
        private int GetOperatorPrecedence(string symbol)
        {
            if (symbol == Symbols.OPEN_BRACKET_symbol || symbol == Symbols.CLOSE_BRACKET_symbol)
            {
                return -1;
            }
            else
            {
                return _symbolTable[symbol].Precedence;
            }
        }
        
        //This method evaluates the postfix expression to return the final answer
        private double EvaluatePostfixExpression(List<OperationToken> postfix)
        {
            Stack<double> operandStack = new Stack<double>();

            //Traverse the postfix list: 1. if the postfix[i] is of type value, then push it to the stack
            //2. Else, get the mapped Operation object for the given operationSymbol, and pop as many elements as the operandCount from the stack and pass it to the objects's Evaluate function 
            //If the stack becomes empty during 2. then throw the InsufficientOperandsException.
            //Push the result of obtained from Evaluate function to the stack.
            //After the traversal of postfix list, the final anwer will be present in the stack, return it.
            for (int i = 0; i < postfix.Count; i++)
            {
                if (postfix[i].Type == SymbolType.Value)
                {
                    operandStack.Push(Convert.ToDouble(postfix[i].OperationSymbol));
                }
                else
                {
                    Operation currentOperation = _symbolTable[postfix[i].OperationSymbol].OperatorObject;
                    int operandCount = currentOperation.CountOfOperand;
                    double[] values = new double[operandCount];

                    for (int j = values.Length - 1; j >= 0; j--)
                    {   
                        if(operandStack.Count() == 0)
                        {
                            throw new InsufficientOperandsException();
                        }
                        values[j] = operandStack.Peek();
                        operandStack.Pop();

                    }
                    operandStack.Push(currentOperation.Evaluate(values));

                }

            }
            if (operandStack.Count() == 0)
            {
                throw new InsufficientOperandsException();
            }
            return operandStack.Peek();
            
        }

        //This public method gets input string as argument and then it calls the above private methods to return the double answer.
        public double Evaluator(string inp)
        {
            List<OperationToken> tokens = ExtractTokens(inp);
            List<OperationToken> postfix = InfixToPostfix(tokens);
            return EvaluatePostfixExpression(postfix);

        }

        //This public method takes Operation object and string operationKey as arguments to add new operations to the _symbolTable dictionary.
        public void RegisterCustomOperation(Operation operation, string operationKey)
        {
            string symbol = _operatorConfiguration[operationKey].Symbol;
            if (_symbolTable.ContainsKey(symbol))
            {
                throw new OperatorSymbolAlreadyExistsException(symbol);
            }
            else
            {
                _symbolTable.Add(symbol, new OperationInformation(operation, _operatorConfiguration[operationKey].Precedence));
            }
        }

    }
}
