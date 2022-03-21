using System;
using System.Collections.Generic;
using System.IO;
using AutoComplete;

namespace Program
{
    public class Program
    {
        static void Main()
        {
            AutoCompleter autoCompleter = new AutoCompleter();

            List<FullName> names = new List<FullName>();

            using (StreamReader reader = new StreamReader(File.Open("names.txt", FileMode.Open)))
            {
                while (!reader.EndOfStream)
                {
                    var name = reader.ReadLine();
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
        }

    }
}
