namespace DRSoft.RedRoverK12.CodePuzzle
{
    using System;

    class Program
    {
        public static void Main(string[] args)
        {
            var root = new Item("", [
                new Item("id"),
                new Item("name"),
                new Item("email"),
                new Item("type", [
                    new Item("id"),
                    new Item("name"),
                    new Item("customFields", [
                        new Item("c1"),
                        new Item("c2"),
                        new Item("c3")
                    ]),
                ]),
                new Item("externalId")
            ]);

            root.Apply(PrintItems, -1);
            Console.WriteLine("=========================");
            root.Apply(PrintItems, item => item.Name, -1);

            Console.ReadKey();
        }

        private static void PrintItems(Item item, int numIndents)
        {
            if (!String.IsNullOrWhiteSpace(item.Name))
            {
                string indents = new(' ', numIndents * 2);
                Console.WriteLine($"{indents}- {item.Name}");
            }
        }
    }
}