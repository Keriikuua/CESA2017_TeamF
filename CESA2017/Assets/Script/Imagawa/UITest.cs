using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class UITest : MonoBehaviour {
    [Header("耐久値用")]
    public GameObject Life_1;
    public GameObject Life_2;
    public GameObject Life_3;
    public GameObject Life_4;
    [Header("敵のスタック")]
    public GameObject EnemyStuck1;
    public GameObject EnemyStuck2;
    public GameObject EnemyStuck3;
    public GameObject EnemyStuck4;
    [Header("自キャラのスタック")]
    public GameObject PlayerStuck1;
    public GameObject PlayerStuck2;
    public GameObject PlayerStuck3;
    public GameObject PlayerStuck4;

    [Header("Turnの円")]
    public Image TurnTimeImage;

    public GameObject[] TimeObj = new GameObject[8];

    GameObject GarbageObj;
    TurnSystem turnsystem;



    Sprite[] EnemySprite;                       //スタック用テクスチャ
    Sprite[] PlayerSprite;                        //プレイヤーのスタック
    Sprite BuckUpSprite_1;
    Sprite BuckUpSprite_2;

    Vector3 PlayerStuck1Pos;
    Vector3 PlayerStuck2Pos;
    Vector3 PlayerStuck3Pos;
    Vector3 PlayerStuck4Pos;

    Vector3 GarbagePos;

    int nEnemyStuckNum = 0;
    int nPlayerStuckNum = 0;
    int nNum = 0;

    float nTimeNum;
    float nNowTime;
    float rate;
    float Initrate;
    int Timenum;
    bool TimeFlg;
    bool GarbegFlg;
   

    private void Start()
    {
        EnemySprite = new Sprite[3];
        PlayerSprite = new Sprite[3];
        EnemySprite = Resources.LoadAll<Sprite>("Enemy/EnemyStuck/");
        PlayerSprite = Resources.LoadAll<Sprite>("Player/PlayerStuck/");
        turnsystem = GameObject.Find("TurnObj").GetComponent<TurnSystem>();
        GarbageObj = transform.FindChild("Garbage").gameObject;

        nTimeNum = 0.0f;
        nNowTime = 0;
        rate = 0.0f;
        Timenum = 0;
        GarbegFlg = false;
    }

    private void Update()
    {
        if(TimeFlg == true)
        {
            nNowTime += Time.deltaTime;
            if (rate <= nNowTime)
            {
                rate += Initrate;
                TimeObj[Timenum].SetActive(false);
                Timenum++;
            }
            if (nTimeNum <= nNowTime)
            {            
                TimeFlg = false;
            }
        }

        if (GarbegFlg)
        {
            if (PlayerStuck1.transform.position.x >= GarbagePos.x)
            {
                PlayerStuck1.transform.position -= new Vector3(3, 0, 0);
            }else if(PlayerStuck1.transform.localScale.x >= 0)
            {
                PlayerStuck1.transform.localScale -= new Vector3(Time.deltaTime, Time.deltaTime, 0);
            }

            if (PlayerStuck2.transform.position.x >= GarbagePos.x)
            {
                PlayerStuck2.transform.position -= new Vector3(3, 0, 0);
            }else if(PlayerStuck2.transform.localScale.x >= 0){
                PlayerStuck2.transform.localScale -= new Vector3(Time.deltaTime, Time.deltaTime, 0);
            }

            if (PlayerStuck3.transform.position.x >= GarbagePos.x)
            {
                PlayerStuck3.transform.position -= new Vector3(3, 0, 0);
            }else if (PlayerStuck3.transform.localScale.x >= 0){
                PlayerStuck3.transform.localScale -= new Vector3(Time.deltaTime, Time.deltaTime, 0);
            }

            if (PlayerStuck4.transform.position.x >= GarbagePos.x)
            {
                PlayerStuck4.transform.position -= new Vector3(3, 0, 0);
            }else if (PlayerStuck4.transform.localScale.x >= 0){
                PlayerStuck4.transform.localScale -= new Vector3(Time.deltaTime, Time.deltaTime, 0);
                

            }else
            {
                PlayerStuck1.SetActive(false);
                PlayerStuck2.SetActive(false);
                PlayerStuck3.SetActive(false);
                PlayerStuck4.SetActive(false);

                PlayerStuck1.transform.position = PlayerStuck1Pos;
                PlayerStuck2.transform.position = PlayerStuck2Pos;
                PlayerStuck3.transform.position = PlayerStuck3Pos;
                PlayerStuck4.transform.position = PlayerStuck4Pos;

                PlayerStuck1.transform.localScale = new Vector3(1, 1, 1);
                PlayerStuck2.transform.localScale = new Vector3(1, 1, 1);
                PlayerStuck3.transform.localScale = new Vector3(1, 1, 1);
                PlayerStuck4.transform.localScale = new Vector3(1, 1, 1);

                GarbageObj.GetComponent<GarbageScript>().OpenReset();
                nPlayerStuckNum = 0;
                GarbegFlg = false;
            }
        }
        
    }

    public void Life()
    {
        switch (nNum)
        {
            case 0:
                Life_1.SetActive(false);
                break;

            case 1:
                Life_2.SetActive(false);
                break;

            case 2:
                Life_3.SetActive(false);
                break;

            case 3:
                Life_4.SetActive(false);
                break;
        }
        nNum++;
    }

    public void EnemyStuck(int num)
    {
        switch (nEnemyStuckNum)
        {
            case 0:
                EnemyStuck1.SetActive(true);
                EnemyStuck1.GetComponent<Image>().sprite = EnemySprite[num];
                nEnemyStuckNum++;
                break;

            case 1:
                EnemyStuck2.SetActive(true);
                EnemyStuck2.GetComponent<Image>().sprite = EnemySprite[num];
                nEnemyStuckNum++;
                break;

            case 2:
                EnemyStuck3.SetActive(true);
                EnemyStuck3.GetComponent<Image>().sprite = EnemySprite[num];
                nEnemyStuckNum++;
                break;

            case 3:
                EnemyStuck4.SetActive(true);
                EnemyStuck4.GetComponent<Image>().sprite = EnemySprite[num];
                nEnemyStuckNum++;
                break;
        }

    }
    //敵出撃
    public void EnemySortie()
    {
        nEnemyStuckNum--;

        //nPlayerStuckNum = 0;
        if (nEnemyStuckNum <= 0)
        {
            nEnemyStuckNum = 0;
        }
        else if (nEnemyStuckNum >= 3)
        {
            nEnemyStuckNum = 3;
        }

        switch (nEnemyStuckNum)
        {
            case 0:
                //ayerStuck1.GetComponent<Image>().sprite = null;
                EnemyStuck1.SetActive(false);
                break;

            case 1:
                EnemyStuck1.GetComponent<Image>().sprite = EnemyStuck2.GetComponent<Image>().sprite;
                //layerStuck2.GetComponent<Image>().sprite = null;
                EnemyStuck2.SetActive(false);
                break;

            case 2:
                EnemyStuck1.GetComponent<Image>().sprite = EnemyStuck2.GetComponent<Image>().sprite;
                EnemyStuck2.GetComponent<Image>().sprite = EnemyStuck3.GetComponent<Image>().sprite;
                //layerStuck3.GetComponent<Image>().sprite = null;
                EnemyStuck3.SetActive(false);
                break;

            case 3:
                EnemyStuck1.GetComponent<Image>().sprite = EnemyStuck2.GetComponent<Image>().sprite;
                EnemyStuck2.GetComponent<Image>().sprite = EnemyStuck3.GetComponent<Image>().sprite;
                EnemyStuck3.GetComponent<Image>().sprite = EnemyStuck4.GetComponent<Image>().sprite;
                //ayerStuck4.GetComponent<Image>().sprite = null;
                EnemyStuck4.SetActive(false);
                break;
        }
    }

    public void PlayerStuck(int num)
    {
        switch (nPlayerStuckNum)
        {
            case 0:
                PlayerStuck1.SetActive(true);
                PlayerStuck1.GetComponent<Image>().sprite = PlayerSprite[num];
                nPlayerStuckNum++;
                break;

            case 1:
                PlayerStuck2.SetActive(true);
                PlayerStuck2.GetComponent<Image>().sprite = PlayerSprite[num];
                nPlayerStuckNum++;
                break;

            case 2:
                PlayerStuck3.SetActive(true);
                PlayerStuck3.GetComponent<Image>().sprite = PlayerSprite[num];
                nPlayerStuckNum++;
                break;

            case 3:
                PlayerStuck4.SetActive(true);
                PlayerStuck4.GetComponent<Image>().sprite = PlayerSprite[num];
                nPlayerStuckNum++;
                break;
        }
    }

    public  void PlayerSortie()
    {
        nPlayerStuckNum--;

        //nPlayerStuckNum = 0;
        if (nPlayerStuckNum <= 0)
        {
            nPlayerStuckNum = 0;
        }
        else if (nPlayerStuckNum >= 3)
        {
            nPlayerStuckNum = 3;
        }

        switch (nPlayerStuckNum)
        {
            case 0:
                //ayerStuck1.GetComponent<Image>().sprite = null;
                PlayerStuck1.SetActive(false);
                break;

            case 1:
                PlayerStuck1.GetComponent<Image>().sprite = PlayerStuck2.GetComponent<Image>().sprite;
               //layerStuck2.GetComponent<Image>().sprite = null;
                PlayerStuck2.SetActive(false);
                break;

            case 2:
                PlayerStuck1.GetComponent<Image>().sprite = PlayerStuck2.GetComponent<Image>().sprite;
                PlayerStuck2.GetComponent<Image>().sprite = PlayerStuck3.GetComponent<Image>().sprite;
               //layerStuck3.GetComponent<Image>().sprite = null;
                PlayerStuck3.SetActive(false);
                break;

            case 3:
                PlayerStuck1.GetComponent<Image>().sprite = PlayerStuck2.GetComponent<Image>().sprite;
                PlayerStuck2.GetComponent<Image>().sprite = PlayerStuck3.GetComponent<Image>().sprite;
                PlayerStuck3.GetComponent<Image>().sprite = PlayerStuck4.GetComponent<Image>().sprite;
                //ayerStuck4.GetComponent<Image>().sprite = null;
                PlayerStuck4.SetActive(false);
                break;
        }
    }

    public void PlayerStuckGarbage(Vector3 pos)
    {
        GarbagePos = pos;
        PlayerStuck1Pos = PlayerStuck1.transform.position;
        PlayerStuck2Pos = PlayerStuck2.transform.position;
        PlayerStuck3Pos = PlayerStuck3.transform.position;
        PlayerStuck4Pos = PlayerStuck4.transform.position;
        turnsystem.DeleteStuck();
        GarbegFlg = true;

    }//プレイヤーをゴミ箱へ

    public void TimeUIActive()
    {
        for (int i = 0;i < TimeObj.Length;i++)
        {
            TimeObj[i].SetActive(true);
        }
        nTimeNum = 0.0f;
        Timenum = 0;
        nNowTime = 0.0f;
    }

    public void TurnTime(float num)
    {
        if (!TimeFlg && nTimeNum <= 0)
        {
            nTimeNum = num;
            TimeFlg = true;
            Initrate = nTimeNum / 8;
            rate = Initrate;
        }
        else
        {
            TimeFlg = true;
        }
        //if (TimeFlg)
        //{
        //    TimeFlg = false;
        //}
    }

}
