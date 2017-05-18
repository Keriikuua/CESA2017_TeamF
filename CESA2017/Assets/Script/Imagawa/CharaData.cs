using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharaData : MonoBehaviour {
    
    //構造体配列
    public struct CharaDataArray{
        public int nHP;
        public int nAttack;
        public int nWeakness;
    }

    CharaDataArray[] charaArray;

    private void Start(){
        charaArray = new CharaDataArray[10];

        InputData();
    }

    private void Update(){

    }



    //データを渡すところ
    public int AttackReturn(int Number){
        return charaArray[Number].nAttack;
    }

    public int HPReturn(int Number){
        return charaArray[Number].nHP;
    }

    public int WeakReturn(int Number){
        return charaArray[Number].nWeakness;
    }

    //プランナーの仕事場
    void InputData(){
        charaArray[0].nHP = 2;
        charaArray[0].nAttack = 2;
        charaArray[0].nWeakness = 0;

        charaArray[1].nHP = 2;
        charaArray[1].nAttack = 2;
        charaArray[1].nWeakness = 0;

        charaArray[2].nHP = 10;
        charaArray[2].nAttack = 200;
        charaArray[2].nWeakness = 300;

        charaArray[3].nHP = 1000;
        charaArray[3].nAttack = 2000;
        charaArray[3].nWeakness = 3000;

        charaArray[4].nHP = 10000;
        charaArray[4].nAttack = 20000;
        charaArray[4].nWeakness = 30000;

        charaArray[5].nHP = 100000;
        charaArray[5].nAttack = 200000;
        charaArray[5].nWeakness = 300000;
    }


}
