using System.Diagnostics.CodeAnalysis;
using GitSharp.Models;

namespace GitSharp.Serializers.Parsable;

internal class ParsableFileType : IParsable<ParsableFileType>
{
    public FileType Value { get; private set; }

    public static ParsableFileType Parse(string s, IFormatProvider? provider)
    {
        return new ParsableFileType()
        {
            Value = Enum.Parse<FileType>(s, ignoreCase: true)
        };
    }

    public static bool TryParse(
        [NotNullWhen(true)] string? s,
        IFormatProvider? provider,
        [MaybeNullWhen(false)] out ParsableFileType result)
    {
        var canParse = Enum.TryParse<FileType>(s, ignoreCase: true, out var fileType);

        if (!canParse)
        {
            result = null;
            return false;
        }
        
        result = new ParsableFileType
        {
            Value = fileType
        };

        return true;
    }
}