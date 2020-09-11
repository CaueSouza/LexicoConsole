using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace LexicoConsole
{
    class FileReader
    {
        public FileReader()
        {

        }

        public string readFile()
        {
            string filePath, fileName;
            Console.Write("Informe o caminho do arquivo: ");
            filePath = Console.ReadLine();
            Console.Write("Informe o nome do arquivo: ");
            fileName = Console.ReadLine();

            Console.WriteLine("Path: {0}\nName: {1}\n", filePath, fileName);

            string fullPath = filePath + '\\' + fileName;
            Console.WriteLine("Full Path: {0}\n", fullPath);

            return File.ReadAllText(fullPath).Replace("\r\n", " \n");
        }
    }
}
