using System;
using System.Collections.Generic;
using System.Text;

namespace LexicoConsole
{
    class Token
    {
        private String simbolo;
        private String lexema;
        private bool isError;

        public Token(string simbolo, string lexema)
        {
            this.simbolo = simbolo;
            this.lexema = lexema;
            this.isError = false;
        }

        public Token(string simbolo, string lexema, bool isError)
        {
            this.simbolo = simbolo;
            this.lexema = lexema;
            this.isError = true;
        }

        public string getSimbolo() { return simbolo; }

        public string getLexema() { return lexema; }

        public bool getIsError() { return isError; }
    }
}
