using System.Collections.Generic;

public interface ITowerStats
{
    List<StatData> GetStats();
    public List<StatData> GetStatsAfterUpgrade();
}