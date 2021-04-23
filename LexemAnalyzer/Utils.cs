using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace LexemAnalyzer
{
    public class Utils
    {
        public static char[] letters = new char[]
        {
            'a','b','c','d','e','f','g','h','i','j','k','l','m','n','o','p','q','r','s','t','u','v','w','x','y','z',
            'A','B','C','D','E','F','G','H','I','J','K','L','M','N','O','P','Q','R','S','T','U','V','W','X','Y','Z', '_'
        };
        public static int[] digits = new int[]
        {
            0,1,2,3,4,5,6,7,8,9
        };
        public static char[] relop = new char[]
        {
            '<', '>', '=', '!' , '-','+','*','/', '&', '|'
        };
        public static char[] delim = new char[]
        {
            ' ', '\t', '\n' 
        };
        public static string[] keywords = new string[]
        {  
            "auto", "break", "case", "char", "const", "continue", "default", "do", "double", "else", "enum", "extern", 
            "float", "for", "goto", "if", "int", "long", "register", "return", "short", "signed", "sizeof", "static", 
            "struct", "switch", "typedef", "union", "unsigned", "void", "volatile", "while" 
        };
        public static char[] punctuation = new char[]
        {
            '}', '{',')','(','[',']',';',',','.'
        };
        public enum Token
        {
            NULL, INT, REAL, SCI, STR, IDENTIFIER, PUNCTUATION, ASSIGN, ADD_ASSIGN, SUB_ASSIGN, MUL_ASSIGN, DIV_ASSIGN, INC,
            DEC, ADD, SUB, MUL, DIV, EQ, NE, LT, GT, LE, GE, NOT, AND, OR, KEYWORD, ungetch, Error, True, False
        }

        public static Dictionary<string, Token> tokensTable = keywords.ToDictionary(k => k, v => Token.KEYWORD);

        public static List<Tuple<string, Token>> outputTable = new List<Tuple<string, Token>>();

        public static Dictionary<string, Token> symbolTable = new Dictionary<string, Token>();

        // initial value of buffer (it is considered, the alphabet doesnt
        // contains '^' character
        public static char buffer = '^';

        //actually holder holds the last character in buffer to 
        //prevent the array counter going too fast :)
        public static void holder(char inp)
        {
            buffer = inp;
        }

        //ungetch shows that if we've stored a character in buffer or not
        public static void ungetch(char inp)
        {
            buffer = inp;
            if(inp != ' ') outputTable.Add(new Tuple<string, Token>("ungetch", Token.ungetch));
        }
        public static char getBufferValue()
        {
            return buffer;
        }
    }
}
