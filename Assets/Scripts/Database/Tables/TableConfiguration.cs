using SQLite4Unity3d;

public class TableConfiguration
{
    [PrimaryKey, AutoIncrement]
    public int pk_configuration { get; set; }
    public int hand { get; set; } //(-1, 0, 1)
    public float rangeHorMin { get; set; }
    public float rangeHorMax { get; set; }
    public float rangeVerMin { get; set; }
    public float rangeVerMax { get; set; }
    public float rangeDeepMin { get; set; }
    public float rangeDeepMax { get; set; }
}
