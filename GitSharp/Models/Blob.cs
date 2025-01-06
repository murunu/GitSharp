namespace GitSharp.Models;

public class Blob : ModelBase
{
    public Blob(byte[] parentHash)
    {
        VerifyHash(parentHash);

        ParentHash = parentHash;
    }
}