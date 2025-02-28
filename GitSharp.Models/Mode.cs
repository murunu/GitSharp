namespace GitSharp.Models;

public enum Mode
{
    Directory = 040000,
    NormalFile = 100644,
    ExecutableFile = 100755,
    SymbolicLink = 120000
}