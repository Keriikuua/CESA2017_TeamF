using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerScr : MonoBehaviour {

    public int nHP;
    public int nAttack;
    public Type.Chara m_Type;

    Vector3 bPlayerGoPos;             //最初に向かう座標をいれる
    bool bGoFlg;
    bool bHitFlg;
    
    TurnSystem turnsystem;

    //目的地に行く
    float nMoveNowTime;             //現在の時間
    float nMoveEndTime;             //移動時間
    bool bMoveOn;                   //移動開始
    bool bMultiIn;                  //複数回入るの防止
    Vector3 DestinationPos;         //目的地のポジション
    Vector3 InitPos;                //移動開始する前のポジション


    int nMoveCount;

    void Awake(){
        turnsystem = GameObject.Find("TurnObj").GetComponent<TurnSystem>();

        turnsystem.PlayerStuckIn(this.gameObject);
    }

    
    private void Start()
    {
        nMoveNowTime = 0;
        nMoveEndTime = 0;
        bMoveOn = false;
        bMultiIn = false;
        DestinationPos = Vector3.zero;
        InitPos = Vector3.zero;

        nMoveCount = 0;

    }//初期化

    void Update(){
        MovePlayer();

        if(transform.position.x <= 93.0f)
        {
            Destroy(this.gameObject);
        }
    }

    
    void MovePlayer()
    {
        if (bMoveOn)
        {
            if (nMoveNowTime <= nMoveEndTime)
            {
                float rate = nMoveNowTime / nMoveEndTime;
                this.transform.position = Vector3.Lerp(InitPos, DestinationPos, rate);
                nMoveNowTime += Time.deltaTime;
            }
            else
            {
                turnsystem.PlayerListChange(this.gameObject);
                nMoveNowTime = 0;
                bMoveOn = false;
                bMultiIn = false;

                if(nMoveCount == 1)
                {
                    turnsystem.SutuckUIDelete();
                }

                if(nMoveCount == 2)
                {
                    RotationRiset();
                    
                }
            }
        }
    }//プレイヤーの移動関係

    
    public void PlayerSwitch(int hp,int attack,Type.Chara type){
        nHP = hp;
        nAttack = attack;
        m_Type = type;
    }//ステータス格納

    public void MovePhasePlus(Vector3 pos, float time)
    {
        if (!bMultiIn)
        {
            DestinationPos = pos;
            InitPos = this.transform.position;
            nMoveEndTime = time;
            transform.LookAt(pos);

            bMoveOn = true;
            bMultiIn = true;
            nMoveCount++;
            this.gameObject.GetComponent<Animator>().SetBool("WaiteFlg", true);
        }
    }//目的地とそこに移動するまでの時間を格納＆移動開始

    public void RotationRiset()
    {
        transform.localRotation = Quaternion.Euler(0.0f, -90.0f, 0.0f);
    }//回転のリセット

    public void AnimaFlgChange()
    {
        this.gameObject.GetComponent<Animator>().SetBool("WaiteFlg", false);
        this.gameObject.GetComponent<Animator>().SetBool("AttackFlg", true);
    }

    public int PlayerType()
    {
        switch (m_Type)
        {
            case Type.Chara.A:
                return 1;
            case Type.Chara.B:
                return 2;
            case Type.Chara.C:
                return 3;
        }

        return 0;
    }

    private void OnCollisionEnter(Collision coll)
    {
        if (!bHitFlg) {
            if (coll.transform.tag == "UpEnemy")
            {
                Debug.Log("hit" + gameObject.name);
                coll.gameObject.GetComponent<EnemyScr>().OnHit(this.gameObject);
                bHitFlg = true;
            }
            
        }
    }

}
