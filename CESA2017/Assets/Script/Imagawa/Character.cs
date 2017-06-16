using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour {

    //キャラステータス
    int nHP;
    int nAttack;
    //int nWeakness;
    int nCharaNum;
    Type.Chara m_Type;

    
    Vector3 MyStartCharaPos;                //自キャラの初期位置
    Vector3 EnemyStartPos;                  //敵キャラの出現位置

    CharaData charadata;
    CharaList charalist;
    UITest uitest;

    GameObject PlayerParentObj;
    GameObject EnemyParentObj;

    public GameObject FieldManeObj;
    FieldManager fieldmane;
    PlayerScr playerscr;

    //最初にゲームオブジェクトとしてキャラを取得するために宣言
    GameObject PlayerCharaObj1;
    GameObject PlayerCharaObj2;
    GameObject PlayerCharaObj3;
    GameObject Enemy1Obj;

    
    void Start(){
        //初期化関係
        nHP = 0;
        nAttack = 0;
        nCharaNum = 0;
        //nWeakness = 0;
        m_Type = Type.Chara.Nothing;
        MyStartCharaPos = Vector3.zero;
 

        charadata = GetComponent<CharaData>();
        charalist = GetComponent<CharaList>();
        fieldmane = FieldManeObj.GetComponent<FieldManager>();
        PlayerParentObj = this.transform.FindChild("PlayerParent").gameObject;
        EnemyParentObj = this.transform.FindChild("EnemyParent").gameObject;
        uitest = GameObject.Find("UI").GetComponent<UITest>();

        //ここでキャラを取得してくる
        Acquisition();

        MyStartCharaPos = fieldmane.SetMyCharaPos();
        //自キャラの出現位置を格納
        MyStartCharaPos = new Vector3(MyStartCharaPos.x, MyStartCharaPos.y, MyStartCharaPos.z);
        //敵キャラのスタート位置を格納
        EnemyStartPos = fieldmane.SetEnemyStartPos();
    }

    void Update(){
        if (Input.GetKeyDown(KeyCode.P)){
            GetData(nCharaNum);
            CharaForm(Type.Chara.A);
            Debug.Log(nHP);
            Debug.Log(nAttack);
            Debug.Log(m_Type);
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
        m_Type = charadata.TypeReturn(Num);
    }

    //キャラ生成
    void CharaForm(Type.Chara type){
        GameObject Obj;
        switch (nCharaNum)
        {
            case 0:
                switch (type) {
                    case Type.Chara.A:
                           Obj = Instantiate(PlayerCharaObj1, MyStartCharaPos, Quaternion.identity) as GameObject;
                           Obj.transform.parent = PlayerParentObj.transform;
                           playerscr = Obj.GetComponent<PlayerScr>();
                           playerscr.PlayerSwitch(nHP, nAttack, type);
                           charalist.PlayerCharaPut(Obj);
                        uitest.PlayerStuck(0);
                        break;
                    case Type.Chara.B:
                        Obj = Instantiate(PlayerCharaObj2, MyStartCharaPos, Quaternion.identity) as GameObject;
                        Obj.transform.parent = PlayerParentObj.transform;
                        playerscr = Obj.GetComponent<PlayerScr>();
                        playerscr.PlayerSwitch(nHP, nAttack, type);
                        charalist.PlayerCharaPut(Obj);
                        uitest.PlayerStuck(0);
                        break;
                    case Type.Chara.C:
                        Obj = Instantiate(PlayerCharaObj3, MyStartCharaPos, Quaternion.identity) as GameObject;
                        Obj.transform.parent = PlayerParentObj.transform;
                        playerscr = Obj.GetComponent<PlayerScr>();
                        playerscr.PlayerSwitch(nHP, nAttack, type);
                        charalist.PlayerCharaPut(Obj);
                        uitest.PlayerStuck(0);
                        break;
                }
            break;
            case 1:
                switch (type)
                {
                    case Type.Chara.A:
                        Obj = Instantiate(PlayerCharaObj1, MyStartCharaPos, Quaternion.identity) as GameObject;
                        Obj.transform.parent = PlayerParentObj.transform;
                        playerscr = Obj.GetComponent<PlayerScr>();
                        playerscr.PlayerSwitch(nHP, nAttack, type);
                        charalist.PlayerCharaPut(Obj);
                        uitest.PlayerStuck(1);
                        break;
                    case Type.Chara.B:
                        Obj = Instantiate(PlayerCharaObj2, MyStartCharaPos, Quaternion.identity) as GameObject;
                        Obj.transform.parent = PlayerParentObj.transform;
                        playerscr = Obj.GetComponent<PlayerScr>();
                        playerscr.PlayerSwitch(nHP, nAttack, type);
                        charalist.PlayerCharaPut(Obj);
                        uitest.PlayerStuck(1);
                        break;
                    case Type.Chara.C:
                        Obj = Instantiate(PlayerCharaObj3, MyStartCharaPos, Quaternion.identity) as GameObject;
                        Obj.transform.parent = PlayerParentObj.transform;
                        playerscr = Obj.GetComponent<PlayerScr>();
                        playerscr.PlayerSwitch(nHP, nAttack, type);
                        charalist.PlayerCharaPut(Obj);
                        uitest.PlayerStuck(1);
                        break;
                }
                break;

            case 2:
                switch (type)
                {
                    case Type.Chara.A:
                        Obj = Instantiate(PlayerCharaObj1, MyStartCharaPos, Quaternion.identity) as GameObject;
                        Obj.transform.parent = PlayerParentObj.transform;
                        playerscr = Obj.GetComponent<PlayerScr>();
                        playerscr.PlayerSwitch(nHP, nAttack, type);
                        charalist.PlayerCharaPut(Obj);
                        uitest.PlayerStuck(2);
                        break;
                    case Type.Chara.B:
                        Obj = Instantiate(PlayerCharaObj2, MyStartCharaPos, Quaternion.identity) as GameObject;
                        Obj.transform.parent = PlayerParentObj.transform;
                        playerscr = Obj.GetComponent<PlayerScr>();
                        playerscr.PlayerSwitch(nHP, nAttack, type);
                        charalist.PlayerCharaPut(Obj);
                        uitest.PlayerStuck(2);
                        break;
                    case Type.Chara.C:
                        Obj = Instantiate(PlayerCharaObj3, MyStartCharaPos, Quaternion.identity) as GameObject;
                        Obj.transform.parent = PlayerParentObj.transform;
                        playerscr = Obj.GetComponent<PlayerScr>();
                        playerscr.PlayerSwitch(nHP, nAttack, type);
                        charalist.PlayerCharaPut(Obj);
                        uitest.PlayerStuck(2);
                        break;
                }
                break;
        }
    }
    //敵生成
    public GameObject EnemyForm(int num){
        GameObject obj;
        switch (num){
            case 0:
                obj = Instantiate(Enemy1Obj, EnemyStartPos, Quaternion.identity) as GameObject;
                obj.transform.parent = EnemyParentObj.transform;
                charalist.EnemyCharaPut(obj);
                GetData(num);
                obj.GetComponent<EnemyScr>().Status(nHP, nAttack, m_Type, MyStartCharaPos);
                return obj;
        }
        return null;
    }


    //Resourcesフォルダーからデータ取得
    void Acquisition(){
        PlayerCharaObj1 = (GameObject)Resources.Load("Player/PlayerChara1");
        PlayerCharaObj2 = (GameObject)Resources.Load("Player/PlayerChara2");
        PlayerCharaObj3 = (GameObject)Resources.Load("Player/PlayerChara3");

        Enemy1Obj = (GameObject)Resources.Load("Enemy/Enemy1");
    }

    // プレイヤーを外部から生成
    public void CreatePlayer(Type.Chara type , int nStrong)
    {
        switch (nStrong)
        {
            case 0:
                GetData(0);
                break;
            case 1:
                GetData(1);
                break;
            case 2:
                GetData(2);
                break;
        }
        CharaForm(type);
        nCharaNum = nStrong;
    }

    public void DebugChara(){
        GetData(nCharaNum);
        CharaForm(Type.Chara.A);   

        nCharaNum++;

        if (nCharaNum >= 2)
        {
            nCharaNum = 0;
        }
    }
}
