using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chara : MonoBehaviour {

    //キャラステータス
    int nHP;
    int nAttack;
    int nWeakness;
    int nCharaNum;
    //自キャラの初期位置
    Vector3 MyCharaPos;


    CharaData charadata;
    CharaList charalist;
    FieldManager fieldmane;

    //最初にゲームオブジェクトとしてキャラを取得するために宣言
    GameObject MyCharaTest;


    
    void Start(){
        nHP = 0;
        nAttack = 0;
        nCharaNum = 0;
        nWeakness = 0;
        MyCharaPos = Vector3.zero;

        charadata = GetComponent<CharaData>();
        charalist = GetComponent<CharaList>();
        fieldmane = GameObject.Find("MapData").GetComponent<FieldManager>();

        //ここでキャラを取得してくる
        Acquisition();

        //自キャラの出現位置を格納
        MyCharaPos = fieldmane.SetMyCharaPos();
    }

    void Update(){
        if (Input.GetKeyDown(KeyCode.P)){
            GetData(nCharaNum);
            Debug.Log(nHP);
            Debug.Log(nAttack);
            Debug.Log(nWeakness);
            nCharaNum++;
        }
    }


    //データ引き出し
    void GetData(int Num){
        nHP = charadata.HPReturn(Num);
        nAttack = charadata.AttackReturn(Num);
        nWeakness = charadata.WeakReturn(Num);
        CharaForm();
    }

    //キャラ生成
    void CharaForm(){
        switch (nCharaNum){
            case 0:
                GameObject Obj =  Instantiate(MyCharaTest, MyCharaPos, Quaternion.identity)as GameObject;
                charalist.PlayerCharaPut(Obj);
            break;
        }
    }

    //Resourcesフォルダーからデータ取得
    void Acquisition(){
        MyCharaTest = (GameObject)Resources.Load("MyChara/MyTestChara");
    }

}
