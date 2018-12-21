using SQLite4Unity3d;
using UnityEngine;
#if !UNITY_EDITOR
using System.Collections;
using System.IO;
#endif
using System.Collections.Generic;

//A complete example of how to work with SQLLite is in the project SQLite4Unity3d-master
//Source: https://github.com/robertohuertasm/SQLite4Unity3d
public class DatabaseManager : MonoBehaviour {
    public static DatabaseManager instance = null;
    private SQLiteConnection DBConnection;
    public bool testForceDBCreation = false;
    // Use this for initialization
    void Start()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            if (instance != null)
            {
                Destroy(gameObject);
            }
        }
        if (testForceDBCreation)
            CreateDB("JuegosKinect.db");
        else
            DataService("JuegosKinect.db"); 
    }

    public void DataService(string DatabaseName)
    {
        var dbPath = string.Format(@"Assets/StreamingAssets/{0}", DatabaseName);
        try
        {
            //Try to open de database. If it does not exists, it sends an error
            DBConnection = new SQLiteConnection(dbPath, SQLiteOpenFlags.ReadWrite | SQLiteOpenFlags.Create);
            /*var admin = GetAdmin();
            foreach (var a in admin)
            {
                print(a.pk_admin + " " + a.username);
            }*/
        }
        catch (SQLiteException s){
            CreateDB(DatabaseName);
        }
    }

    public void CreateDB(string DatabaseName)
    {
        DBConnection = new SQLiteConnection(string.Format(@"Assets/StreamingAssets/{0}", DatabaseName), SQLiteOpenFlags.ReadWrite | SQLiteOpenFlags.Create);
        DBConnection.DropTable<TableAdmin>();
        DBConnection.CreateTable<TableAdmin>();
        DBConnection.InsertAll(new[]{
            new TableAdmin{
                username = "Admin",
                password = "Admin"
            }
        });

        DBConnection.DropTable<TableGame>();
        DBConnection.CreateTable<TableGame>();
        DBConnection.InsertAll(new[]{
            new TableGame{
                pk_game = 1,
                name = "Game 1"
            },
            new TableGame{
                pk_game = 2,
                name = "Game 2"
            },
            new TableGame{
                pk_game = 3,
                name = "Game 3"
            },
            new TableGame{
                pk_game = 4,
                name = "Game 4"
            },
            new TableGame{
                pk_game = 5,
                name = "Game 5"
            },
            new TableGame{
                pk_game = 6,
                name = "Game 6"
            },
            new TableGame{
                pk_game = 7,
                name = "Game 7"
            },
            new TableGame{
                pk_game = 8,
                name = "Game 8"
            },
            new TableGame{
                pk_game = 9,
                name = "Game 9"
            }
        });

        //DATABASE TEST DATA
        DBConnection.DropTable<TableKinesiologist>();
        DBConnection.CreateTable<TableKinesiologist>();
        DBConnection.InsertAll(new[]{
            new TableKinesiologist{
                pk_kinesiologist = 1, //needed to testing
                name = "TestKinesiologist1",
                username = "test1",
                password = "test1"
            },
            new TableKinesiologist{
                pk_kinesiologist = 2, //needed to testing
                name = "TestKinesiologist2",
                username = "test2",
                password = "test2"
            }
        });

        DBConnection.DropTable<TablePatient>();
        DBConnection.CreateTable<TablePatient>();
        DBConnection.InsertAll(new[]{
            new TablePatient{
                pk_patient = 1,
                name = "TestPatient1",
                fk_kinesiologist = 1
            },
            new TablePatient{
                pk_patient = 2,
                name = "TestPatient2",
                fk_kinesiologist = 1
            },
            new TablePatient{
                pk_patient = 3,
                name = "TestPatient3",
                fk_kinesiologist = 2
            }
        });

        DBConnection.DropTable<TableConfiguration>();
        DBConnection.CreateTable<TableConfiguration>();
        DBConnection.InsertAll(new[]{
            new TableConfiguration{
                pk_configuration = 1,
                hand = 1,
                rangeHorMin = -10f,
                rangeHorMax = 10f,
                rangeVerMin = -10f,
                rangeVerMax = 10f,
                rangeDeepMin = -10f,
                rangeDeepMax = 10f
            }
        });

        DBConnection.DropTable<TableSession>();
        DBConnection.CreateTable<TableSession>();
        DBConnection.InsertAll(new[]{
            new TableSession{
                pk_session = 1,
                fk_configuration = 1,
                fk_game = 1,
                fk_patient = 1,
                score = 1000,
                dateSession = "2018-12-23 10:00:00",
                speedMin = 10f,
                speedMax = 20f,
                heatMap = "image.png"
            }
        });

        print("Database created");
    }
    /*
    public IEnumerable<TableAdmin> GetAdmin()
    {
        return DBConnection.Table<TableAdmin>();
    }*/

    public int Login(string username, string password)
    {
        int pk_kinesiologist = -1;
        var k = DBConnection.Table<TableKinesiologist>().Where(x => x.username == username && x.password == password);
        if(k.Count() > 0)
            pk_kinesiologist = k.First().pk_kinesiologist;
        return pk_kinesiologist;
    }

    public TablePatient[] GetPatientsInKinesiologist()
    {
        int fk_kinesiologist = PlayerPrefs.GetInt("pk_kinesiologist");
        var p = DBConnection.Table<TablePatient>().Where(x => x.fk_kinesiologist == fk_kinesiologist);
        if (p.Count() > 0)
        {
            TablePatient[] tablePatients = new TablePatient[p.Count()];
            for (int i = 0; i < p.Count(); i++)
            {
                tablePatients[i] = p.ElementAt(i);
            }
            return tablePatients;
        }
        return null;
    }

    // Update is called once per frame
    void Update () {
		
	}
}
