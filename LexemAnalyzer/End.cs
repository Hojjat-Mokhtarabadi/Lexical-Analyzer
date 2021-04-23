using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LexemAnalyzer
{
    static class End
    {
        public static EndState happend = new EndState();
    }

    class EndState : IBase
    {
        public IBase call(char input)
        {
            return End.happend;
        }
    }

}
