namespace Meganium.Api
{
    //Refactor: tirar este monte de opções
    public interface IOptions
    {
        object Get(string name);
        string GetString(string name);
        int GetInt(string name);
        long GetLong(string name);
        int Get(string name, int defaultValue);
        bool Get(string name, bool defaultValue);
        void Set(string name, object value);
    }
}