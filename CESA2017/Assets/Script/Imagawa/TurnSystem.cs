using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnSystem : MonoBehaviour
{
    //
    public enum TurnState : int
    {
        nNone = 0,
        nEnemyForm,         //敵生成
        nMovePhase,         //移動
        nInterposePhase,    //挟む
    }
    [Header("enumのターン系")]
    public TurnState turnstate;

    [SerializeField]
    [Header("タッチした時のレイ飛ばすためのカメラ格納")]
    public GameObject CameraObj;


    //移動可能オブジェ格納用リスト
    [SerializeField]
    [Header("敵の移動可能オブジェ")]
    List<GameObject> EnemyMoveOkList = new List<GameObject>();              //敵の移動可能オブジェ


    [SerializeField]
    [Header("プレイヤーの移動可能オブジェ")]
    List<GameObject> PlayerMoveOkList = new List<GameObject>();              //プレイヤーの移動可能オブジェ

    [SerializeField]
    [Header("左右の移動待ちオブジェ")]
    List<GameObject> MoveOkProvisional = new List<GameObject>();            //左右で移動待ちのオブジェ
    [SerializeField]
    [Header("スタックされてるオブジェ")]
    List<GameObject> PlayerStuckList = new List<GameObject>();              //スタックされてるオブジェクト
    [SerializeField]
    [Header("左右のレーンに移動しているオブジェクト")]
    List<GameObject> LeftAndRightMoveObj = new List<GameObject>();          //左右レーンに移動しているオブジェクト格納用

    //Enemy生成データ格納
    StageForm stageform;
    List<string[]> StageEnemyData = new List<string[]>();
    int nEvacuate = 0;
    bool bFormEnd;
    bool bHasamuFlg;
    bool bRandomFormFlg;

    int nTurn;              //現在のターン数
    int nEnemyCount;        //倒した敵の数

    Character character;
    FieldManager fildmane;
    DebugButton DButton;
    UITest uitest;
    FieldManager MapData;
    
    
   
    Vector3[] PlayerRighitLeftPosition = new Vector3[2];                //左右の自キャラが待機する座標
    Vector3 PlayerWaitePosition;                                        //左右移動前の待機場所

    [SerializeField]
    [Header("挟めるオブジェクト格納用")]
    GameObject[] InterposeObj = new GameObject[2];      //挟めるオブジェクト格納用
    [SerializeField]
    [Header("↓一つ目の指がタッチしたオブジェクト")]
    List<GameObject> FarstTuchObj = new List<GameObject>();
    [SerializeField]
    [Header("↓二つ目の指がタッチしたオブジェクト")]
    List<GameObject> SecondTuchObj = new List<GameObject>();
    List<GameObject> ThreeTuchObj = new List<GameObject>();
    //フリックの終了地点
    Vector3 TouchEndPosLeft;
    Vector3 TouchEndPosRight;
    //フリックが正しい判定用
    int nCorrect;
    //等速移動用変数
    float nowTime = 0.0f;
    float endTime = 2.0f;
    //等速移動用のスタート地点
    Vector3 StartPos_Right;

    bool bDestoryFlg = false;
    bool HasamuMoveFlg = false;

    //自動ターン用変数
    float TurnTime;             //現在のターンに入ってからの時間
    bool MoveFlg = false;       //一回移動用
    //スタック
    bool StuckFlg = false;
    bool UITimeFlg = false;
    bool EnemyStuckFlg = false;

    //加速関係
    int acceleNum = 0;

    //壁の生成
    bool WallOnFlg;
    public GameObject WallObj;

    private void Awake()
    {

    }


    void Start()
    {
        turnstate = TurnState.nNone;
        bFormEnd = false;
        bHasamuFlg = false;
        bRandomFormFlg = false;
        WallOnFlg = false;
        nTurn = 1;
        nCorrect = 0;
        TurnTime = 0;

        character = GameObject.Find("Chara").GetComponent<Character>();
        fildmane = GameObject.Find("MapData").GetComponent<FieldManager>();
        DButton = GameObject.Find("Button").GetComponent<DebugButton>();
        uitest = GameObject.Find("UI").GetComponent<UITest>();
        MapData = GameObject.Find("MapData").GetComponent<FieldManager>();
        stageform = GetComponent<StageForm>();
        StageEnemyData = stageform.StageData_Enemy();


        PlayerRighitLeftPosition[0] = fildmane.SetMyRightCharaPos();
        PlayerRighitLeftPosition[1] = fildmane.SetMyLeftCharaPos();
        PlayerWaitePosition = fildmane.SetMyWaiteCharaPos();

        
    }


    void Update()
    {
        
        //ここでリストから消えたのがあれば削除
        for(int i = 0;i < PlayerMoveOkList.Count; i++)
        {
            if(PlayerMoveOkList[i] == null)
            {
                PlayerMoveOkList.RemoveAt(i);
            }
        }

        for (int i = 0; i < EnemyMoveOkList.Count; i++)
        {
            if (EnemyMoveOkList[i] == null)
            {
                EnemyMoveOkList.RemoveAt(i);
            }
        }

        for (int i = 0; i < FarstTuchObj.Count; i++)
        {
            if (FarstTuchObj[i] == null)
            {
                FarstTuchObj.RemoveAt(i);
            }
        }
        for (int i = 0; i < SecondTuchObj.Count; i++)
        {
            if (SecondTuchObj[i] == null)
            {
                SecondTuchObj.RemoveAt(i);
            }
        }
        for (int i = 0; i < ThreeTuchObj.Count; i++)
        {
            if (ThreeTuchObj[i] == null)
            {
                ThreeTuchObj.RemoveAt(i);
            }
        }

        PlayerMovePhaseCheck();
        PlayerLeftRightMove();
       
        //ターン処理
        switch (turnstate)
        {
            //
            case TurnState.nNone:
                TurnTime += Time.deltaTime;

                if (!StuckFlg)
                {
                    StuckFlg = true;
                    EnemyStuck();
                }

                if (TurnTime >= 0.5f)
                {
                    turnstate++;
                    TurnTime = 0;
                    DButton.TurnNum(nTurn);
                    StuckFlg = false;
                }



                break;

            //敵生成フェーズ
            case TurnState.nEnemyForm:
                TurnTime += Time.deltaTime;
                if (TurnTime >= 3)
                {
                    uitest.TimeUIActive();
                    turnstate++;
                    TurnTime = 0;
                }

                if (StageEnemyData.Count <= nTurn)
                {
                    if (!bRandomFormFlg)
                    {
                        int n = Random.Range(0, 5);
                        if (n == 1 || n == 2)
                        {
                            EnemyMoveOkList.Add(character.EnemyForm(0));
                            
                            bFormEnd = true;
                            Debug.Log("ランダム");
                        }
                        bRandomFormFlg = true;
                    }
                }
                else
                {
                    //壁の生成
                    if (!WallOnFlg)
                    {
                        WallCreate();
                        WallOnFlg = true;
                    }

                    nEvacuate = int.Parse(StageEnemyData[nTurn - 1][0]);
                    if (nEvacuate != 0 && !bFormEnd)
                    {
                        EnemyMoveOkList.Add(character.EnemyForm(nEvacuate));
                        uitest.EnemySortie();
                        bFormEnd = true;
                    }
                }
                
                break;
            //移動フェーズ
            case TurnState.nMovePhase:
                if (!MoveFlg)
                {
                    PlayerMoveOk();
                    MovePhase();
                }
                TurnTime += Time.deltaTime;


                if (TurnTime >= 3)
                {
                    turnstate++;
                    MoveFlg = false;
                    TurnTime = 0;
                    UITimeFlg = false;
                }
                break;
            //挟むフェーズ
            case TurnState.nInterposePhase:
                InterposePhase();
                TurnTime += Time.deltaTime;

                if (!UITimeFlg)
                {
                    uitest.TurnTime(5);
                    UITimeFlg = true;
                }

                if (TurnTime >= 5.0f)
                {
                    turnstate = TurnState.nEnemyForm;
                    DButton.TurnNum(nTurn);
                    bFormEnd = false;
                    TurnTime = 0;
                    nTurn++;

                    StuckFlg = true;
                    EnemyStuck();
                    WallOnFlg = false;
                }
                break;
        }
    }


    //移動フェーズ
    void MovePhase()
    {
        if (acceleNum == 0)
        {
            //プレイヤーの移動
            for (int i = 0; i < PlayerMoveOkList.Count; i++)
            {
                if (PlayerMoveOkList[i] == null)
                {
                    PlayerMoveOkList.RemoveAt(i);
                    continue;
                }
                PlayerMoveOkList[i].GetComponent<PlayerScr>().MovePhasePlus(new Vector3(PlayerMoveOkList[i].transform.position.x - 2.2f, PlayerMoveOkList[i].transform.position.y, PlayerMoveOkList[i].transform.position.z), 2);
            }
        }else{
            //プレイヤーの移動
            for (int i = 0; i < PlayerMoveOkList.Count; i++)
            {
                if (PlayerMoveOkList[i] == null)
                {
                    PlayerMoveOkList.RemoveAt(i);
                    continue;
                }
                PlayerMoveOkList[i].GetComponent<PlayerScr>().MovePhasePlus(new Vector3(PlayerMoveOkList[i].transform.position.x - 2.2f * acceleNum, PlayerMoveOkList[i].transform.position.y, PlayerMoveOkList[i].transform.position.z), 2);
            }

            acceleNum = 0;
        }
        //敵キャラの移動
        for (int i = 0; i < EnemyMoveOkList.Count; i++)
        {
            if (EnemyMoveOkList[i] == null)
            {
                EnemyMoveOkList.RemoveAt(i);
                continue;
            }
            EnemyMoveOkList[i].GetComponent<EnemyScr>().MoveOn();
            //EnemyMoveOkList[i].transform.position += new Vector3(1.5f, 0, 0);
        }

        MoveFlg = true;
        
    }
 
    void InterposePhase()
    {
        //画面を押してオブジェクトに触れている指の数
        int nTouchObjNum = 0;
        MoveFlg = false;
        if (!HasamuMoveFlg)
        {
            for (int i = 0; i < Input.touchCount; i++)
            {
                if (Input.touches[i].phase == TouchPhase.Moved || Input.touches[i].phase == TouchPhase.Stationary)
                {
                    //1つ目の指が触れてるもの
                    if (i == 0)
                    {
                        var ray = CameraObj.GetComponent<Camera>().ScreenPointToRay(Input.touches[i].position);
                        RaycastHit[] hit = new RaycastHit[100];
                        hit = Physics.RaycastAll(ray, 500.0f);
                        if (hit.Length > 0)
                        {
                            for (int b = 0; b < hit.Length; b++)
                            {
                                //当たったオブジェクトがキャラリストにあるかチェック
                                for (int v = 0; v < PlayerMoveOkList.Count; v++)
                                {
                                    if (hit[b].collider.gameObject == PlayerMoveOkList[v].gameObject)
                                    {
                                        FarstTuchObj.Add(hit[b].collider.gameObject);
                                        nTouchObjNum++;
                                    }
                                }
                            }
                        }
                        nTouchObjNum = 0;
                    }

                    //2つ目の指が触れてるもの
                    if (i == 1)
                    {
                        var ray = CameraObj.GetComponent<Camera>().ScreenPointToRay(Input.touches[i].position);
                        RaycastHit[] hit = new RaycastHit[100];
                        hit = Physics.RaycastAll(ray, 500.0f);
                        if (hit.Length > 0)
                        {
                            for (int b = 0; b < hit.Length; b++)
                            {
                                //当たったオブジェクトがキャラリストにあるかチェック
                                for (int v = 0; v < PlayerMoveOkList.Count; v++)
                                {
                                    if (hit[b].collider.gameObject == PlayerMoveOkList[v].gameObject)
                                    {
                                        SecondTuchObj.Add(hit[b].collider.gameObject);
                                        nTouchObjNum++;
                                    }
                                }
                            }
                        }
                        nTouchObjNum = 0;
                    }
                    //念のため3つ目の指が触れてるもの
                    if (i == 2)
                    {
                        var ray = CameraObj.GetComponent<Camera>().ScreenPointToRay(Input.touches[i].position);
                        RaycastHit[] hit = new RaycastHit[100];
                        hit = Physics.RaycastAll(ray, 500.0f);
                        if (hit.Length > 0)
                        {
                            for (int b = 0; b < hit.Length; b++)
                            {
                                //当たったオブジェクトがキャラリストにあるかチェック
                                for (int v = 0; v < PlayerMoveOkList.Count; v++)
                                {
                                    if (hit[b].collider.gameObject == PlayerMoveOkList[v].gameObject)
                                    {
                                        ThreeTuchObj.Add(hit[b].collider.gameObject);
                                        nTouchObjNum++;
                                    }
                                }
                            }
                        }
                    }
                    nTouchObjNum = 0;
                }
            }
        }
        //挟めるオブジェクトか判定      
        for (int i = 0; i < FarstTuchObj.Count; i++)
        {
            Vector3 pos = FarstTuchObj[i].transform.position;

            for (int v = 0; v < SecondTuchObj.Count; v++)
            {
                if (pos.z != SecondTuchObj[v].transform.position.z)
                {
                    //横かどうか判定
                    if (pos.x <= SecondTuchObj[v].transform.position.x + 0.5f && pos.x >= SecondTuchObj[v].transform.position.x - 0.5f ) { 
                    
                        //ゼロが左でイチが右
                        if (SecondTuchObj[v].transform.position.z < pos.z)
                        {
                            InterposeObj[0] = FarstTuchObj[i].transform.gameObject;
                            InterposeObj[1] = SecondTuchObj[v].transform.gameObject;

                        }
                        else 
                        {
                            InterposeObj[1] = FarstTuchObj[i].transform.gameObject;
                            InterposeObj[0] = SecondTuchObj[v].transform.gameObject;

                        }

                        break;
                    }
                    //斜め判定
                    if (pos.x + 3 == SecondTuchObj[v].transform.position.x || pos.x - 3 == SecondTuchObj[v].transform.position.x)
                    {
                        //ゼロが左でイチが右
                        if (SecondTuchObj[v].transform.position.z < pos.z)
                        {
                            InterposeObj[0] = FarstTuchObj[i].transform.gameObject;
                            InterposeObj[1] = SecondTuchObj[v].transform.gameObject;

                        }
                        else
                        {
                            InterposeObj[1] = FarstTuchObj[i].transform.gameObject;
                            InterposeObj[0] = SecondTuchObj[v].transform.gameObject;

                        }
                    }
                    break;
                }
            }
        }
        //-----------------------------------------------------------------------------------------------------------  
        if (Input.touchCount >= 2 && !bHasamuFlg)
        {
            for (int i = 0; i < Input.touchCount; i++)
            {
                if (Input.touches[i].phase == TouchPhase.Ended)
                {
                    if (InterposeObj[1] != null)
                    {
                        //フリック終わりのポジションを格納
                        if (Input.touches[i].position.x > 200)
                        {
                            nCorrect++;
                        }
                        if (Input.touches[i].position.x < 200)
                        {
                            nCorrect++;
                        }
                    }
                }
            }
        }
        //真ん中に向かってきてフリックしているかチェック
        //if (TouchEndPosLeft.x != 0 && TouchStartPosLeft.x < TouchEndPosLeft.x && TouchEndPosLeft.x < 0 && nCorrect != 2)
        //{
        //    Debug.Log("1");

        //}
        //if (TouchEndPosRight.x != 0 && TouchStartPosRight.x > TouchEndPosRight.x && TouchEndPosRight.x > 0 && nCorrect != 2)
        //{

        //}

        if (nCorrect == 2)
        {
            if (!bDestoryFlg)
            {
                InterposeObj[0].GetComponent<PlayerScr>().MovePhasePlus(InterposeObj[1].transform.position,1);
                InterposeObj[1].GetComponent<PlayerScr>().MovePhasePlus(InterposeObj[0].transform.position,1);
                bDestoryFlg = true;
                HasamuMoveFlg = true;
            }
            if (nowTime < endTime / 2)
            {

                nowTime += Time.deltaTime;
            }
            else
            {

                for (int i = 0; i < EnemyMoveOkList.Count; i++)
                {
                    //if (EnemyMoveOkList[i].transform.position.x >= InterposeObj[0].transform.position.x - 0.3f && EnemyMoveOkList[i].transform.position.x <= InterposeObj[0].transform.position.x + 0.3f)
                    //{
                    //    BattlePhase(InterposeObj[0], InterposeObj[1], EnemyMoveOkList[i]);
                    //}
                }
                HasamuMoveFlg = false;
                //Destroy(InterposeObj[0]);
                //Destroy(InterposeObj[1]);
                InterposeObj[0] = null;
                InterposeObj[1] = null;
                nCorrect = 0;
                nowTime = 0;
                bDestoryFlg = false;
                bHasamuFlg = false;
                //turnstate++;
            }
        }
        else
        {
            nCorrect = 0;
        }


        for (int i = 0; i < FarstTuchObj.Count; i++)
        {
            FarstTuchObj.RemoveAt(0);
        }

        for (int i = 0; i < SecondTuchObj.Count; i++)
        {
            SecondTuchObj.RemoveAt(0);
        }

        for (int i = 0; i < ThreeTuchObj.Count; i++)
        {
            SecondTuchObj.RemoveAt(0);
        }

        if (Input.GetKeyDown(KeyCode.L))
        {
            InterposeObj[0] = PlayerMoveOkList[0].gameObject;
            InterposeObj[1] = PlayerMoveOkList[1].gameObject;
            InterposeObj[0].GetComponent<PlayerScr>().MovePhasePlus(InterposeObj[1].transform.position, 1);
            InterposeObj[1].GetComponent<PlayerScr>().MovePhasePlus(InterposeObj[0].transform.position, 1);
            InterposeObj[0].GetComponent<PlayerScr>().AnimaFlgChange();
            InterposeObj[1].GetComponent<PlayerScr>().AnimaFlgChange();
        }
    }//挟むフェーズ

    public void BattlePhase(GameObject pObj1, GameObject pObj2, GameObject eObj)
    {
        PlayerScr pObj1Scr = pObj1.GetComponent<PlayerScr>();
        PlayerScr pObj2Scr = pObj2.GetComponent<PlayerScr>();
        EnemyScr eObjScr = eObj.GetComponent<EnemyScr>();
        int nPlayerAttack = pObj1Scr.nAttack + pObj2Scr.nAttack;
        int nEnemyHp = eObjScr.nHp;

        nEnemyHp -= nPlayerAttack;
        if (nEnemyHp <= 0)
        {
            for (int i = 0; i < EnemyMoveOkList.Count; i++)
            {
                if (EnemyMoveOkList[i] == eObj)
                {
                    EnemyMoveOkList.RemoveAt(i);
                }
            }
            EnemyDowun();
            Destroy(eObj);
            
        }
        //PlayerMoveOkListから消すオブジェクトを削除する
        for (int i = 0; i < PlayerMoveOkList.Count; i++)
        {
            if (PlayerMoveOkList[i].gameObject == pObj1)
            {
                Destroy(PlayerMoveOkList[i].gameObject);
                PlayerMoveOkList.RemoveAt(i);
            }
        }

        for (int i = 0; i < PlayerMoveOkList.Count; i++)
        {
            if (PlayerMoveOkList[i].gameObject == pObj2)
            {
                Destroy(PlayerMoveOkList[i].gameObject);
                PlayerMoveOkList.RemoveAt(i);
            }
        }

    }//バトルフェーズ

    void EnemyStuck()
    {
        if (EnemyStuckFlg)
        {
            if (StageEnemyData.Count > nTurn + 2)
            {
                nEvacuate = int.Parse(StageEnemyData[nTurn + 2][0]);
                if (nEvacuate != 0)
                {
                    uitest.EnemyStuck(nEvacuate);
                }
            }
        }
        if (!EnemyStuckFlg)
        {
            for (int i = nTurn; i < nTurn + 2; i++)
            {
                nEvacuate = int.Parse(StageEnemyData[i][0]);
                if (nEvacuate != 0)
                {
                    uitest.EnemyStuck(nEvacuate);
                }
            }
            EnemyStuckFlg = true;
        }
    }//敵のスタック

    public void PlayerStuckIn(GameObject obj)
    {
        PlayerStuckList.Add(obj);
        
    }//プレイヤーは生成されたらまずスタックに入る

    void PlayerMovePhaseCheck()
    {
        if (PlayerStuckList.Count >= 1)     //プレイヤーがスタックリストの中に一つ以上あるなら
        {
            int num = MoveOkProvisional.Count + LeftAndRightMoveObj.Count;
            if (num < 2)
            {
                PlayerStuckList[0].GetComponent<PlayerScr>().MovePhasePlus(PlayerWaitePosition, 2);
                
            }
        }
    }//プレイヤーをスタックから動かす

    void PlayerLeftRightMove()
    {
        if(MoveOkProvisional.Count >= 1)
        {
            int num = LeftAndRightMoveObj.Count;
            switch (num)
            {
                case 0:
                    MoveOkProvisional[0].GetComponent<PlayerScr>().MovePhasePlus(PlayerRighitLeftPosition[0], 2);
                    break;
                case 1:
                    MoveOkProvisional[0].GetComponent<PlayerScr>().MovePhasePlus(PlayerRighitLeftPosition[1], 2);
                    break;
            }
        }
    }//左右移動待ちから動かす

    void PlayerMoveOk()
    {
        if(LeftAndRightMoveObj.Count == 2)
        {
            PlayerMoveOkList.Add(LeftAndRightMoveObj[0]);
            PlayerMoveOkList.Add(LeftAndRightMoveObj[1]);
            LeftAndRightMoveObj.RemoveAt(0);
            LeftAndRightMoveObj.RemoveAt(0);
        }
    }//左右レーンに移動できている

    public void PlayerListChange(GameObject obj)
    {
        for (int i = 0;i < PlayerStuckList.Count; i++)
        {
            if(PlayerStuckList[i].gameObject == obj)
            {
                MoveOkProvisional.Add(PlayerStuckList[i]);
                PlayerStuckList.RemoveAt(i);
                return;
            }
        }

        for (int i = 0; i < MoveOkProvisional.Count; i++)
        {
            if (MoveOkProvisional[i].gameObject == obj)
            {
                LeftAndRightMoveObj.Add(MoveOkProvisional[i]);
                MoveOkProvisional.RemoveAt(i);
                return;
            }
        }

    }
    
    void GetDirection()
    {
    }//フリックされた方向取得関数(未実装)

    public TurnState SetTurnState()
    {
        return turnstate;
    }//今どのフェーズなのか把握用
  
    public void DeleteStuck()
    {
        if(PlayerStuckList.Count >= 1)
        {
            for (int i = 0;i < PlayerStuckList.Count;i++)
            {
                Destroy(PlayerStuckList[i].gameObject);
            }

            while (PlayerStuckList.Count != 0)
            {
                PlayerStuckList.RemoveAt(0);
            }
        }
    }//スタックされてるオブジェクトの全削除

    public void EnemyDowun()
    {
        nEnemyCount++;

        if(nEnemyCount >= 2)
        {
            GameObject.Find("Button").GetComponent<DebugButton>().Win();
        }
    }

    void WallCreate()
    {
        int LeftNum = int.Parse(StageEnemyData[nTurn - 1][1]);
        int RightNum = int.Parse(StageEnemyData[nTurn - 1][2]);
        if (LeftNum != 0)
        {
            Vector3 pos = MapData.MapDataPullOut(LeftNum, 1);
            Instantiate(WallObj, pos, Quaternion.identity);
        }

        if (RightNum != 0)
        {
            Vector3 pos = MapData.MapDataPullOut(RightNum, 3);
            Instantiate(WallObj, pos, Quaternion.identity);
        }
    }

    public void SutuckUIDelete()
    {
        uitest.PlayerSortie();
    }

    public void AcceleFullyOpen()
    {
        acceleNum++;
    }//加速ボタンが押されたらここを呼ぶ

}
