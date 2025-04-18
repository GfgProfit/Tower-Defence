[System.Serializable]
public struct StatData
{
    public string Name { get; private set; }
    public string Value { get; private set; }

    public StatData(string name, string value)
    {
        Name = name;
        Value = value;
    }
}