using SQLite4Unity3d;

public class TableGame
{
    [PrimaryKey, AutoIncrement]
    public int pk_game { get; set; }
    public string name { get; set; }
}
