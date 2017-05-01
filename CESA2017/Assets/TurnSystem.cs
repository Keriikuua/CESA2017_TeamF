using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnSystem : MonoBehaviour {

    private int nWhichTurn = 0;
    //
    public enum TurnState : int{
        nNone = 0,
        nMovePhase,
        nInterposePhase,
        nBattlePhase
    }
    public TurnState turnstate;

    void Start(){
        turnstate = TurnState.nNone;
    }
    

    void Update(){
        //ターン処理
        switch (turnstate){
            //
            case TurnState.nNone:
                Debug.Log("なんもなし");
                break;
            //移動フェーズ
            case TurnState.nMovePhase:
                MovePhase();
            break;
            //挟むフェーズ
            case TurnState.nInterposePhase:
                InterposePhase();
            break;
            //バトルフェーズ
            case TurnState.nBattlePhase:
                BattlePhase();
            break;
        }

        //デバッグコマンド
        if (Input.GetKeyDown(KeyCode.Return)){
            turnstate++;
            if(turnstate < TurnState.nBattlePhase){
                turnstate = TurnState.nNone;
            }
        }

    }


    //移動フェーズ
    void MovePhase(){
        Debug.Log("移動フェーズ");
    }

    //挟むフェーズ
    void InterposePhase(){
        Debug.Log("挟むフェーズ");
    }

    //バトルフェーズ
    void BattlePhase(){
        Debug.Log("バトルフェーズ");
    }

}
