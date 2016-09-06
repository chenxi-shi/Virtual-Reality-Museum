using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
//using UnityEngine.SceneManagement;

public class ReadFig : MonoBehaviour {

    //delete figNameArr after connection to database part
    //private List<string> figNameArr;
    public Transform Fig;
    //单位角度 a
    private float a;
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

    // Use this for initialization
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
	
    void loadImage(List<Transform> fC, List<string> fN)
    {
        fC = new List<Transform>();
        //画的数量
        print("The num of Fig: " + fN.Count);
        //单位角度    
        a = 360 / fN.Count;
        //Debug.Log("角度 a=" + a);

        for (int i = 0; i < fN.Count; i++)
        {
            //为f加一个实例
            fC.Add(Instantiate(Fig));

            //为实例加载图画
            addTexture(fN[i], fC[i]);
        }

        for (int i = 0; i < fN.Count; i++)
        {
            //计算quanternion位置
            q = Quaternion.AngleAxis(i * a + 180F, Vector3.up);

            //为f的实例定位
            //将quanternion转化为vector3
            //print("旋转轴" + Vector3.up);
            //print("Rotation2: " + q);

            //公转 * Quaternion * Vector3 = Vector3
            fC[i].position = new Vector3(0, 1, 0) + Quaternion.AngleAxis((i+1) * a, Vector3.up) * Vector3.forward * 3;
            //自转
            fC[i].rotation = q;

            prepPos(fC[i]);
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

    //加载图画
    void addTexture(string loc, Transform aTrfm)
    {
        print("addTexture 执行了! ^_^");

        //从loc处取图片作为texture
        Texture t = Resources.Load(loc) as Texture;

        float Ra = H_W_Ratio(t.width, t.height);
        //print(loc + Ra);
        if (t == null)
            Debug.Log("Load Object Fail");

        aTrfm.localScale = new Vector3(1F, Ra * 2F, 1F);
        print(loc);
        //print("父对象局部比例: " + aTrfm.localScale);
        //print("全局缩放比例: " + aTrfm.lossyScale);

        //为一个f实例加texture
        aTrfm.GetComponent<Renderer>().material.mainTexture = t;
    }

    //读画的比例
    float H_W_Ratio(float w, float h)
    {
        //print("weight: " + w);
        //print("hight: " + h);
        return h / w;
    }
}