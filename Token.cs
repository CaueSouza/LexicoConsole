using System;
using System.Collections.Generic;
using System.Text;

namespace LexicoConsole
{
    class Token
    {
        private String simbolo;
        private String lexema;

        public Token(string simbolo, string lexema)
        {
            this.simbolo = simbolo;
            this.lexema = lexema;
        }

        public string getSimbolo() { return simbolo; }

        public string getLexema() { return lexema; }
    }
}
