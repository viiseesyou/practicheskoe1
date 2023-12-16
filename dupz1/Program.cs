using System;
using System.Collections.Generic;

class Grammar
{
    private HashSet<char> nonTerminals;
    private Dictionary<char, List<string>> rules;
    private HashSet<char> alphabet;

    public Grammar()
    {
        nonTerminals = new HashSet<char>();
        rules = new Dictionary<char, List<string>>();
        alphabet = new HashSet<char>();
    }

    public void AddRule(char nonTerminal, string rule)
    {
        if (!rules.ContainsKey(nonTerminal))
        {
            rules[nonTerminal] = new List<string>();
            nonTerminals.Add(nonTerminal);
        }
        rules[nonTerminal].Add(rule);
        foreach (char symbol in rule)
        {
            if (!nonTerminals.Contains(symbol))
            {
                alphabet.Add(symbol);
            }
        }
    }

    public IEnumerable<string> GenerateWords(int n)
    {
        char startSymbol = 'S';
        List<string> words = new List<string>();
        bool foundWords = false;
        foreach (string word in GenerateWordsRecursive(startSymbol.ToString(), n, n))
        {
            words.Add(word);
            foundWords = true;
        }
        if (!foundWords)
        {
            throw new Exception("Слов длины " + n + " не существует.");
        }
        return words;
    }

    private IEnumerable<string> GenerateWordsRecursive(string currentWord, int n, int maxDepth)
    {
        if (currentWord.Length == n)
        {
            yield return currentWord;
            yield break;
        }
        if (maxDepth == 0)
        {
            yield break;
        }
        char currentSymbol = currentWord[currentWord.Length - 1];
        if (nonTerminals.Contains(currentSymbol))
        {
            foreach (string rule in rules[currentSymbol])
            {
                foreach (string word in GenerateWordsRecursive(currentWord + rule, n, maxDepth - 1))
                {
                    yield return word;
                }
            }
        }
        else
        {
            foreach (char symbol in alphabet)
            {
                foreach (string word in GenerateWordsRecursive(currentWord + symbol, n, maxDepth - 1))
                {
                    yield return word;
                }
            }
        }
    }


    class MainClass
    {
        public static void Main()
        {
            Grammar grammar = new Grammar();
            grammar.AddRule('S', "aSb");
            grammar.AddRule('S', "");
            foreach (int n in ReadInts())
            {
                try
                {
                    foreach (string word in grammar.GenerateWords(n))
                    {
                        Console.WriteLine(word);
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
        }

        private static IEnumerable<int> ReadInts()
        {
            while (true)
            {
                Console.Write("Введите длину слов (n): ");
                string input = Console.ReadLine();
                if (string.IsNullOrWhiteSpace(input))
                {
                    yield break;
                }
                if (int.TryParse(input, out int n))
                {
                    yield return n;
                }
                else
                {
                    Console.WriteLine("Ошибка ввода.");
                }
            }
        }
    }
}