using System;
using System.Collections.Generic;
using System.Text;

namespace Trie
{
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
