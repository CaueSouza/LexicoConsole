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

        static void Main(string[] args)
        {
            string filePath, fileName;
            Console.Write("Informe o caminho do arquivo: ");
            filePath = Console.ReadLine();
            Console.Write("Informe o nome do arquivo: ");
            fileName = Console.ReadLine();

            Console.WriteLine("Path: {0}\nName: {1}\n", filePath, fileName);

            string fullPath = filePath + '\\' + fileName;
            Console.WriteLine("Full Path: {0}", fullPath);

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
                        readCaracter();//temporario
                    }
                }
            }
            catch (IOException e)
            {
                Console.WriteLine("Impossivel ler o arquivo");
            }
        }

        private static void readCaracter()
        {
            if (cont == fullString.Length-1) {
                notEOF = false;
            } else {
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
            else
            {
                return null;
            }
        }

        private static Token treatDigit()
        {
            return new Token("teste", "teste");
        }

        private static Token treatIdentifierAndReservedWord()
        {
            return new Token("teste", "teste");
        }

        private static Token treatAssignment()
        {
            return new Token("teste", "teste");
        }

        private static Token treatArithmetic()
        {
            return new Token("teste", "teste");
        }

        private static Token treatRelational()
        {
            return new Token("teste", "teste");
        }
    }
}
