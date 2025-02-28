using GitSharp.Models;
using GitSharp.Serializers.Interfaces;
using OutParsing;

namespace GitSharp.Serializers.Implementations;

internal class TreeSerializer : ITreeSerializer
{
    public Tree Deserialize(string data)
    {
        var tree = new Tree();

        foreach (var line in data.Split(Environment.NewLine))
        {
            OutParser.Parse(
                line,
                "{mode} {type} {hash}\t{name}",
                out string mode,
                out string type,
                out string hash,
                out string name);

            var fileType = Enum.Parse<FileType>(type, ignoreCase: true);
            var modeType = Enum.Parse<Mode>(mode, ignoreCase: true);
            
            tree.TreeEntries.Add(
                new TreeEntry
                {
                    Type = fileType,
                    Hash = hash,
                    Name = name,
                    Mode = modeType
                });
        }

        return tree;
    }

    public string Serialize(Tree tree)
    {
        List<string> strings = [];

        foreach (var treeEntry in tree.TreeEntries)
        {
            var mode = ((int)treeEntry.Mode).ToString();
            if (mode.Length < 6)
            {
                mode = mode.PadLeft(6, '0');
            }
            strings.Add($"{mode} {treeEntry.Type.ToString().ToLower()} {treeEntry.Hash}\t{treeEntry.Name}");
        }

        return string.Join(Environment.NewLine, strings);
    }
}