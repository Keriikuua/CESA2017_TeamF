using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class EnemyScr : MonoBehaviour {

    public int nHp;
    public int nAtack;
    public Type.Chara m_Type;
    bool bEndflg;
    public Vector3 GoalPos;
    DebugButton dButton;

    public GameObject[] hitobj;
    public int hitnum = 0;

    //移動用の変数
    bool bMoveFlg;
    Vector3 DirectionPos;
    Vector3 NowPos;
    float fMoveTime;
    float fEndTime;


    private void Start()
    {
        hitobj = new GameObject[2];
        bEndflg = false;
        bMoveFlg = false;
    }

    private void Update()
    {
        if(this.transform.position.x >= GoalPos.x - 5.0f)
        {
            if (!bEndflg)
            {
                //dButton = GameObject.Find("Button").GetComponent<DebugButton>();
                //dButton.Endurance(0.2f);
                GameObject.Find("UI").GetComponent<UITest>().Life();
                GameObject.Find("TurnObj").GetComponent<TurnSystem>().EnemyDowun();
                this.gameObject.GetComponent<Animator>().SetBool("AttackFlg",true);
                bEndflg = true;
                //Destroy(this.gameObject);
                //bEndflg = true;
            }
        }

        if (bMoveFlg == true)
        {
            if(fMoveTime < fEndTime)
            {
                float rate = fMoveTime / fEndTime;
                this.transform.position = Vector3.Lerp(NowPos, DirectionPos, rate);
                fMoveTime += Time.deltaTime;
            }
            else
            {              
                bMoveFlg = false;
            }
        }

        if(nHp <= 0)
        {
            GameObject.Find("TurnObj").GetComponent<TurnSystem>().EnemyDowun();
            Destroy(this.gameObject);
            
        }

    }

    public void Status(int nhp,int nattack,Type.Chara type,Vector3 goal)
    {
        nHp = nhp;
        nAtack = nattack;
        m_Type = type;
        GoalPos = goal;
    }

    public void MoveOn()
    {
        fMoveTime = 0;
        fEndTime = 2.0f;
        NowPos = transform.position;
        DirectionPos = new Vector3(transform.position.x + 2.2f, transform.position.y, transform.position.z);
        bMoveFlg = true;
    }

    public void OnHit(GameObject obj)
    {
        hitobj[hitnum] = obj;
        hitnum++;
        if(hitnum >= 2)
        {
            Debug.Log(hitobj[0].name +"" + hitobj[1].name);
            GameObject.Find("TurnObj").GetComponent<TurnSystem>().BattlePhase(hitobj[0], hitobj[1], this.gameObject);
        }
    }

    public void MyDestroy()
    {
        Destroy(this.gameObject);
    }

    void OnCollisionEnter(Collision coll)
    {
        
        //if(coll.gameObject.tag == "UpPlayer")
        //{
        //    hitobj[hitnum] = coll.gameObject;

        //    hitnum++;
        //}  

        //if(hitnum == 2)
        //{
        //    GameObject.Find("TurnObj").GetComponent<TurnSystem>().BattlePhase(hitobj[0], hitobj[1], this.gameObject);
        //}
    }

}
