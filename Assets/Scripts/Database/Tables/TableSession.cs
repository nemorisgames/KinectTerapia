using SQLite4Unity3d;

public class TableSession
{
    [PrimaryKey, AutoIncrement]
    public int pk_session { get; set; }
    public int fk_patient { get; set; }
    public int fk_configuration { get; set; }
    public int fk_game { get; set; }
    public string dateSession { get; set; }
    public int score { get; set; }
    public float speedMin { get; set; }
    public float speedMax { get; set; }
    public string heatMap { get; set; }
}
