using UnityEngine;
using MySql.Data.MySqlClient;
//using SqlConnection;
//using SqlConnection.State;
using System;
using System.Data;
using System.Collections;
using System.Collections.Generic;


public class LoginGUI : MonoBehaviour {

    #region Variables
    //Static Variables

    private string inputUName = "";
    private string inputUPassword = "";
    private string inputServer = "";
    private string inputUID = "";
    private string inputPWD = "";
    private string inputDatabase = "";
    private string CurrentManu = "";
    private bool getConInfo = true;
    private bool tryConnect = false;
    private bool compareinput = false;
    public Texture2D CursorTex;  // cursor texture
    public int cursorSizeX = 16;  // cursor size x
    public int cursorSizeY = 16;  // cursor size y
    public AccessDatabase LoginData;

    //private List<int> uLevelx;
    //private List<string> uNamex;
    //private List<string> uPasswordx;

    #endregion

    void Awake()
    {
        //constate = "";
        print("LoginGUI Awake() ^_^");       
    }

    void Start()
    {
        Cursor.visible = false;
    }

    void OnGUI()
    {
        print("ONGUI!!!!!");
        print("compareinput" + compareinput);

        //mouse
        GUI.DrawTexture(new Rect(Event.current.mousePosition.x - cursorSizeX / 2, Event.current.mousePosition.y - cursorSizeY / 2, cursorSizeX, cursorSizeY), CursorTex);
        
        if (getConInfo == true)
        {
            GUI.Window(0, new Rect(Screen.width / 2 - 100, Screen.height / 2 - 85, 200, 150), connectWindowFunc, "Connect Infomation");            
        }

        //connect to mysql
        if (tryConnect == true)
        {
            print("LoginData.constr = " + PlayerPrefs.GetString("constr"));
            LoginData.buildConnect(PlayerPrefs.GetString("constr"));
            if (LoginData.connect.State.Equals(ConnectionState.Open))
            {
                print("LoginData.connect.State.Equals(ConnectionState.Open)= " + LoginData.connect.State.Equals(ConnectionState.Open));
                LoginData.ReadUserInfo();
                /*for (int i = 0; i < LoginData.loginInfo.Count; i++)
                {
                    print("LoginData.connect.State.Equals(ConnectionState.Open)= " + LoginData.connect.State.Equals(ConnectionState.Open));
                    print(LoginData.loginInfo[i].uLevel);
                    uLevelx.Add(LoginData.loginInfo[i].uLevel);
                    uNamex.Add(LoginData.loginInfo[i].uName);
                    uPasswordx.Add(LoginData.loginInfo[i].uPassword);
                }*/

                print("the first userinfo: " + LoginData.loginInfo[0].uName);
                print("the first userinfo: " + LoginData.loginInfo[0].uPassword);
                
                compareinput = true;
                tryConnect = false;
            }
            else
            {
                getConInfo = true;
                tryConnect = false;
            }
        }
        //print(LoginData.connect.State);
            
        if (compareinput)
        {
            //Repaint();
            //print("getConInfo" + getConInfo);
            //print("tryConnect" + tryConnect);
            //print("go into login window");
            //print(LoginData.loginInfo.Count);
            try
            {
                GUI.Window(0, new Rect(Screen.width / 2 - 100, Screen.height / 2 - 55, 200, 110), loginWindowFunc, "Login Window");
                //GUI.Window(0, new Rect(Screen.width / 2 - 100, Screen.height / 2 - 85, 200, 110), loginWindowFunc, "Connect Successful! Login Please");
            }
            catch (Exception ex)
            {
                Debug.Log(ex.ToString());
            }

            if (CurrentManu == "InRoom")
            {
                Application.LoadLevel("InRoomScene");
            }
            else if (CurrentManu == "LoginFail")
            {
                LoginFailGUI();
            }
        }            
        else
        {
            getConInfo = true;
            tryConnect = false;
            return;
        }            
    }


    void connectWindowFunc(int windowID)
    {
        GUI.Label(new Rect(10, 30, 60, 20), "server");
        inputServer = GUI.TextField(new Rect(80, 30, 100, 20), inputServer);
        GUI.Label(new Rect(10, 50, 60, 20), "uid");
        inputUID = GUI.TextField(new Rect(80, 50, 100, 20), inputUID);
        GUI.Label(new Rect(10, 70, 60, 20), "pwd");
        inputPWD = GUI.TextField(new Rect(80, 70, 100, 20), inputPWD);
        GUI.Label(new Rect(10, 90, 60, 20), "database");
        inputDatabase = GUI.TextField(new Rect(80, 90, 100, 20), inputDatabase);
        PlayerPrefs.SetString("constr", "server =" + inputServer + ";uid=" + inputUID + ";pwd=" + inputPWD + ";database=" + inputDatabase + ";port=3306;Pooling=true");

        if (GUI.Button(new Rect(10, 120, 120, 20), "Connect Database"))
        {
            print("Print button Connect Database");
            tryConnect = true;
            getConInfo = false;
            return;
        }
    }

    void loginWindowFunc(int windowID)
    {
        print("loginWindowFunc!^_^"); 

        GUI.Label(new Rect(10, 30, 60, 20), "ID");
        inputUName = GUI.TextField(new Rect(80, 30, 100, 20), inputUName);
        GUI.Label(new Rect(10, 50, 60, 20), "Password");
        inputUPassword = GUI.PasswordField(new Rect(80, 50, 100, 20), inputUPassword, "*"[0]);

        if (GUI.Button(new Rect(70, 80, 50, 20), "Login"))
        {
            print("Press login button! ^-^");
            //Check the ID and password pair
			for (int i = 0; i < LoginData.loginInfo.Count; i++)
            {
                print("uName.Count: " + LoginData.loginInfo.Count);
				if(inputUName == LoginData.loginInfo[i].uName && inputUPassword == LoginData.loginInfo[i].uPassword)
                {
                    print("inputUName: " + inputUName);
                    print("inputUPassword: " + inputUPassword);
                    PlayerPrefs.SetInt("UserLevel", LoginData.loginInfo[i].uLevel);
				    CurrentManu = "InRoom";
                    break;
                }
			    else
                {
                    print("inputUName: " + inputUName);
                    print("inputUPassword: " + inputUPassword);
                    CurrentManu = "LoginFail";
                }
                
			}
        }
        GUI.DragWindow(new Rect(0, 0, 10000, 10000));
    }


    void LoginFailGUI()
    {
        GUI.Window(1, new Rect(Screen.width / 2 - 100, Screen.height / 2 - 55, 200, 110), loginWindowFunc, "Login fail, try again, please");
    }
}
