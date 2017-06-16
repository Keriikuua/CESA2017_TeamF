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


    Sprite[] EnemySprite;                       //スタック用テクスチャ
    Sprite[] PlayerSprite;                        //プレイヤーのスタック
    Sprite BuckUpSprite_1;
    Sprite BuckUpSprite_2;

    int nEnemyStuckNum = 0;
    int nPlayerStuckNum = 0;
    int nNum = 0;

    float nTimeNum;
    bool TimeFlg;

    private void Start()
    {
        EnemySprite = new Sprite[3];
        PlayerSprite = new Sprite[3];
        EnemySprite = Resources.LoadAll<Sprite>("Enemy/EnemyStuck/");
        PlayerSprite = Resources.LoadAll<Sprite>("Player/PlayerStuck/");
    }

    private void Update()
    {
        if(TimeFlg == true)
        {
            TurnTimeImage.fillAmount -= 1.0f / nTimeNum * Time.deltaTime;
            if (TurnTimeImage.fillAmount <= 0)
            {            
                TimeFlg = false;
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
        if (EnemyStuck2.activeSelf == true)
        {
            EnemyStuck1.GetComponent<Image>().sprite = EnemyStuck2.GetComponent<Image>().sprite;
        }
        else
        {
            EnemyStuck1.SetActive(false);
        }

        if (EnemyStuck3.activeSelf == true)
            EnemyStuck2.GetComponent<Image>().sprite = EnemyStuck3.GetComponent<Image>().sprite;
        else
            EnemyStuck2.SetActive(false);

        if (EnemyStuck4.activeSelf == true)
        {
            EnemyStuck3.GetComponent<Image>().sprite = EnemyStuck4.GetComponent<Image>().sprite;
            EnemyStuck4.SetActive(false);
        }
        else
            EnemyStuck3.SetActive(false);

        nEnemyStuckNum--;

        if (nEnemyStuckNum <= 0)
        {
            nEnemyStuckNum = 0;
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

        Debug.Log(nPlayerStuckNum);
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
        //if (PlayerStuck2.activeSelf == true)
        //{
        //    PlayerStuck1.GetComponent<Image>().sprite = PlayerStuck2.GetComponent<Image>().sprite;
        //    if (PlayerStuck3.activeSelf == false)
        //    {
        //        PlayerStuck2.SetActive(false);
        //    }
        //}
        //else
        //{
        //    PlayerStuck1.SetActive(false);
        //}

        //if (PlayerStuck3.activeSelf == true)
        //{
        //    PlayerStuck2.GetComponent<Image>().sprite = PlayerStuck3.GetComponent<Image>().sprite;
        //    if (PlayerStuck4.activeSelf == false)
        //    {
        //        PlayerStuck3.SetActive(false);
        //    }
        //}

        //if (PlayerStuck4.activeSelf == true)
        //{
        //    PlayerStuck3.GetComponent<Image>().sprite = PlayerStuck4.GetComponent<Image>().sprite;
        //    PlayerStuck4.SetActive(false);
        //}
    }

    public void TurnTime(float num)
    {
        nTimeNum = num;
        TurnTimeImage.fillAmount = 1.0f;
        TimeFlg = true;
    }

}
