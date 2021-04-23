using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


/*
 * 
 * Implemented by Hojjat Mokhtarabadi
 * 
 */
namespace LexemAnalyzer
{
    //states base class which by its call method we can easily transition between
    //different states
    interface IBase
    {
        public IBase call(char input);
    }

    //intial state of transition diagram
    class InitialState : IBase
    {
        public IBase call(char input)
        {
            bool isLetter = Array.Exists(Utils.letters, element => element == input);
            bool isDigit = Array.Exists(Utils.digits, element => element == (int)Char.GetNumericValue(input));
            bool isRelop = Array.Exists(Utils.relop, element => element == input);
            bool isPunctuation = Array.Exists(Utils.punctuation, element => element == input);
            bool isDelim = Array.Exists(Utils.delim, element => element == input);
            bool isStr = input == '"';
            bool isComment = input == '/';
            bool isFinished = input == '$';

            if (isLetter)
            {
                return new S1(input.ToString());
            }
            else if (isDigit)
            {
                return new S2(input.ToString());
            }
            else if (isRelop)
            {
                return new RelopState(input);
            }
            else if (isPunctuation)
            {
                return new GoalState(input, Utils.Token.PUNCTUATION);
            }
            else if (isDelim)
            {
                return new InitialState();
            }
            else if (isStr)
            {
                return new StrState(input.ToString());
            }
            else if (isComment)
            {
                return new CommentState(input.ToString());
            }
            else if (isFinished)
            {
                return End.happend;
            }
            return new Error();
        }
    }

    //------------------------------String---------------------------------
    class StrState : IBase
    {
        string _input;
        public StrState(string input)
        {
            _input = input;
        }
        public IBase call(char input)
        {
            string lexem = $"{_input}{input}";
            if (input == '"')
            {
                return new GoalState(lexem, Utils.Token.STR);
            }
            return new StrState(lexem);
        }
    }

