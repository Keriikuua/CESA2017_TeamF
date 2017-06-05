using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharaList : MonoBehaviour {

    //リスト宣言
    public List<GameObject> PlayerList = new List<GameObject>();
    List<GameObject> EnemyList = new List<GameObject>();
    List<GameObject> ObstacleList = new List<GameObject>();

    //デバッグ用
    int n = 0;

    void Start(){
        
    }

    void Update(){
        for(int i = 0;i < PlayerList.Count; i++){
            if(PlayerList[i] == null){
                PlayerList.RemoveAt(i);
            }
        }

        for (int i = 0; i < EnemyList.Count; i++)
        {
            if (EnemyList[i] == null)
            {
                EnemyList.RemoveAt(i);
            }
        }
    }


    //リストにプレイヤーキャラを格納
    public void PlayerCharaPut(GameObject obj){
        PlayerList.Add(obj);
        n++;
    }
    //リストにエネミーキャラを格納
    public void EnemyCharaPut(GameObject obj) {
        EnemyList.Add(obj);
    }
    //リストに障害物を格納
    public void ObstaclePut(GameObject obj){
        ObstacleList.Add(obj);
    }

}
