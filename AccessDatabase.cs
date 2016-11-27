using UnityEngine;
//using MySql.Data;
using MySql.Data.MySqlClient;
using System;
using System.Data;
using System.Collections;
using System.Collections.Generic;

// SQL:
// select U_Level, U_Name, U_Password from User;
// select ArtW_ID, Fig_Name from Artwork where ArtW_level = 1;   ---> for VIP user only
// select ArtW_ID, Fig_Name from Artwork where ArtW_level = 1 OR ArtW_level = 2;   ---> for all of the users

public class AccessDatabase : MonoBehaviour {

    #region Variables
    // Static Variables
    // check the attribute's name

    float posx, posy, posz;   // painting location
    int UID;

    // Private Variables
    // private bool loading = false;

    internal MySqlConnection connect;
    // command object
    private MySqlCommand cmd = null;
    // reader object
    private MySqlDataReader datareader = null;
    // object collection array
    private GameObject[] bodies;
    // private Dictionary<string, string> loginPair = new Dictionary<string, string>();

    // object definitions
    internal struct userData
    {
        internal int uLevel;
        internal string uName;
        internal string uPassword;
    }
    internal List<userData> loginInfo;

    internal List<string> figName;

    //public struct PosData
    //{
    //public int U_ID;
    //public float pos_x, pos_y, pos_z;
    //}
    //internal List<PosData> ClonePos;
    #endregion

    void Awake()
    {
        print("AccessData Awake() ^_^");
    }

	// Update is called once per frame
	void Update() {
	
	}

    internal void buildConnect(string constr)
    {
        try
        {
            print("AccessData buildConnect() ^_^");

            connect = new MySqlConnection(constr);
            if (connect == null)
            {
                print("Error connect load!");
            }
            
            connect.Open();
            Debug.Log("Connection State: " + connect.State);
        }
        catch (Exception ex)
        {
            Debug.Log(ex.ToString());
        }
    }

    // Read all entries from user table
    internal void ReadUserInfo()
    {
        print("Excuted AccessData ReadUserInfo() ^_^");

        string query = string.Empty;
        if (loginInfo == null)
            loginInfo = new List<userData>();
        if (loginInfo.Count > 0)
            loginInfo.Clear();

        try
        {
            query = "select `U_Level`, `U_Name`, `U_Password` from `VR`.`User`";
            if (connect.State.ToString() != "Open")
                connect.Open();
            using (connect)
            {
                using (cmd = new MySqlCommand(query, connect))
                {
                    datareader = cmd.ExecuteReader();
                    if (datareader.HasRows)
                        while (datareader.Read())
                        {
                            userData uInfo = new userData();

                            //database always returns string
                            uInfo.uLevel = int.Parse(datareader["U_Level"].ToString());  //string --> int
                            uInfo.uName = datareader["U_Name"].ToString();
                            uInfo.uPassword = datareader["U_Password"].ToString();

                            loginInfo.Add(uInfo);
                        }
                    datareader.Dispose();
                }
            }
        }
        catch (Exception ex)
        {
            Debug.Log(ex.ToString());
        }
    }

    // Update pic location of pictures in the table based on the iddemo_table

    internal void UpdateEntries(int UID, float posx, float posy, float posz) 
    {
        //prepPos();
        string query = string.Empty;
        try
        {
            query = "UPDATE `VR`.`Position` SET `X_p`=?posx, `Y_p`=?posy, `Z_p`=?posz WHERE `Artwork_ArtW_ID`=?UID";
            if (connect.State.ToString() != "Open")
                connect.Open();
            using (connect)
            {
                using (cmd = new MySqlCommand(query, connect))
                {
                    MySqlParameter oParamx = cmd.Parameters.Add("?posx", MySqlDbType.Float);
                    oParamx.Value = posx;
                    MySqlParameter oParamy = cmd.Parameters.Add("?posy", MySqlDbType.Float);
                    oParamy.Value = posy;
                    MySqlParameter oParamz = cmd.Parameters.Add("?posz", MySqlDbType.Float);
                    oParamz.Value = posz;
                    MySqlParameter oParamID = cmd.Parameters.Add("?UID", MySqlDbType.Int32);
                    oParamID.Value = UID;

                    cmd.ExecuteNonQuery();
                }
            }
        }
        catch (Exception ex)
        {
            Debug.Log(ex.ToString());
        }
        finally
        {
        }
    }

    // read VIP figure info from database
    internal void Read1FigInfo()
    {
        print("Excuted AccessData Read1FigInfo() ^_^");

        string query = string.Empty;
        if (figName == null)
            figName = new List<string>();
        if (figName.Count > 0)
            figName.Clear();

        try
        {
            query = "select `ArtW_ID`, `Fig_Name` from `VR`.`Artwork` where `ArtW_level` = 1";
            if (connect.State.ToString() != "Open")
                connect.Open();
            using (connect)
            {
                using (cmd = new MySqlCommand(query, connect))
                {
                    datareader = cmd.ExecuteReader();
                    if (datareader.HasRows)
                        while (datareader.Read())
                        {
                            string loc;
                            //artWInfo.arwID = int.Parse(datareader["ArtW_ID"].ToString());
                            loc = datareader["Fig_Name"].ToString();

                            figName.Add(loc);
							print(loc);
                        }
                    datareader.Dispose();
                }
            }
			if (figName == null){
				print("Error: get figName failed");
			}
        }
        catch (Exception ex)
        {
            Debug.Log(ex.ToString());
        }
        finally
        {
        }
    }

    // read all users' figure info from database
    internal void Read2FigInfo()
    {
        print("Excuted AccessData Read2FigInfo() ^_^");

        string query = string.Empty;
        if (figName == null)
            figName = new List<string>();
        if (figName.Count > 0)
            figName.Clear();

        try
        {
            query = "select `ArtW_ID`, `Fig_Name` from `VR`.`Artwork` where `ArtW_level` = 1 OR `ArtW_level` = 2";
            if (connect.State.ToString() != "Open")
                connect.Open();
            using (connect)
            {
                using (cmd = new MySqlCommand(query, connect))
                {
                    datareader = cmd.ExecuteReader();
                    if (datareader.HasRows)
                        while (datareader.Read())
                        {
                            string loc;
                            loc = datareader["Fig_Name"].ToString();

                            figName.Add(loc);
                        }
                    datareader.Dispose();
                }
            }   
        }
        catch (Exception ex)
        {
            Debug.Log(ex.ToString());
        }
        finally
        {
        }
    }

    // close connection to database
    void OnApplicationQuit()
    {
        Debug.Log("killing con");
        if (connect != null)
        {
            if (connect.State.ToString() != "Closed")
                connect.Close();
            connect.Dispose();
        }
    }
}
