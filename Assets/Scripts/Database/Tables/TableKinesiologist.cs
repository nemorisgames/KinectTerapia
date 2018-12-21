using SQLite4Unity3d;

public class TableKinesiologist
{
    [PrimaryKey, AutoIncrement]
    public int pk_kinesiologist { get; set; }
    public string name { get; set; }
    public string username { get; set; }
    public string password { get; set; }
}
