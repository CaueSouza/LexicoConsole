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

            //using (StreamReader sr = File.OpenText(fullPath))
            //{
            //    string line;
            //    while ((line = sr.ReadLine()) != null)
            //    {
            //        // Do something with line...
            //        Console.WriteLine(line);
            //    }
            //}

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
                        //listaTokens.Add(readToken());
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
            } else
            {
                cont++;
                caracterAtual = fullString[cont];
            }
        }

        private static Token readToken()
        {
            return null;
        }
    }
}