    //------------------------------Comment--------------------------------
    class CommentState : IBase
    {
        string _input;
        public CommentState(string input)
        {
            _input = input;
        }
        public IBase call(char input)
        {
            string lexem = $"{_input}{input}";
            if (input == '/')
            {
                return new OneComment(lexem);
            }
            if (input == '*')
            {
                return new MultiComment(lexem);
            }
            return new InitialState();
        }
    }
    class OneComment : IBase
    {
        string _input;
        public OneComment(string input)
        {
            _input = input;
        }
        public IBase call(char input)
        {
            string lexem = $"{_input}{input}";
            if (input == '\n')
            {
                return new InitialState();
            }
            return new OneComment(lexem);
        }
    }
    class MultiComment : IBase
    {
        string _input;
        public MultiComment(string input)
        {
            _input = input;
        }
        public IBase call(char input)
        {
            string lexem = $"{_input}{input}";
            if (input == '*')
            {
                return new MultiCommentEnding(lexem);
            }
            return new MultiComment(lexem);
        }
    }
    class MultiCommentEnding : IBase
    {
        string _input;
        public MultiCommentEnding(string input)
        {
            _input = input;
        }
        public IBase call(char input)
        {
            string lexem = $"{_input}{input}";
            if (input == '/')
            {
                return new InitialState();
            }
            return new MultiComment(lexem);
        }
    }
    //------------------------------Relop------------------------------------
    class RelopState : IBase
    {
        char _input;
        public RelopState(char input)
        {
            _input = input;
        }
        public IBase call(char input)
        {
            switch (_input)
            {
                case '<':
                    {
                        if (input == '=')
                        {
                            string lexem = $"{_input}{input}";
                            return new GoalState(lexem, Utils.Token.LE);
                        }
                        Utils.holder(input);
                        return new GoalState(_input, Utils.Token.LT);
                    }
                case '>':
                    {
                        if (input == '=')
                        {
                            string lexem = $"{_input}{input}";
                            return new GoalState(lexem, Utils.Token.GE);
                        }
                        Utils.holder(input);
                        return new GoalState(_input, Utils.Token.GT);
                    }
                case '=':
                    {
                        if (input == '=')
                        {
                            string lexem = $"{_input}{input}";
                            return new GoalState(lexem, Utils.Token.EQ);
                        }
                        Utils.holder(input);
                        return new GoalState(_input, Utils.Token.ASSIGN);
                    }
                case '!':
                    {
                        if (input == '=')
                        {
                            string lexem = $"{_input}{input}";
                            return new GoalState(lexem, Utils.Token.NE);
                        }
                        Utils.holder(input);
                        return new GoalState(_input, Utils.Token.NOT);
                    }
                case '+':
                    {
                        if (input == '+')
                        {
                            string lexem = $"{_input}{input}";
                            return new GoalState(lexem, Utils.Token.INC);
                        }
                        else if (input == '=')
                        {
                            string lexem = $"{_input}{input}";
                            return new GoalState(lexem, Utils.Token.ADD_ASSIGN);
                        }
                        Utils.holder(input);
                        return new GoalState(_input, Utils.Token.ADD);
                    }
                case '-':
                    {
                        if (input == '-')
                        {
                            string lexem = $"{_input}{input}";
                            return new GoalState(lexem, Utils.Token.INC);
                        }
                        else if (input == '=')
                        {
                            string lexem = $"{_input}{input}";
                            return new GoalState(lexem, Utils.Token.SUB_ASSIGN);
                        }
                        Utils.holder(input);
                        return new GoalState(_input, Utils.Token.SUB);
                    }
                case '*':
                    {
                        if (input == '=')
                        {
                            string lexem = $"{_input}{input}";
                            return new GoalState(lexem, Utils.Token.MUL_ASSIGN);
                        }
                        Utils.holder(input);
                        return new GoalState(_input, Utils.Token.MUL);
                    }
                case '/':
                    {
                        string lexem = $"{_input}{input}";
                        if (input == '=')
                        {
                            return new GoalState(lexem, Utils.Token.DIV_ASSIGN);
                        }
                        else if (input == '/')
                        {
                            return new OneComment(lexem);
                        }
                        else if (input == '*')
                        {
                            return new MultiComment(lexem);
                        }
                        Utils.holder(input);
                        return new GoalState(_input, Utils.Token.DIV);
                    }
                case '&':
                    {
                        if (input == '&')
                        {
                            string lexem = $"{_input}{input}";
                            return new GoalState(lexem, Utils.Token.AND);
                        }
                        Utils.holder(input);
                        return new Error();
                    }
                case '|':
                    {
                        if (input == '|')
                        {
                            string lexem = $"{_input}{input}";
                            return new GoalState(lexem, Utils.Token.OR);
                        }
                        Utils.holder(input);
                        return new Error();
                    }
                default: return new Error();
            }
        }
    }
    //----------------------------Identifier---------------------------------
    // 
    class S1 : IBase
    {
        string _input;
        public S1(string input)
        {
            _input = input;
        }
        public IBase call(char input)
        {
            bool isLetter = Array.Exists(Utils.letters, element => element == input);
            bool isDigit = Array.Exists(Utils.digits, element => element == (int)Char.GetNumericValue(input));
            string lexem = $"{_input}{input}";

            if (isLetter || isDigit)
            {
                return new S1(lexem);
            }
            else if (!isLetter && !isDigit && Utils.tokensTable.ContainsKey(_input))
            {
                Utils.holder(input);
                return new GoalState(_input, Utils.Token.KEYWORD);
            }
            else
            {
                Utils.holder(input);
                return new GoalState(_input, Utils.Token.IDENTIFIER);
            }
        }
    }
    //------------------------------Digit------------------------------------
    //! All S2 to S7 states belong to digit analyzer
    class S2 : IBase
    {
        string _input;
        public S2(string input)
        {
            _input = input;
        }
        public IBase call(char input)
        {
            bool isDigit = Array.Exists(Utils.digits, element => element == (int)Char.GetNumericValue(input));
            string lexem = $"{_input}{input}";
            if (isDigit)
            {
                return new S2(lexem);
            }
            else if (input == '.')
            {
                return new S3(lexem);
            }
            else if (input == 'E')
            {
                return new S4(lexem);
            }
            Utils.holder(input);
            return new GoalState(_input, Utils.Token.INT);
        }
    }
    class S3 : IBase
    {
        string _input;
        public S3(string input)
        {
            _input = input;
        }
        public IBase call(char input)
        {
            bool isDigit = Array.Exists(Utils.digits, element => element == (int)Char.GetNumericValue(input));
            string lexem = $"{_input}{input}";
            if (isDigit)
            {
                return new S5(lexem);
            }
            return new Error();
        }
    }
    class S4 : IBase
    {
        string _input;
        public S4(string input)
        {
            _input = input;
        }
        public IBase call(char input)
        {
            bool isDigit = Array.Exists(Utils.digits, element => element == (int)Char.GetNumericValue(input));
            string lexem = $"{_input}{input}";
            if (isDigit)
            {
                return new S6(lexem);
            }
            else if (input == '-' || input == '+')
            {
                return new S7(lexem);
            }
            return new Error();
        }
    }
    class S5 : IBase
    {
        string _input;
        public S5(string input)
        {
            _input = input;
        }
        public IBase call(char input)
        {
            bool isDigit = Array.Exists(Utils.digits, element => element == (int)Char.GetNumericValue(input));
            if (isDigit)
            {
                string lexem = $"{_input}{input}";
                return new S5(lexem);
            }
            else if (input == 'E')
            {
                string lexem = $"{_input}{input}";
                return new S4(lexem);
            }
            Utils.holder(input);
            return new GoalState(_input, Utils.Token.REAL);
        }
    }
    class S6 : IBase
    {
        string _input;
        public S6(string input)
        {
            _input = input;
        }

