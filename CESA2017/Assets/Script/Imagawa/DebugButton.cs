using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DebugButton : MonoBehaviour {
    
    Text DebugTurntext;
    Text TurnText;
    Text DefeatText;
    TurnSystem turnsystem;
    Character chara;

    private void Start(){
        turnsystem = GameObject.Find("TurnObj").GetComponent<TurnSystem>();
        chara = GameObject.Find("Chara").GetComponent<Character>();

        DefeatText = GameObject.Find("ENDTEXT").GetComponent<Text>();
        TurnText = GameObject.Find("TrunNumText").GetComponent<Text>();
        DebugTurntext = GameObject.Find("DebugTurn").GetComponent<Text>();
    }

    private void Update()
    {
        DebugTurn();
    }

    //ターンを次に進める
    public void DebugTrunSkip(){
        DebugTurn();
    }
    //キャラの生成
    public void DebugCharaInst(){
        chara.DebugChara();
    }
    //勝ち表示
    public void Win()
    {
        DefeatText.text = "勝ち";
    }
    //今どののターンか把握するよう
    void DebugTurn(){
        string sTurn = turnsystem.SetTurnState().ToString();
        switch (sTurn){
            case "nNone":
                DebugTurntext.text = "None";
            break;
            case "nEnemyForm":
                DebugTurntext.text = "敵生成";
            break;
            case "nMovePhase":
                DebugTurntext.text = "移動";
                break;
            case "nInterposePhase":
                DebugTurntext.text = "挟む";
                break;
            case "nBattlePhase":
                DebugTurntext.text = "バトル";
                break;
        }
    }
    //何ターン目か表示用
    public void TurnNum(int num)
    {
        TurnText.text = num.ToString();
    }

    //スタック全削除
    public void DEbugDeleteStuck()
    {
        turnsystem.DeleteStuck();
    }

}
