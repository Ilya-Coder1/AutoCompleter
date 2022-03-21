using System.Collections.Generic;
using System.Collections;
using System;
using System.IO;
using Trie;

namespace AutoComplete
{
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
}
