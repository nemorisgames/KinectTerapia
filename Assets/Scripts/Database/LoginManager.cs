using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoginManager : MonoBehaviour {
    public UILabel errorMessage;
	// Use this for initialization
	void Start () {
        PlayerPrefs.DeleteAll();
	}

    public void LoginButton(string username, string password)
    {
        print(username + " " + password);
        int pk_kinesiologistAux = (DatabaseManager.instance.Login(username, password));
        if(pk_kinesiologistAux < 0)
        {
            errorMessage.text = "El usuario no existe o la contraseña no corresponde";
        }
        else
        {
            PlayerPrefs.SetInt("pk_kinesiologist", pk_kinesiologistAux);
            SceneManager.LoadScene("Kinesiologist");
        }
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
