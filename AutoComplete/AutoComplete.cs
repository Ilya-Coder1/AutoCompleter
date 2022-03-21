using System.Collections.Generic;
using System.Collections;
using System;
using System.IO;

namespace AutoComplete
{
    class Program
    {
        static void Main()
        {
            AutoCompleter autoCompleter = new AutoCompleter();

            List<FullName> names = new List<FullName>();

            using (StreamReader reader = new StreamReader(File.Open("names.txt", FileMode.Open)))
            {
                while (!reader.EndOfStream)
                {
                    var name =  reader.ReadLine();
                    var parts = name.Split(' ');
                    names.Add(new FullName(parts[0], parts[1], parts[2]));
                }
            }

            autoCompleter.AddToSearch(names);

            Console.WriteLine("Введите строку для поиска:");
            var request = Console.ReadLine();

            System.Diagnostics.Stopwatch timer = new System.Diagnostics.Stopwatch();

            timer.Start();
            var results = autoCompleter.Search(request);
            timer.Stop();
            if (results != null)
            {
                foreach (var result in results)
                {
                    Console.WriteLine(result);
                }
            }
            else
            {
                Console.WriteLine("Не найдено");
            }
            Console.WriteLine(timer.ElapsedTicks);

            Hashtable hashtable = new Hashtable();
        }
    }

    public struct FullName
    {
        public string? Name;
        public string? Surname;
        public string? Patronymic;

        public FullName(string? surname, string? name, string? patronymic)
        {
            Name = name;
            Surname = surname;
            Patronymic = patronymic;
        }
    }

    public class AutoCompleter
    {
        TrieNode _fullNames = TrieNode.CreateRootNode();

        public void AddToSearch(List<FullName> fullNames)
        {
            foreach (var name in fullNames)
            {
                string temp = String.Join(' ', name.Surname?.Trim(), name.Name?.Trim(), name.Patronymic?.Trim());
                _fullNames.Add(temp.Trim());
            }
        }

        public List<string> Search(string prefix)
        {
            List<string> searchResults = new List<string>();

            if (String.IsNullOrEmpty(prefix))
                throw new ArgumentNullException("Введена пустая строка");

            if (String.IsNullOrEmpty(prefix.Replace(" ", "")))
                throw new ArgumentNullException("Введена пустая строка");

            if (prefix.Length > 100)
                throw new ArgumentException("");

            var parsedPrefix = prefix.Split(' ', 3, StringSplitOptions.RemoveEmptyEntries);

            string prefixForSearch = String.Join(' ', parsedPrefix);

            var searchNode = _fullNames.FindString(prefixForSearch);

            if (searchNode != null)
            {
                searchNode.GetAllStrings(prefixForSearch.Substring(0, prefixForSearch.Length - 1), searchResults);
                return searchResults;
            }

            return null;
        }
    }

    public class TrieNode
    {
        private static char rootValue = '~';
        char value;
        List<TrieNode> children;

        static public TrieNode CreateRootNode()
        {
            return new TrieNode(rootValue);
        }

        public TrieNode(char value)
        {
            this.value = value;
        }

        public void Add(string data)
        {
            if (String.IsNullOrEmpty(data))
                return;

            if (children == null)
                children = new List<TrieNode>();

            char c = data[0];
            TrieNode child = FindChar(c);
            if (child == null)
            {
                child = new TrieNode(c);
                children.Add(child);
            }

            child.Add(data.Substring(1));
        }

        private TrieNode FindChar(char c)
        {
            if (children != null)
            {
                foreach (var node in children)
                {
                    if (node.value == c)
                        return node;
                }
            }

            return null;
        }

        public TrieNode FindString(string str)
        {
            TrieNode current = this;

            for (int i = 0; i < str.Length; i++)
            {
                current = current.FindChar(str[i]);
                if (current == null)
                    return null;
            }

            return current;
        }

        public void GetAllStrings(string prefix, List<string> strings)
        {
            if (strings is null)
                strings = new List<string>();

            if (value != rootValue)
            {
                prefix += value;
            }

            if (children != null)
            {
                foreach (var node in children)
                {
                    node.GetAllStrings(prefix, strings);
                }
            }
            else
            {
                strings.Add(prefix);
            }
        }
    }

}
