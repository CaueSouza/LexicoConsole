using System;
using System.Collections.Generic;
using System.Text;

namespace LexicoConsole
{
    class Token
    {
        private String simbol;
        private String lexem;
        private bool isError;
        private int errorLine;

        public Token(string simbol, string lexem)
        {
            this.simbol = simbol;
            this.lexem = lexem;
            isError = false;
        }

        public Token(int errorLine)
        {
            isError = true;
            this.errorLine = errorLine;
        }

        public string getSimbol() { return simbol; }

        public string getLexem() { return lexem; }

        public bool getIsError() { return isError; }

        public int getErrorLine() { return errorLine; }
    }
}
