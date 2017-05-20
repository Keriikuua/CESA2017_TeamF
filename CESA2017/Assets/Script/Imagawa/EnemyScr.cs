using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyScr : MonoBehaviour {

    public int nHp;
    public int nAtack;
    public Type.Chara m_Type;
    Vector3 GoalPos;

    private void Start()
    {
        
    }

    private void Update()
    {
        if(this.transform.position.x == GoalPos.x)
        {

        }
    }

    public void Status(int nhp,int nattack,Type.Chara type,Vector3 goal)
    {
        nHp = nhp;
        nAtack = nattack;
        m_Type = type;
        GoalPos = goal;
    }

}
