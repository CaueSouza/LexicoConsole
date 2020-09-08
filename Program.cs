using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography.X509Certificates;

namespace LexicoConsole
{
    class Program
    {
        private static char caracterAtual = ' ';
        private static int cont = 0;
        private static string fullString = "";
        private static List<Token> listaTokens = new List<Token>();
        private static bool notEOF = true;

        static void Main()
        {
            string filePath, fileName;
            Console.Write("Informe o caminho do arquivo: ");
            filePath = Console.ReadLine();
            Console.Write("Informe o nome do arquivo: ");
            fileName = Console.ReadLine();

            Console.WriteLine("Path: {0}\nName: {1}\n", filePath, fileName);

            string fullPath = filePath + '\\' + fileName;
            Console.WriteLine("Full Path: {0}\n", fullPath);

            readFile(fullPath);
        }

        private static void readFile(String fullPath)
        {
            try
            {
                string[] lines = File.ReadAllLines(fullPath);

                foreach (string line in lines)
                {
                    fullString += " " + line;
                }

                caracterAtual = fullString[cont];

                while (notEOF)
                {
                    while ((caracterAtual == '{' || caracterAtual == ' ') && notEOF)
                    {
                        if (caracterAtual == '{')
                        {
                            while (caracterAtual != '}' && notEOF)
                            {
                                readCaracter();
                            }

                            readCaracter();
                        }

                        while (caracterAtual == ' ')
                        {
                            readCaracter();
                        }
                    }

                    if (notEOF)
                    {
                        Token token = readToken();
                        listaTokens.Add(token);
                    }
                }

                foreach (Token t in listaTokens)
                {
                    Console.WriteLine("Simbolo-> {0}\nLexema-> {1}\n", t.getSimbolo(), t.getLexema());
                }
            }
            catch (IOException e)
            {
                Console.WriteLine("Impossivel ler o arquivo");
            }
        }

        private static void readCaracter()
        {
            if (cont == fullString.Length - 1)
            {
                notEOF = false;
            }
            else
            {
                cont++;
                caracterAtual = fullString[cont];
            }
        }

        private static bool isDigit()
        {
            if (caracterAtual >= 48 && caracterAtual <= 57) return true;
            else return false;
        }

        private static bool isLetter()
        {
            if ((caracterAtual >= 65 && caracterAtual <= 90) || (caracterAtual >= 97 && caracterAtual <= 122)) return true;
            else return false;
        }

        private static bool isAssignment()
        {
            if (caracterAtual == ':') return true;
            else return false;
        }

        private static bool isArithmetic()
        {
            if (caracterAtual == '+' || caracterAtual == '-' || caracterAtual == '*') return true;
            else return false;
        }

        private static bool isRelational()
        {
            if (caracterAtual == '<' || caracterAtual == '>' || caracterAtual == '=' || caracterAtual == '!') return true;
            else return false;
        }

        private static bool isPunctuation()
        {
            if (caracterAtual == ';' || caracterAtual == ',' || caracterAtual == '(' || caracterAtual == ')' || caracterAtual == '.') return true;
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
                return null;
            }
        }

        private static Token treatDigit()
        {
            string num = caracterAtual.ToString();
            readCaracter();

            while (isDigit() && notEOF)
            {
                num += caracterAtual.ToString();
                readCaracter();
            }

            return new Token("snumero", num);
        }

        private static Token treatIdentifierAndReservedWord()
        {
            string id = caracterAtual.ToString();
            readCaracter();

            while ((isLetter() || isDigit() || id.Equals("_")) && notEOF)
            {
                id += caracterAtual.ToString();
                readCaracter();
            }

            string simbolo;

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
                    simbolo = "s" + id;
                    break;
                default:
                    simbolo = "sidentificador";
                    break;
            }

            return new Token(simbolo, id);
        }

        private static Token treatAssignment()
        {
            string assignment = caracterAtual.ToString();
            readCaracter();

            if (assignment.Equals("="))
            {
                string caracter = caracterAtual.ToString();
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
            return new Token("teste", "teste");
        }

        private static Token treatRelational()
        {
            string relacional = caracterAtual.ToString();
            readCaracter();
            string caracter = caracterAtual.ToString();

            switch (relacional)
            {
                case "<":
                    if (caracterAtual.Equals("="))
                    {
                        readCaracter();
                        return new Token("smenorig", relacional + caracter);
                    } 
                    else return new Token("smenor", relacional);

                case ">":
                    if (caracterAtual.Equals("="))
                    {
                        readCaracter();
                        return new Token("smaiorig", relacional + caracter);
                    }
                    else return new Token("smaior", relacional);

                case "!":
                    if (caracterAtual.Equals("="))
                    {
                        readCaracter();
                        return new Token("sdif", relacional + caracter);
                    }
                    else return new Token("ERRO", "ERRO");

                case "=":
                    return new Token("sig", relacional);

                default:
                    return new Token("ERRO", "ERRO");
            }
        }

        private static Token treatPunctuation()
        {
            string punctuation = caracterAtual.ToString();
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
