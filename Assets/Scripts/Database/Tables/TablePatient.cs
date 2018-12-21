using SQLite4Unity3d;

public class TablePatient
{
    [PrimaryKey, AutoIncrement]
    public int pk_patient { get; set; }
    public int fk_kinesiologist { get; set; }
    public string name { get; set; }
}
