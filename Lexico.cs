﻿using System;
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
                            int lineCommentStarted = lineCount;

                            while (actualChar != '}' && notEOF)
                            {
                                readCaracter();
                            }

                            if (!notEOF && actualChar != '}') createErrorToken(lineCommentStarted, 1);

                            readCaracter();
                        }


                        if (actualChar == '/' && notEOF)
                        {
                            readCaracter();
                            if (actualChar == '*' && notEOF)
                            {
                                int lineCommentStarted = lineCount;

                                readCaracter();
                                bool endedComment = true;

                                while (endedComment && notEOF)
                                {
                                    while (actualChar != '*' && notEOF)
                                    {
                                        readCaracter();
                                    }

                                    if (notEOF)
                                    {
                                        readCaracter();

                                        if (notEOF)
                                        {
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
                                        else createErrorToken(lineCommentStarted, 1);
                                    }
                                    else createErrorToken(lineCommentStarted, 1);
                                }
                            }
                            else
                            {
                                notEOF = false;
                                createErrorToken(lineCount, 2);
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
                    if (t.getIsError())
                    {
                        Console.WriteLine("Erro na linha {0}", t.getLine());
                        treatErrorType(t.getErrorType());
                    }
                    else
                    {
                        Console.WriteLine("Simbolo-> {0}\nLexema-> {1}\nLinha-> {2}\n", t.getSimbol(), t.getLexem(), t.getLine());
                    }
                }
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
                    if (count != fullString.Length - 1)
                    {
                        count++;
                        actualChar = fullString[count];
                    }
                    else
                    {
                        notEOF = false;
                    }
                }
            }
        }

        private static void createErrorToken(int errorLine, int errorType)
        {
            tokenList.Add(new Token(errorLine, errorType));
        }

        private static void treatErrorType(int errorType)
        {
            switch (errorType)
            {
                case 1:
                    Console.WriteLine("Comentario aberto sem fechamento\n");
                    break;
                case 2:
                    Console.WriteLine("Caracter Invalido\n");
                    break;
                default:
                    Console.WriteLine("Erro nao identificado\n");
                    break;
            }
        }

        private static bool isDigit()
        {
            return Char.IsDigit(actualChar);
        }

        private static bool isLetter()
        {
            return Char.IsLetter(actualChar);
        }

        private static bool isAssignment()
        {
            return actualChar == ':';
        }

        private static bool isArithmetic()
        {
            return actualChar == '+' || actualChar == '-' || actualChar == '*';
        }

        private static bool isRelational()
        {
            return actualChar == '<' || actualChar == '>' || actualChar == '=' || actualChar == '!';
        }

        private static bool isPunctuation()
        {
            return actualChar == ';' || actualChar == ',' || actualChar == '(' || actualChar == ')' || actualChar == '.';
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
                return new Token(lineCount, 2);
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

            return new Token("snumero", num, lineCount);
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

            return new Token(simbol, id, lineCount);
        }

        private static Token treatAssignment()
        {
            string assignment = actualChar.ToString();
            readCaracter();

            if (actualChar.ToString().Equals("="))
            {
                string caracter = actualChar.ToString();
                readCaracter();
                return new Token("satribuicao", assignment + caracter, lineCount);
            }
            else
            {
                return new Token("sdoispontos", assignment, lineCount);
            }
        }

        private static Token treatArithmetic()
        {
            string aritmetico = actualChar.ToString();
            readCaracter();

            switch (aritmetico)
            {
                case "+":
                    return new Token("smais", aritmetico, lineCount);
                case "-":
                    return new Token("smenos", aritmetico, lineCount);
                case "*":
                    return new Token("smult", aritmetico, lineCount);
                default:
                    return new Token(lineCount, 3);
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
                        return new Token("smenorig", relacional + caracter, lineCount);
                    }
                    else return new Token("smenor", relacional, lineCount);

                case ">":
                    if (caracter.Equals("="))
                    {
                        readCaracter();
                        return new Token("smaiorig", relacional + caracter, lineCount);
                    }
                    else return new Token("smaior", relacional, lineCount);

                case "!":
                    if (caracter.Equals("="))
                    {
                        readCaracter();
                        return new Token("sdif", relacional + caracter, lineCount);
                    }
                    else return new Token(lineCount, 2);

                case "=":
                    return new Token("sig", relacional, lineCount);

                default:
                    return new Token(lineCount, 3);
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
            return new Token(simbolo, punctuation, lineCount);
        }
    }
}
