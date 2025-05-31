namespace DRSoft.RedRoverK12.CodePuzzle
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text.RegularExpressions;

    delegate void Operation(Item item, int level);

    readonly struct Item
    {
        public Item(string name)
            : this(name, [])
        { }

        public Item(string name, Item[] children)
        {
            Name = name;
            Children = children;
        }

        public static void Parse(string input)
        {

        }

        public void Apply(Operation action, int level = 0)
        {
            action(this, level);
            foreach (var child in Children)
                child.Apply(action, level + 1);
        }

        public void Apply<TKey>(Operation action, Func<Item, TKey> orderby, int level = 0)
        {
            action(this, level);
            foreach (var child in Children.OrderBy(orderby))
                child.Apply(action, orderby, level + 1);
        }

        public string Name { get; }
        public Item[] Children { get; }

        private static readonly Regex _splitter = new(@"(?<name>\w*)(?<children>\(.*\))?\,?\s*");
    }
}