        public IBase call(char input)
        {
            bool isDigit = Array.Exists(Utils.digits, element => element == (int)Char.GetNumericValue(input));
            if (isDigit)
            {
                string lexem = $"{_input}{input}";
                return new S6(lexem);
            }
            Utils.holder(input);
            return new GoalState(_input, Utils.Token.SCI);
        }
    }
    class S7 : IBase
    {
        string _input;
        public S7(string input)
        {
            _input = input;
        }
        public IBase call(char input)
        {
            bool isDigit = Array.Exists(Utils.digits, element => element == (int)Char.GetNumericValue(input));
            if (isDigit)
            {
                string lexem = $"{_input}{input}";
                return new S6(lexem);
            }
            return new Error();
        }
    }
    // ---------------------------Goal State---------------------------------

    class GoalState : IBase
    {
        dynamic _lexem;
        Utils.Token _token;
        public GoalState(dynamic lexem, Utils.Token token)
        {
            _token = token;
            _lexem = lexem;
        }
        public IBase call(char input)
        {
            if ((_token == Utils.Token.IDENTIFIER) && (!Utils.symbolTable.ContainsKey(_lexem)))
            {
                Utils.symbolTable.Add(_lexem.ToString(), _token);
                Utils.outputTable.Add(new Tuple<string, Utils.Token>(_lexem.ToString(), _token));
                Utils.outputTable.Add(new Tuple<string, Utils.Token>("true", Utils.Token.True));
            }
            else if ((_token == Utils.Token.IDENTIFIER) && (Utils.symbolTable.ContainsKey(_lexem)))
            {
                Utils.outputTable.Add(new Tuple<string, Utils.Token>(_lexem.ToString(), _token));
                Utils.outputTable.Add(new Tuple<string, Utils.Token>("false", Utils.Token.True));
            }
            else
            {
                Utils.outputTable.Add(new Tuple<string, Utils.Token>(_lexem.ToString(), _token));
            }
            Utils.ungetch(input);
            return new InitialState();
        }
    }

    //------------------------------Error----------------------------------
    class Error : IBase
    {
        public IBase call(char input)
        {
            Utils.outputTable.Add(new Tuple<string, Utils.Token>("Error", Utils.Token.Error));
            Utils.holder(input);
            return new InitialState();
        }
    }

    //--------------------------Lexical Analyzer------------------------------
    public class Analyzer
    {
        string input;
        InitialState initialState = new InitialState();

        public Analyzer(string input)
        {
            this.input = input;
        }

        public void execute()
        {
            char[] chArr = input.ToCharArray();
            char nextChar = chArr[0];
            IBase next_state = initialState.call(nextChar);
            int i = 1;
            while (next_state != End.happend)
            {
                if (Utils.getBufferValue() != '^')
                {
                    nextChar = Utils.getBufferValue();
                    Utils.buffer = '^';
                }
                else
                {
                    nextChar = chArr[i];
                    i++;
                }
                next_state = next_state.call(nextChar);
            }
        }
    }
}
