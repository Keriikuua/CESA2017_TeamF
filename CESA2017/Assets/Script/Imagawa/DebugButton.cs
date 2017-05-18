using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DebugButton : MonoBehaviour {

    public GameObject ParentObj1;
    public GameObject ParentObj2;
    public Text text;
    TurnSystem turnsystem;
    Character chara;

    private void Start(){
        turnsystem = ParentObj1.GetComponent<TurnSystem>();
        chara = ParentObj2.GetComponent<Character>();
    }

    public void DebugTrunSkip(){
        turnsystem.TrunSkip();
        DebugTurn();
    }

    public void DebugCharaInst(){
        chara.DebugChara();
    }

    void DebugTurn(){
        string sTurn = turnsystem.SetTurnState().ToString();
        switch (sTurn){
            case "nNone":
                text.text = "まだ考えてるターン";
            break;
            case "nEnemyForm":
                text.text = "敵生成フェーズ";
            break;
            case "nMovePhase":
                text.text = "移動フェーズ";
                break;
            case "nInterposePhase":
                text.text = "挟むフェーズ";
                break;
            case "nBattlePhase":
                text.text = "バトルフェーズ";
                break;
        }
    }

}
