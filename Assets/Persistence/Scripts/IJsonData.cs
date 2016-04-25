public interface IJsonData<T>
{
    string ToString();

    void Load(string savedData);
}
