namespace DRSoft.RedRoverK12.CodePuzzle
{
    using System;

    class Program
    {
        public static void Main(string[] args)
        {
            // this could just as easily be `args[0]`
            string input = "(id, name, email, type(id, name, customFields(c1, c2, c3)), externalId)";

            var root = Item.Parse(input);

            // given the input, we expect `root` not to have a name. PrintItems() accounts for empty names,
            // but we don't want the extra indentation, so we start "back" one level.
            root.Apply(PrintItems, -1);

            Console.WriteLine("=========================");

            root.Apply(PrintItems, item => item.Name, -1);
        }

        private static void PrintItems(Item item, int numIndents)
        {
            if (!String.IsNullOrWhiteSpace(item.Name))
            {
                // this string constructor can only take a single space (' ') character, but we want an indent to be two spaces
                string indents = new(' ', numIndents * 2);
                Console.WriteLine($"{indents}- {item.Name}");
            }
        }
    }
}