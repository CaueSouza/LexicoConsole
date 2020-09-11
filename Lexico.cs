using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;

namespace LexicoConsole
{
    class Lexico
    {
        private static char actualChar = ' ';
        private static int count = -1;
        private static int lineCount;
        private static string fullString = "";
        private static List<Token> tokenList = new List<Token>();
        private static bool notEOF = true;
        private static FileReader fileReader = new FileReader();

        static void Main()
        {
            executeLexico();
        }

        private static void executeLexico()
        {
            try
            {
                fullString = fileReader.readFile();

                lineCount = 1;

                readCaracter();

                while (notEOF)
                {
                    while ((actualChar == '{' || actualChar == ' ' || actualChar == '/') && notEOF)
                    {
                        if (actualChar == '{')
                        {
                            while (actualChar != '}' && notEOF)
                            {
                                readCaracter();
                            }

                            readCaracter();
                        }


                        if (actualChar == '/' && notEOF)
                        {
                            readCaracter();
                            if (actualChar == '*' && notEOF)
                            {
                                readCaracter();
                                bool endedComment = true;

                                while (endedComment && notEOF)
                                {
                                    while (actualChar != '*' && notEOF)
                                    {
                                        readCaracter();
                                    }

                                    readCaracter();
                                    if (actualChar == '/')
                                    {
                                        readCaracter();
                                        endedComment = false;
                                    }
                                    else
                                    {
                                        readCaracter();
                                    }
                                }
                            }
                            else
                            {
                                notEOF = false;
                                tokenList.Add(new Token("ERRO", "ERRO", true));
                            }
                        }


                        while (actualChar == ' ' && notEOF)
                        {
                            readCaracter();
                        }
                    }

                    if (notEOF)
                    {
                        Token token = readToken();
                        tokenList.Add(token);

                        if (token.getIsError())
                        {
                            break;
                        }
                    }
                }


                foreach (Token t in tokenList)
                {
                    if (!t.getIsError())
                    {
                        Console.WriteLine("Simbolo-> {0}\nLexema-> {1}\n", t.getSimbol(), t.getLexem());
                    }
                    else
                    {
                        Console.WriteLine("Erro na linha {0}\n", lineCount);
                    }
                }

                /*foreach (string line in lines)
                {
                    fullString = line;
                    
                    notEOF = true;
                    lineCount++;

                    if (String.IsNullOrEmpty(fullString)) notEOF = false;
                    else actualChar = fullString[count];


                    while (notEOF)
                    {
                        while ((actualChar == '{' || actualChar == ' ' || actualChar == '/') && notEOF)
                        {
                            if (actualChar == '{')
                            {
                                while (actualChar != '}' && notEOF)
                                {
                                    readCaracter();
                                }

                                readCaracter();
                            }

                            if (actualChar == '/' && notEOF)
                            {
                                readCaracter();
                                if (actualChar == '*' && notEOF)
                                {
                                    readCaracter();

                                    while (actualChar != '*' && notEOF)
                                    {
                                        readCaracter();
                                    }

                                    if (actualChar == '/') { }
                                } else
                                {
                                    tokenList.Add(new Token("ERRO", "ERRO", true));
                                }
                            }


                            while (actualChar == ' ' && notEOF)
                            {
                                readCaracter();
                            }
                        }

                        if (notEOF)
                        {
                            Token token = readToken();
                            tokenList.Add(token);

                            if (token.getIsError())
                            {
                                break;
                            }
                        }
                    }

                    if (tokenList.Count != 0)
                    {
                        Token lastToken = tokenList[tokenList.Count - 1];
                        if (lastToken.getIsError())
                        {
                            break;
                        }
                    }
                }
                */

                //foreach (Token t in tokenList)
                //{
                //    if (!t.getIsError())
                //    {
                //        Console.WriteLine("Simbolo-> {0}\nLexema-> {1}\n", t.getSimbol(), t.getLexem());
                //    }
                //    else
                //    {
                //        Console.WriteLine("Erro na linha {0}\n", lineCount);
                //    }
                //}
            }
            catch (IOException)
            {
                Console.WriteLine("Impossivel ler o arquivo");
            }
        }

        private static void readCaracter()
        {
            if (count == fullString.Length - 1)
            {
                notEOF = false;
            }
            else
            {
                count++;
                actualChar = fullString[count];

                if (actualChar == '\n')
                {
                    lineCount++;
                    count++;
                    actualChar = fullString[count];
                }
            }
        }

        private static bool isDigit()
        {
            if (actualChar >= 48 && actualChar <= 57) return true;
            else return false;
        }

