using System;
using System.Collections.Generic;
using System.Text;

namespace LexicoConsole
{
    class Token
    {
        public String simbolo;
        public String lexema;

        public Token(string simbolo, string lexema)
        {
            this.simbolo = simbolo;
            this.lexema = lexema;
        }

        public string getSimbolo() { return simbolo; }

        public string getLexema() { return lexema; }
    }
}
