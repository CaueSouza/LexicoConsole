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

        public Token(string simbol, string lexem)
        {
            this.simbol = simbol;
            this.lexem = lexem;
            this.isError = false;
        }

        public Token(string simbol, string lexem, bool isError)
        {
            this.simbol = simbol;
            this.lexem = lexem;
            this.isError = true;
        }

        public string getSimbol() { return simbol; }

        public string getLexem() { return lexem; }

        public bool getIsError() { return isError; }
    }
}