        private static bool isLetter()
        {
            if ((actualChar >= 65 && actualChar <= 90) || (actualChar >= 97 && actualChar <= 122)) return true;
            else return false;
        }

        private static bool isAssignment()
        {
            if (actualChar == ':') return true;
            else return false;
        }

        private static bool isArithmetic()
        {
            if (actualChar == '+' || actualChar == '-' || actualChar == '*') return true;
            else return false;
        }

        private static bool isRelational()
        {
            if (actualChar == '<' || actualChar == '>' || actualChar == '=' || actualChar == '!') return true;
            else return false;
        }

        private static bool isPunctuation()
        {
            if (actualChar == ';' || actualChar == ',' || actualChar == '(' || actualChar == ')' || actualChar == '.') return true;
            else return false;
        }

        private static Token readToken()
        {
            if (isDigit())
            {
                return treatDigit();
            }
            else if (isLetter())
            {
                return treatIdentifierAndReservedWord();
            }
            else if (isAssignment())
            {
                return treatAssignment();
            }
            else if (isArithmetic())
            {
                return treatArithmetic();
            }
            else if (isRelational())
            {
                return treatRelational();
            }
            else if (isPunctuation())
            {
                return treatPunctuation();
            }
            else
            {
                return new Token("ERRO", "ERRO", true);
            }
        }

        private static Token treatDigit()
        {
            string num = actualChar.ToString();
            readCaracter();

            while (isDigit() && notEOF)
            {
                num += actualChar.ToString();
                readCaracter();
            }

            return new Token("snumero", num);
        }

        private static Token treatIdentifierAndReservedWord()
        {
            string id = actualChar.ToString();
            readCaracter();

            while ((isLetter() || isDigit() || actualChar.Equals("_")) && notEOF)
            {
                id += actualChar.ToString();
                readCaracter();
            }

            string simbol;

            switch (id)
            {
                case "programa":
                case "se":
                case "entao":
                case "senao":
                case "enquanto":
                case "faca":
                case "inicio":
                case "fim":
                case "escreva":
                case "leia":
                case "var":
                case "inteiro":
                case "booleano":
                case "verdadeiro":
                case "falso":
                case "procedimento":
                case "funcao":
                case "div":
                case "e":
                case "ou":
                case "nao":
                    simbol = "s" + id;
                    break;
                default:
                    simbol = "sidentificador";
                    break;
            }

            return new Token(simbol, id);
        }

        private static Token treatAssignment()
        {
            string assignment = actualChar.ToString();
            readCaracter();

            if (actualChar.ToString().Equals("="))
            {
                string caracter = actualChar.ToString();
                readCaracter();
                return new Token("satribuicao", assignment + caracter);
            }
            else
            {
                return new Token("sdoispontos", assignment);
            }
        }

        private static Token treatArithmetic()
        {
            string aritmetico = actualChar.ToString();
            readCaracter();

            switch (aritmetico)
            {
                case "+":
                    return new Token("smais", aritmetico);
                case "-":
                    return new Token("smenos", aritmetico);
                case "*":
                    return new Token("smult", aritmetico);
                default:
                    return new Token("ERROR", "ERROR", true);
            }
        }

        private static Token treatRelational()
        {
            string relacional = actualChar.ToString();
            readCaracter();
            string caracter = actualChar.ToString();

            switch (relacional)
            {
                case "<":
                    if (caracter.Equals("="))
                    {
                        readCaracter();
                        return new Token("smenorig", relacional + caracter);
                    }
                    else return new Token("smenor", relacional);

                case ">":
                    if (caracter.Equals("="))
                    {
                        readCaracter();
                        return new Token("smaiorig", relacional + caracter);
                    }
                    else return new Token("smaior", relacional);

                case "!":
                    if (caracter.Equals("="))
                    {
                        readCaracter();
                        return new Token("sdif", relacional + caracter);
                    }
                    else return new Token("ERRO", "ERRO", true);

                case "=":
                    return new Token("sig", relacional);

                default:
                    return new Token("ERRO", "ERRO", true);
            }
        }

        private static Token treatPunctuation()
        {
            string punctuation = actualChar.ToString();
            string simbolo = "";

            switch (punctuation)
            {
                case ";":
                    simbolo = "sponto_virgula";
                    break;
                case ",":
                    simbolo = "svirgula";
                    break;
                case ".":
                    simbolo = "sponto";
                    break;
                case "(":
                    simbolo = "sabre_parenteses";
                    break;
                case ")":
                    simbolo = "sfecha_parenteses";
                    break;
            }

            readCaracter();
            return new Token(simbolo, punctuation);
        }
    }
}
