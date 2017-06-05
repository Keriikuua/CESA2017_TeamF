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
    Rigidbody rigid;

    //目的地に行く
    float nowTime = 0;
    float endTime = 2;
    Vector3 InitPos = Vector3.zero;

    Vector3 CenterPos;                  //真ん中の座標
    bool MoveOKFlg = false;
    Vector3 NowPos;
    bool bcenterflg;

    int nFlg;

    void Awake(){
        turnsystem = GameObject.Find("TurnObj").GetComponent<TurnSystem>();
        rigid = GetComponent<Rigidbody>();
        bGoFlg = false;
        bcenterflg = false;
        bHitFlg = false;
        InitPos = this.gameObject.transform.position;

        this.gameObject.tag = "UpPlayer";
    }

    void Update(){
        //最初の目的地に向かう
        if (bGoFlg == true){
            if(transform.position != bPlayerGoPos){
                if (nowTime <= endTime){
                    float rate = nowTime / endTime;
                    transform.position = Vector3.Lerp(InitPos, bPlayerGoPos, rate);

                    nowTime += Time.deltaTime;
                }else{
                    transform.position = bPlayerGoPos;
                    rigid.constraints = RigidbodyConstraints.FreezeRotation | RigidbodyConstraints.FreezePositionY;
                }

            }
            else{
                    turnsystem.PlayerMoveOk(this.gameObject);

                bGoFlg = false;
            }
        }

        if (bcenterflg)
        {
            if (nowTime <= endTime)
            {
                float rate = nowTime / endTime;
                rigid.MovePosition(Vector3.Lerp(NowPos, CenterPos, rate));
                nowTime += Time.deltaTime;
            }
            else
            {
                rigid.MovePosition(CenterPos);
            }
        }
    }

    //プレイヤーがどっちに行くか決める＆ステータス格納
    public void PlayerSwitch(Vector3 pos,int hp,int attack,Type.Chara type){
        bPlayerGoPos = pos;
        nHP = hp;
        nAttack = attack;
        m_Type = type;
        
        bGoFlg = turnsystem.MoveDecision(this.gameObject);
    }

    public void WaitClear() {
        bGoFlg = true;
    }

    public void PosZ(Vector3 pos,int num)
    {
        CenterPos = pos;
        nFlg = num;
        //if(transform.position.z == 0)
        //{
        //    CenterPos = new Vector3(transform.position.x, transform.position.y, transform.position.z + 2);
        //}

        //if (transform.position.z == 4)
        //{
        //    CenterPos = new Vector3(transform.position.x, transform.position.y, transform.position.z - 2);
        //}
        NowPos = transform.position;
        nowTime = 0;
        bcenterflg = true;
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
