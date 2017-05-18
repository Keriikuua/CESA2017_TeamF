using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour {

    //キャラステータス
    int nHP;
    int nAttack;
    int nWeakness;
    int nCharaNum;
    bool bCharaSwitch;

    
    Vector3 MyStartCharaPos;                //自キャラの初期位置
    Vector3 EnemyStartPos;                  //敵キャラの出現位置
    //自キャラが向かう左右の座標
    Vector3[] MyHeadforCharaPos = new Vector3[2];
    //左右の判断用
    int nJud;

    CharaData charadata;
    CharaList charalist;
    public GameObject FieldManeObj;
    FieldManager fieldmane;
    PlayerScr playerscr;

    //最初にゲームオブジェクトとしてキャラを取得するために宣言
    GameObject MyCharaTest;
    GameObject Enemy1Obj;

    
    void Start(){
        //初期化関係
        nHP = 0;
        nAttack = 0;
        nCharaNum = 0;
        nWeakness = 0;
        bCharaSwitch = false;
        MyStartCharaPos = Vector3.zero;

        MyHeadforCharaPos[0] = Vector3.zero;
        nJud = 0;

        charadata = GetComponent<CharaData>();
        charalist = GetComponent<CharaList>();
        fieldmane = FieldManeObj.GetComponent<FieldManager>();

        //ここでキャラを取得してくる
        Acquisition();

        //自キャラの出現位置を格納
        MyStartCharaPos = fieldmane.SetMyCharaPos();
        MyHeadforCharaPos[0] = fieldmane.SetMyRightCharaPos();
        MyHeadforCharaPos[1] = fieldmane.SetMyLeftCharaPos();
        //敵キャラのスタート位置を格納
        EnemyStartPos = fieldmane.SetEnemyStartPos();
    }

    void Update(){
        if (Input.GetKeyDown(KeyCode.P)){
            GetData(nCharaNum);
            CharaForm();
            Debug.Log(nHP);
            Debug.Log(nAttack);
            Debug.Log(nWeakness);
            nCharaNum++;

            if(nCharaNum >= 2){
                nCharaNum = 0;
            }
        }

    }


    //データ引き出し
    void GetData(int Num){
        nHP = charadata.HPReturn(Num);
        nAttack = charadata.AttackReturn(Num);
        nWeakness = charadata.WeakReturn(Num);
        
    }

    //キャラ生成
    void CharaForm(){
        GameObject Obj;
        switch (nCharaNum){    
            case 0:
                Obj =  Instantiate(MyCharaTest, MyStartCharaPos, Quaternion.identity)as GameObject;
                playerscr = Obj.GetComponent<PlayerScr>();
                charalist.PlayerCharaPut(Obj);             
                playerscr.PlayerSwitch(MyHeadforCharaPos[nJud],nHP,nAttack,nWeakness);
                
                nJud++;
            break;
            case 1:
                Obj = Instantiate(MyCharaTest, MyStartCharaPos, Quaternion.identity) as GameObject;
                playerscr = Obj.GetComponent<PlayerScr>();
                playerscr.PlayerSwitch(MyHeadforCharaPos[nJud], nHP, nAttack, nWeakness);
                charalist.PlayerCharaPut(Obj);
                nJud++;
            break;
        }

        if(nJud == 2){
            nJud = 0;
        }
    }
    //敵生成
    public GameObject EnemyForm(int num){
        GameObject obj;
        switch (num){
            case 0:
                obj = Instantiate(Enemy1Obj, EnemyStartPos, Quaternion.identity) as GameObject;
                charalist.EnemyCharaPut(obj);
                GetData(num);
                obj.GetComponent<EnemyScr>().Status(nHP, nAttack, nWeakness, MyStartCharaPos);
                return obj;
        }
        return null;
    }


    //Resourcesフォルダーからデータ取得
    void Acquisition(){
        MyCharaTest = (GameObject)Resources.Load("MyChara/MyTestChara");

        Enemy1Obj = (GameObject)Resources.Load("Enemy/Enemy1");
    }


    public void DebugChara(){
        GetData(nCharaNum);
        CharaForm();
        Debug.Log(nHP);
        Debug.Log(nAttack);
        Debug.Log(nWeakness);        

        nCharaNum++;

        if (nCharaNum >= 2)
        {
            nCharaNum = 0;
        }
    }
}
