using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GarbageScript : MonoBehaviour {

    GameObject TheBottomObj;
    GameObject RightLidObj;
    GameObject LeftLidObj;
    GameObject GarbegeButton;

    UITest uitest;

    bool OpenFlg;
    bool StuckFlw;
    float OpenTime;

    bool CloseFlg;


    private void Start()
    {
        TheBottomObj = transform.FindChild("Thebottom").gameObject;
        RightLidObj = transform.FindChild("RightLid").gameObject;
        LeftLidObj = transform.FindChild("LeftLid").gameObject;
        GarbegeButton = transform.FindChild("GarbageButton").gameObject;
        uitest = GameObject.Find("UI").GetComponent<UITest>();

        OpenFlg = false;
        StuckFlw = false;
        CloseFlg = false;
        OpenTime = 0.0f;
    }

    private void Update()
    {
        if (OpenFlg)
        {
            OpenTime += Time.deltaTime;
            if (OpenTime <= 1.0f)
            {
                RightLidObj.transform.localScale -= new Vector3(Time.deltaTime, 0, 0);
                LeftLidObj.transform.localScale -= new Vector3(Time.deltaTime, 0, 0);
            }else
            {
                RightLidObj.transform.localScale = new Vector3(0, 1, 1);
                LeftLidObj.transform.localScale = new Vector3(0, 1, 1);
                if (!StuckFlw)
                {
                    uitest.PlayerStuckGarbage(TheBottomObj.transform.position);
                    StuckFlw = true;
                }
            }
        }else if (CloseFlg){
            OpenTime += Time.deltaTime;
            if (OpenTime <= 1.0f)
            {
                RightLidObj.transform.localScale += new Vector3(Time.deltaTime, 0, 0);
                LeftLidObj.transform.localScale += new Vector3(Time.deltaTime, 0, 0);
            }
            else
            {
                RightLidObj.transform.localScale = new Vector3(1, 1, 1);
                LeftLidObj.transform.localScale = new Vector3(1, 1, 1);
                CloseFlg = false;
                StuckFlw = false;
                OpenTime = 0.0f;
                GarbegeButton.GetComponent<Image>().enabled = true;
            }
        }
    }

    public void Open()
    {
        if (!OpenFlg && !CloseFlg)
        {
            OpenFlg = true;
            GarbegeButton.GetComponent<Image>().enabled = false;
        }
        else if (OpenTime >= 1.0f)
        {
            OpenReset();
        }
    }

    public void OpenReset()
    {
        //RightLidObj.transform.localScale = new Vector3(0, 1, 1);
        //LeftLidObj.transform.localScale = new Vector3(0, 1, 1);
        OpenFlg = false;
        CloseFlg = true;
        OpenTime = 0.0f;
    }


}
