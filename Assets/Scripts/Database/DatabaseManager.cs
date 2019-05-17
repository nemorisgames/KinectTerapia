﻿using SQLite4Unity3d;
using UnityEngine;
#if !UNITY_EDITOR
using System.Collections;
using System.IO;
#endif
using System.Collections.Generic;

//A complete example of how to work with SQLLite is in the project SQLite4Unity3d-master
//Source: https://github.com/robertohuertasm/SQLite4Unity3d
//Examples: https://github.com/praeclarum/sqlite-net
public class DatabaseManager : MonoBehaviour
{
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

        testForceDBCreation = (PlayerPrefs.GetInt("TestDB", 0) == 1 ? true : false);
        if (testForceDBCreation)
            CreateDB("JuegosKinect.db");
        else
            DataService("JuegosKinect.db");
    }

    public void DataService(string DatabaseName)
    {
        var dbPath = string.Format(Application.persistentDataPath + "/{0}", DatabaseName);
        //var dbPath = string.Format(Application.dataPath + "/Resources" + "/{0}", DatabaseName);
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
        catch (SQLiteException s)
        {
            CreateDB(DatabaseName);
        }
    }

    public void CreateDB(string DatabaseName)
    {
        //Application.persistentDataPath
        //DBConnection = new SQLiteConnection(string.Format(Application.dataPath + "/Resources" + "/{0}", DatabaseName), SQLiteOpenFlags.ReadWrite | SQLiteOpenFlags.Create);
        DBConnection = new SQLiteConnection(string.Format(Application.persistentDataPath + "/{0}", DatabaseName), SQLiteOpenFlags.ReadWrite | SQLiteOpenFlags.Create);
        //print(Application.persistentDataPath + "/" + DatabaseName);
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
            },
            new TableSession{
                pk_session = 2,
                fk_configuration = 1,
                fk_game = 1,
                fk_patient = 1,
                score = 2000,
                dateSession = "2018-12-24 10:00:00",
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
        if (k.Count() > 0)
            pk_kinesiologist = k.First().pk_kinesiologist;
        return pk_kinesiologist;
    }

    public void SaveSession(string game, int score, float speedMin, float speedMax)
    {
        int currentGame = int.Parse(game.Substring(4, 1));
        int currentConfiguration = PlayerPrefs.GetInt("currentConfiguration", 1);
        DBConnection.Insert(new TableSession
        {
            fk_configuration = currentConfiguration,
            fk_game = currentGame,
            fk_patient = PlayerPrefs.GetInt("pk_patient"),
            score = score,
            dateSession = System.DateTime.Now.ToString(),
            speedMin = speedMin,
            speedMax = speedMax,
            heatMap = "image.png"
        });
        /*List<TableSession> ts = DBConnection.Query<TableSession>("select * from TableSession");
        foreach(TableSession t in ts)
            Debug.Log(t.pk_session + " | " + t.fk_game+ " | "+t.score);*/
    }

    public void SaveConfiguration()
    {
        List<TableConfiguration> tc = DBConnection.Query<TableConfiguration>("select * from TableConfiguration");
        int conf = tc.Count + 1;
        DBConnection.Insert(new TableConfiguration
        {
            pk_configuration = conf,
            hand = PlayerPrefs.GetInt("handSelected"),
            rangeHorMin = PlayerPrefs.GetFloat("limitHorMin"),
            rangeHorMax = PlayerPrefs.GetFloat("limitHorMax"),
            rangeVerMin = PlayerPrefs.GetFloat("limitVerMin"),
            rangeVerMax = PlayerPrefs.GetFloat("limitVerMax"),
            rangeDeepMin = PlayerPrefs.GetFloat("limitDepthMin"),
            rangeDeepMax = PlayerPrefs.GetFloat("limitDepthMax")
        });
        PlayerPrefs.SetInt("currentConfiguration", conf);
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

    public List<TablePatientsResult> GetResultsOnPatient(int pk_patient)
    {
        //List<TableSession> s = DatabaseManager.instance.DBConnection.Query<TableSession>("select \"g.name\" as \"pk_session\", \"MAX(s.score)\" as \"pk_game\" from TableSession s, TableGame g where s.fk_game = g.pk_game AND s.fk_patient = ? GROUP BY g.name", pk_patient);
        List<TableSession> session = DatabaseManager.instance.DBConnection.Query<TableSession>("select pk_session, fk_game, MAX(score) as score from TableSession where fk_patient = ? GROUP BY fk_game", pk_patient);
        List<TablePatientsResult> tableResult = new List<TablePatientsResult>();
        foreach (TableSession ts in session)
        {
            List<TableGame> game = DatabaseManager.instance.DBConnection.Query<TableGame>("select name from TableGame where pk_game = ?", ts.fk_game);
            TablePatientsResult tpr = new TablePatientsResult();
            tpr.pk_game = ts.fk_game;
            tpr.game = game[0].name;
            tpr.score = ts.score;
            tableResult.Add(tpr);
            //print(tpr.game + " " + tpr.score);
        }
        return tableResult;
    }

    public List<TablePatientsDetails> GetDetailsOnPatient(int pk_patient, int pk_game)
    {
        //List<TableSession> s = DatabaseManager.instance.DBConnection.Query<TableSession>("select \"g.name\" as \"pk_session\", \"MAX(s.score)\" as \"pk_game\" from TableSession s, TableGame g where s.fk_game = g.pk_game AND s.fk_patient = ? GROUP BY g.name", pk_patient);
        List<TableSession> session = DatabaseManager.instance.DBConnection.Query<TableSession>("select pk_session,fk_configuration, dateSession, score ,speedMin, speedMax, heatmap from TableSession where fk_patient = ? AND fk_game = ? ORDER BY dateSession desc", pk_patient, pk_game);
        List<TablePatientsDetails> tableDetails = new List<TablePatientsDetails>();
        foreach (TableSession ts in session)
        {
            List<TableConfiguration> config = DatabaseManager.instance.DBConnection.Query<TableConfiguration>("select hand from TableConfiguration where pk_configuration = ?", ts.fk_configuration);
            TablePatientsDetails tpr = new TablePatientsDetails();
            tpr.pk_session = ts.pk_session;
            tpr.dateSession = ts.dateSession;
            tpr.hand = config[0].hand;
            tpr.score = ts.score;
            tpr.speedMin = ts.speedMax;
            tpr.heatmap = ts.heatMap;
            tableDetails.Add(tpr);
        }
        return tableDetails;
    }

    // Update is called once per frame
    void Update()
    {

    }
}

//Aux table to store the results for a patient
public class TablePatientsResult
{
    public int pk_game { get; set; }
    public string game { get; set; }
    public int score { get; set; }
}

//Aux table to store the details for a patient
public class TablePatientsDetails
{
    public int pk_session { get; set; }
    public string dateSession { get; set; }
    public int hand { get; set; }
    public int score { get; set; }
    public float speedMin { get; set; }
    public float speedMax { get; set; }
    public string heatmap { get; set; }
}
