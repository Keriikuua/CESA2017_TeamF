using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class TurnSystem : MonoBehaviour
{
    //
    public enum TurnState : int
    {
        nNone = 0,
        nEnemyForm,         //敵生成
        nMovePhase,         //移動
        nInterposePhase,    //挟む
        nBattlePhase        //バトル
    }
    public TurnState turnstate;

    public GameObject CameraObj;


    //移動可能オブジェ格納用リスト
    public List<GameObject> PlayerMoveOkList = new List<GameObject>();              //プレイヤーの移動可能オブジェ
    private List<GameObject> EnemyMoveOkList = new List<GameObject>();              //敵の移動可能オブジェ
    public List<GameObject> MoveOkProvisional = new List<GameObject>();                    //左右で移動待ちのオブジェ
    public List<GameObject> MoveInitWaitObj = new List<GameObject>();                      //初期地点で沸くの待ちオブジェ
    public int nPlayerOkCount;
    int nEnemyOkCount;
    int nPlayerCount = 0;                                                           //MoveDecisionで使う

    //ステージデータ格納
    StageForm stageform;
    List<string[]> StageEnemyData = new List<string[]>();
    int nEvacuate = 0;
    bool bFormEnd;
    bool bHasamuFlg;
    bool bRandomFormFlg;

    int nTurn;              //現在のターン数

    Character character;
    CharaList charalist;        //生成されてるキャラの参照のため

    public GameObject[] InterposeObj = new GameObject[2];      //挟めるオブジェクト格納用
    [SerializeField]
    List<GameObject> FarstTuchObj = new List<GameObject>();
    [SerializeField]
    List<GameObject> SecondTuchObj = new List<GameObject>();
    [SerializeField]
    List<GameObject> ThreeTuchObj = new List<GameObject>();
    //タップされたスタート地点の座標
    Vector3 TouchStartPosLeft;
    Vector3 TouchStartPosRight;
    //フリックの終了地点
    Vector3 TouchEndPosLeft;
    Vector3 TouchEndPosRight;
    //フリックされたオブジェクトの移動地点
    Vector3 GoalPos_Left;
    Vector3 GoalPos_Right;
    //フリックが正しい判定用
    int nCorrect;
    //等速移動用変数
    float startTime = 0.0f;
    float nowTime = 0.0f;
    float endTime = 2.0f;
    //等速移動用のスタート地点
    Vector3 StartPos_Left;
    Vector3 StartPos_Right;

    bool bDestoryFlg = false;
    bool HasamuMoveFlg = false;

    //自動ターン用変数
    float TurnTime;             //現在のターンに入ってからの時間
    bool MoveFlg = false;       //一回移動用
    bool MoveCheak = false;     //InitMoveチェック


    private void Awake()
    {

        AddTag("UpEnemy");
        AddTag("UpPlayer");
    }


    void Start()
    {
        turnstate = TurnState.nNone;
        nPlayerOkCount = 0;
        nEnemyOkCount = 0;
        bFormEnd = false;
        bHasamuFlg = false;
        bRandomFormFlg = false;
        nTurn = 0;
        nCorrect = 0;
        TurnTime = 0;

        character = GameObject.Find("Chara").GetComponent<Character>();
        charalist = GameObject.Find("Chara").GetComponent<CharaList>();
        stageform = GetComponent<StageForm>();
        StageEnemyData = stageform.StageData_Enemy();



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



        //ターン処理
        switch (turnstate)
        {
            //
            case TurnState.nNone:
                TurnTime += Time.deltaTime;
                if (TurnTime >= 2)
                {
                    turnstate++;
                    TurnTime = 0;
                }


                break;

            //敵生成フェーズ
            case TurnState.nEnemyForm:
                TurnTime += Time.deltaTime;
                if (TurnTime >= 2)
                {
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
                            
                        }
                        bRandomFormFlg = true;
                    }
                }
                else
                {
                    nEvacuate = int.Parse(StageEnemyData[nTurn][0]);
                    if (nEvacuate == 0 && !bFormEnd)
                    {
                        EnemyMoveOkList.Add(character.EnemyForm(0));
                        bFormEnd = true;
                    }
                }
                    break;
            //移動フェーズ
            case TurnState.nMovePhase:
                MovePhase();
                TurnTime += Time.deltaTime;
                if (TurnTime >= 5)
                {
                    turnstate++;
                    TurnTime = 0;
                }
                break;
            //挟むフェーズ
            case TurnState.nInterposePhase:
                InterposePhase();
                if (!MoveCheak)
                {
                    if (MoveInitWaitObj.Count >= 2 && MoveOkProvisional.Count == 0)
                    {
                        MoveInitWaitObj[0].GetComponent<PlayerScr>().WaitClear();
                        MoveInitWaitObj[1].GetComponent<PlayerScr>().WaitClear();
                        MoveInitWaitObj.RemoveAt(0);
                        MoveInitWaitObj.RemoveAt(0);
                    }
                    MoveCheak = true;
                }
                TurnTime += Time.deltaTime;

                if (TurnTime >= 5)
                {
                    turnstate++;
                    TurnTime = 0;
                }
                break;
            //バトルフェーズ
            case TurnState.nBattlePhase:
                TurnTime += Time.deltaTime;

                if (TurnTime >= 2.5f)
                {
                    turnstate = TurnState.nEnemyForm;
                    nTurn++;
                    bFormEnd = false;
                    TurnTime = 0;
                }
                break;
        }

        //デバッグコマンド
        if (Input.GetKeyDown(KeyCode.Return))
        {
            turnstate++;
            if (turnstate > TurnState.nBattlePhase)
            {
                turnstate = TurnState.nNone;
                nTurn++;
                bFormEnd = false;
            }
        }

    }


    //移動フェーズ
    void MovePhase()
    {
        bRandomFormFlg = false;
        if (!MoveFlg)
        {
            Debug.Log("ifのました");
            if (MoveInitWaitObj.Count > 0 && MoveInitWaitObj.Count < 2)
            {
                MoveInitWaitObj[0].GetComponent<PlayerScr>().WaitClear();
                MoveInitWaitObj.RemoveAt(0);
            }

            if (nPlayerOkCount >= 2)
            {
                Debug.Log("OK");
                PlayerMoveOkList.Add(MoveOkProvisional[0]);
                PlayerMoveOkList.Add(MoveOkProvisional[1]);
                MoveOkProvisional.RemoveAt(0);
                MoveOkProvisional.RemoveAt(0);
                nPlayerCount = 0;
                nPlayerOkCount = 0;
            }

            if (nPlayerOkCount == 0)
            {
                if (MoveInitWaitObj.Count >= 2)
                {
                    MoveInitWaitObj[0].GetComponent<PlayerScr>().WaitClear();
                    MoveInitWaitObj[1].GetComponent<PlayerScr>().WaitClear();
                    MoveInitWaitObj.RemoveAt(0);
                    MoveInitWaitObj.RemoveAt(0);
                }
            }

            //プレイヤーの移動
            for (int i = 0; i < PlayerMoveOkList.Count; i++)
            {
                if (PlayerMoveOkList[i] == null)
                {
                    PlayerMoveOkList.RemoveAt(i);
                    continue;
                }
                PlayerMoveOkList[i].GetComponent<PlayerScr>().PosZ(new Vector3(PlayerMoveOkList[i].transform.position.x - 1.5f, PlayerMoveOkList[i].transform.position.y, PlayerMoveOkList[i].transform.position.z), 1);
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
            MoveCheak = false;
        }

    }

    //挟むフェーズ
    void InterposePhase()
    {
        //画面を押してオブジェクトに触れている指の数
        int nTouchObjNum = 0;
        GameObject[] ObjArray = new GameObject[10];          //当たってるオブジェクト格納用
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
                    if (pos.x == SecondTuchObj[v].transform.position.x) { 

                        if (InterposeObj[0] != null)
                        {

                            InterposeObj[0].GetComponent<Renderer>().material.color = Color.white;
                            InterposeObj[1].GetComponent<Renderer>().material.color = Color.white;
                        }
                    
                        //ゼロが左でイチが右
                        if (SecondTuchObj[v].transform.position.z < pos.z)
                        {
                            InterposeObj[0] = FarstTuchObj[i].transform.gameObject;
                            InterposeObj[1] = SecondTuchObj[v].transform.gameObject;

                            TouchStartPosLeft = InterposeObj[0].transform.position;
                            TouchStartPosRight = InterposeObj[1].transform.position;

                        }
                        else 
                        {
                            InterposeObj[1] = FarstTuchObj[i].transform.gameObject;
                            InterposeObj[0] = SecondTuchObj[v].transform.gameObject;

                            TouchStartPosLeft = InterposeObj[0].transform.position;
                            TouchStartPosRight = InterposeObj[1].transform.position;
                        }

                        InterposeObj[0].GetComponent<Renderer>().material.color = Color.red;
                        InterposeObj[1].GetComponent<Renderer>().material.color = Color.red;
                        break;
                    }
                    //斜め判定
                    if (pos.x + 3 == SecondTuchObj[v].transform.position.x || pos.x - 3 == SecondTuchObj[v].transform.position.x)
                    {
                        if (InterposeObj[0] != null)
                        {

                            InterposeObj[0].GetComponent<Renderer>().material.color = Color.white;
                            InterposeObj[1].GetComponent<Renderer>().material.color = Color.white;
                        }
                        //ゼロが左でイチが右
                        if (SecondTuchObj[v].transform.position.z < pos.z)
                        {
                            InterposeObj[0] = FarstTuchObj[i].transform.gameObject;
                            InterposeObj[1] = SecondTuchObj[v].transform.gameObject;

                            TouchStartPosLeft = InterposeObj[0].transform.position;
                            TouchStartPosRight = InterposeObj[1].transform.position;

                        }
                        else
                        {
                            InterposeObj[1] = FarstTuchObj[i].transform.gameObject;
                            InterposeObj[0] = SecondTuchObj[v].transform.gameObject;

                            TouchStartPosLeft = InterposeObj[0].transform.position;
                            TouchStartPosRight = InterposeObj[1].transform.position;
                        }

                        InterposeObj[0].GetComponent<Renderer>().material.color = Color.red;
                        InterposeObj[1].GetComponent<Renderer>().material.color = Color.red;
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
                            Vector2 pos = Camera.main.ScreenToWorldPoint(Input.touches[i].position);
                            GoalPos_Left = new Vector3(TouchStartPosLeft.z, pos.y, pos.x);
                            StartPos_Left = InterposeObj[0].transform.position;
                            nCorrect++;
                        }
                        if (Input.touches[i].position.x < 200)
                        {
                            Vector2 pos = Camera.main.ScreenToWorldPoint(Input.touches[i].position);
                            GoalPos_Right = new Vector3(TouchStartPosRight.z, pos.y, pos.x);
                            StartPos_Right = InterposeObj[0].transform.position;
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
                InterposeObj[0].GetComponent<PlayerScr>().PosZ(InterposeObj[1].transform.position,0);
                InterposeObj[1].GetComponent<PlayerScr>().PosZ(InterposeObj[0].transform.position,0);
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
                Destroy(InterposeObj[0]);
                Destroy(InterposeObj[1]);
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
            InterposeObj[0].GetComponent<PlayerScr>().PosZ(InterposeObj[1].transform.position, 0);
            InterposeObj[1].GetComponent<PlayerScr>().PosZ(InterposeObj[0].transform.position, 0);
        }


    }
    //バトルフェーズ
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

    }

    //タグ生成関数
    static void AddTag(string tagname)
    {
        UnityEngine.Object[] asset = AssetDatabase.LoadAllAssetsAtPath("ProjectSettings/TagManager.asset");
        if ((asset != null) && (asset.Length > 0))
        {
            SerializedObject so = new SerializedObject(asset[0]);
            SerializedProperty tags = so.FindProperty("tags");

            for (int i = 0; i < tags.arraySize; ++i)
            {
                if (tags.GetArrayElementAtIndex(i).stringValue == tagname)
                {
                    return;
                }
            }

            int index = tags.arraySize;
            tags.InsertArrayElementAtIndex(index);
            tags.GetArrayElementAtIndex(index).stringValue = tagname;
            so.ApplyModifiedProperties();
            so.Update();
        }
    }

    //フリックされた方向取得関数
    void GetDirection()
    {
    }

    public void TrunSkip()
    {
        turnstate++;
        if (turnstate > TurnState.nBattlePhase)
        {
            turnstate = TurnState.nNone;
            nTurn++;
            bFormEnd = false;
        }
    }

    public void PlayerMoveOk(GameObject obj)
    {
        MoveOkProvisional.Add(obj);
        nPlayerOkCount++;
        //PlayerMoveOkList.Add(obj);
    }

    public bool MoveDecision(GameObject obj)
    {
        //if (turnstate != TurnState.nMovePhase)
        //{
            if (MoveOkProvisional.Count >= 2)
            {
                MoveInitWaitObj.Add(obj);
                return false;
            }
            else if (nPlayerCount < 2)
            {
                nPlayerCount++;
                return true;
            }
        //}

        MoveInitWaitObj.Add(obj);
        return false;

    }

    public TurnState SetTurnState()
    {
        return turnstate;
    }

    public void DeleteStuck()
    {
        if(MoveInitWaitObj.Count >= 1)
        {
            for (int i = 0;i < MoveInitWaitObj.Count;i++)
            {
                Destroy(MoveInitWaitObj[i].gameObject);
            }

            while (MoveInitWaitObj.Count != 0)
            {
                MoveInitWaitObj.RemoveAt(0);
            }
        }



    }

}
