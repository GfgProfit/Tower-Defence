using System.Collections.Generic;
using UnityEngine;

public class LevelLoader
{
    public IReadOnlyList<LevelConfig> Levels
    {
        get
        {
            return new List<LevelConfig>(Resources.LoadAll<LevelConfig>("Levels"));
        }
    }
}