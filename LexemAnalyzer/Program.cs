using System;
using System.Collections;
using System.IO;

namespace LexemAnalyzer
{
    class Program
    {
        static void Main(string[] args)
        {
            /*
             * IMPORTANT: the given string must be ended with '$'
             */

            string textFile = @"input.txt";
            string text = "$";
            string text2 = "int _num23= 125E9; \n if(_num23 == 12) {\n //comment \n } else reze= -8 \"this is string\".$";
            if (File.Exists(textFile))
            {
                // Read entire text file content in one string    
                text = File.ReadAllText(textFile);
            }
            //Console.WriteLine(text);
            Analyzer a = new Analyzer(text2);
            a.execute();
            foreach (var (i, j) in Utils.outputTable)
            {
                if((j!= Utils.Token.ungetch) && (j!= Utils.Token.True) && (j!= Utils.Token.False))
                    Console.WriteLine($"{i} {j}");
                else
                    Console.WriteLine($"{i}");
            }
        }
    }
}