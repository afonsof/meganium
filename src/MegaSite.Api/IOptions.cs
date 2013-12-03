namespace MegaSite.Api
{
    //Refactor: tirar este monte de opções
    public interface IOptions
    {
        string Get(string name);
        int GetInt(string name);
        long GetLong(string name);
        int Get(string name, int defaultValue);
        bool Get(string name, bool defaultValue);
        void Set(string name, int? value);
        void Set(string name, long? value);
        void Set(string name, string value);
    }
}