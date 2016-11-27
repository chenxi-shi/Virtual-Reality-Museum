using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
//using UnityEngine.SceneManagement;

public class ReadFig : MonoBehaviour {

    //delete figNameArr after connection to database
    //private List<string> figNameArr;
    public Transform Fig;
    private float a;  //unit angle
    private Quaternion q;
    private List<Transform> figClone = new List<Transform>();
    public AccessDatabase AccessData;
    private int UserLevel;
    private int U_ID;
    private float pos_x, pos_y, pos_z;


    void Awake()
    {
        print("ReadFig AWAKE() ^_^");
        AccessData.buildConnect(PlayerPrefs.GetString("constr"));
        AccessData.ReadUserInfo();
    }

    // initialization
    void Start ()
    {
        print("ReadFig START() ^_^");
		LoginFig();
    }
	

	void LoginFig(){
		for (int i = 0; i < AccessData.loginInfo.Count; i++){
            //AccessData.Read1FigInfo();
            //loadImage(figClone, AccessData.figName);

			if (PlayerPrefs.GetInt("UserLevel") == 1){
				AccessData.Read1FigInfo();					
			}
			else if (PlayerPrefs.GetInt("UserLevel") == 2){
                AccessData.Read2FigInfo();
			}
			loadImage(figClone, AccessData.figName);
            break;
		}           		
	}
	
    void loadImage(List<Transform> transfor, List<string> fig_name_lst)
    {
        // Design: let all of the figures show as a circle
        transform_lst = new List<Transform>();  // count of figures
        a = 360 / fig_name_lst.Count;  // unit angle 
        // Debug.Log("unit angle a=" + a);
        print("The num of Fig: " + fig_name_lst.Count);

        // create instances for figures and render them
        for (int i = 0; i < fig_name_lst.Count; i++)
        {
            transform_lst.Add(Instantiate(Fig));
            addTexture(fig_name_lst[i], transform_lst[i]);
        }

        for (int i = 0; i < fig_name_lst.Count; i++)
        {
            // locate the figure
            // caculate the Quaternion coordinate from vector3 coordinate system
            q = Quaternion.AngleAxis(i * a + 180F, Vector3.up);

            // quanternion --> vector3
            // print("Rotation axis" + Vector3.up);
            // print("Rotation2: " + q);

            // revolution * Quaternion * Vector3 = Vector3
            transform_lst[i].position = new Vector3(0, 1, 0) + Quaternion.AngleAxis((i+1) * a, Vector3.up) * Vector3.forward * 3;
            // rotation
            transform_lst[i].rotation = q;

            prepPos(transform_lst[i]);
            AccessData.UpdateEntries(i + 1, pos_x, pos_y, pos_z);
        }
    }

    void prepPos(Transform fCpos)
    {

        //bodies = GameObject.FindGameObjectsWithTag("Savable");
        //_GameItems = new List<data>();
        //data itm;
        //foreach (GameObject body in bodies)
        //{
            //itm = new data();
            //itm.ID = body.name + "_" + body.GetInstanceID();
            //itm.Name = body.name;
            //itm.levelname = Application.loadedLevelName;
            //itm.objectType = body.name.Replace("(Clone)", "");
        pos_x = fCpos.transform.position.x;
        pos_y = fCpos.transform.position.y;
        pos_z = fCpos.transform.position.z;
            //_GameItems.Add(itm);
        //}
        //Debug.Log("Items in collection: " + _GameItems.Count);
    }

    // render figure
    void addTexture(string fig_loc, Transform aTrfm)
    {
        print("addTexture 执行了! ^_^");

        // load figure as texture
        Texture t = Resources.Load(fig_loc) as Texture;

        float Ra = H_W_Ratio(t.width, t.height);
        //print(fig_loc + Ra);
        if (t == null)
            Debug.Log("Load Object Fail");

        aTrfm.localScale = new Vector3(1F, Ra * 2F, 1F);
        print(fig_loc);
        //print("Parent object local proportion: " + aTrfm.localScale);
        //print("Global zoom proportion: " + aTrfm.lossyScale);

        // render the texture
        aTrfm.GetComponent<Renderer>().material.mainTexture = t;
    }

    // read the hight and width of picture
    float H_W_Ratio(float w, float h)
    {
        //print("weight: " + w);
        //print("hight: " + h);
        return h / w;
    }
}