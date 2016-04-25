using UnityEngine;

public class Device : IJsonData<Device>
{
	public int Id  { get; set; }
	public string Name { get; set; }
	public string Description { get; set; }

    public Device()
    {
        this.Name = string.Empty;
        this.Description = string.Empty;
    }

    public Device(int id, string name, string description)
    {
        this.Id = id;
        this.Name = name;
        this.Description = description;
    }

    public static Device CreateFromJSON(string jsonString)
    {
        return JsonUtility.FromJson<Device>(jsonString);
    }

    public string SaveToString()
    {
        return JsonUtility.ToJson(this);
    }

    public void Load(string savedData)
    {
        JsonUtility.FromJsonOverwrite(savedData, this);
    }
}
