using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DebugButton : MonoBehaviour {

    public GameObject ParentObj1;
    public GameObject ParentObj2;
    public Text text;
    Text DefeatText;
    Slider slider;
    TurnSystem turnsystem;
    Character chara;

    private void Start(){
        turnsystem = ParentObj1.GetComponent<TurnSystem>();
        chara = ParentObj2.GetComponent<Character>();
        slider = GameObject.Find("EnduranceValue").GetComponent<Slider>();
        DefeatText = GameObject.Find("ENDTEXT").GetComponent<Text>();
        
    }

    private void Update()
    {
        DebugTurn();
    }

    //ターンを次に進める
    public void DebugTrunSkip(){
        turnsystem.TrunSkip();
        DebugTurn();
    }
    //キャラの生成
    public void DebugCharaInst(){
        chara.DebugChara();
    }
    //耐久値減らす
    public void Endurance(float num)
    {
        slider.value -= num;
        if(slider.value <= 0)
        {
            DefeatText.text = "負け";
        }
    }
    //今どののターンか把握するよう
    void DebugTurn(){
        string sTurn = turnsystem.SetTurnState().ToString();
        switch (sTurn){
            case "nNone":
                text.text = "None";
            break;
            case "nEnemyForm":
                text.text = "敵生成";
            break;
            case "nMovePhase":
                text.text = "移動";
                break;
            case "nInterposePhase":
                text.text = "挟む";
                break;
            case "nBattlePhase":
                text.text = "バトル";
                break;
        }
    }

    //スタック全削除
    public void DEbugDeleteStuck()
    {
        turnsystem.DeleteStuck();
    }

}
