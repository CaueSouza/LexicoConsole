using System;
using System.IO;

namespace LexicoConsole
{
    class Program
    {
        
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

            try
            {
                string[] lines = File.ReadAllLines(fullPath);

                foreach (string line in lines)
                {

                }
            }
            catch (IOException e)
            {
                Console.WriteLine("Impossivel ler o arquivo");
            }
        }
    }
}
