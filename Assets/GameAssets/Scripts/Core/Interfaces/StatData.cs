[System.Serializable]
public struct StatData
{
    public string Value { get; private set; }

    public StatData(string value)
    {
        Value = value;
    }
}