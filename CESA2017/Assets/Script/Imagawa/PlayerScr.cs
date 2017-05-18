using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerScr : MonoBehaviour {

    public int nHP;
    public int nAttack;
    public int nWeaknes;

    Vector3 bPlayerGoPos;             //最初に向かう座標をいれる
    bool bGoFlg;

    TurnSystem turnsystem;

    //目的地に行く
    float nowTime = 0;
    float endTime = 2;
    Vector3 InitPos = Vector3.zero;

    Vector3 CenterPos;                  //真ん中の座標
    Vector3 NowPos;
    bool bcenterflg;

    void Awake(){
        turnsystem = GameObject.Find("TurnObj").GetComponent<TurnSystem>();
        bGoFlg = false;
        bcenterflg = false;
        InitPos = this.gameObject.transform.position;
    }

    void Update(){
        //最初の目的地に向かう
        if (bGoFlg == true){
            if(transform.position != bPlayerGoPos){
                if (nowTime <= endTime){
                    float rate = nowTime / endTime;
                    this.transform.position = Vector3.Lerp(InitPos, bPlayerGoPos, rate);
                    nowTime += Time.deltaTime;
                }else{
                    transform.position = bPlayerGoPos;
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
                this.transform.position = Vector3.Lerp(NowPos, CenterPos, rate);
                nowTime += Time.deltaTime;
            }
            else
            {
                transform.position = CenterPos;
            }
        }



    }

    //プレイヤーがどっちに行くか決める＆ステータス格納
    public void PlayerSwitch(Vector3 pos,int hp,int attack,int weake){
        bPlayerGoPos = pos;
        nHP = hp;
        nAttack = attack;
        nWeaknes = weake;
        
        bGoFlg = turnsystem.MoveDecision(this.gameObject);
    }

    public void WaitClear() {
        bGoFlg = true;
    }

    public void PosZ()
    {
        if(transform.position.z == 0)
        {
            CenterPos = new Vector3(transform.position.x, transform.position.y, transform.position.z + 2);
        }

        if (transform.position.z == 4)
        {
            CenterPos = new Vector3(transform.position.x, transform.position.y, transform.position.z - 2);
        }
        NowPos = transform.position;
        nowTime = 0;
        bcenterflg = true;
    }

}
