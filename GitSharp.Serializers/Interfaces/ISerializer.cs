namespace GitSharp.Serializers.Interfaces;

public interface ISerializer<T>
{
    T Deserialize(string data);
    
    string Serialize(T obj);
}