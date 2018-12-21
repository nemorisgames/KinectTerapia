using SQLite4Unity3d;

public class TableAdmin
{
    [PrimaryKey, AutoIncrement]
    public int pk_admin { get; set; }
    public string username { get; set; }
    public string password { get; set; }
}
