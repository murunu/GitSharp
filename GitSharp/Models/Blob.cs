using System.Text;
using GitSharp.Helpers;

namespace GitSharp.Models;

public class Blob : ModelBase
{
    public static async Task<Blob?> FromHash(string hash)
    {
        var blob = new Blob()
        {
            Hash = Convert.FromHexString(hash)
        };
        
        return blob;
    }
}