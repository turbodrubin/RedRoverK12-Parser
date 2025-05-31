namespace DRSoft.RedRoverK12.CodePuzzle
{
    using System;
    using System.Linq;
    using System.Text.RegularExpressions;

    // wWe declare a specific delegate type instead of relying on `Action<Item, int>` because it gives us a singular - and specific - type definition to use with Item.
    delegate void Operation(Item item, int level);

    // This could be a record type, but we don't really care about the value-equality semantics so we'll stick with readonly struct.
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

        public string Name { get; }
        public Item[] Children { get; }

        // This could have been `Print()` and `Print(ordering)`, but we'll indulge in a bit of functional-programming separate the operation from the tree-walking.
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

        // Composites lend themselves naturally to recursion, don't they?
        //
        // This implementation assumes that `input` represents a single item (possibly with children).
        // Hence the use of Match() on `input`, and Matches() when parsing out the children.
        public static Item Parse(string input)
        {
            var match = _splitter.Match(input);
            if (!match.Success)
                // If we cared about performance, we could build some so-called "throw helpers". But we don't, so we won't.
                throw new ArgumentException("input could not be parsed", nameof(input), new FormatException("input does not conform to expected format"));

            var name = match.Groups["name"].Value;

            var children = match.Groups["children"].Value;
            if (children.Length > 0)
            {
                // `children` starts like "(child1, child2, ...)", so we trim the parenthesis before recursively applying the regex
                var childMatches = _splitter.Matches(children[1..^1]);
                var childItems = childMatches.Select(child => Parse(child.Value)).ToArray();
                return new Item(name, childItems);
            }
            else
                return new Item(name);
        }

        // Regexs can get nasty. The idea with this one is to look for strings like:
        // (name1, name2, name3(child1, child2(grandchild1, grandchild2), child3), name4, ...)
        // and capture the names and the entire child set for recursive parsing.
        private static readonly Regex _splitter = new(@"(?<name>\w*)(?<children>\(.*\))?\,?\s*");
    }
}
