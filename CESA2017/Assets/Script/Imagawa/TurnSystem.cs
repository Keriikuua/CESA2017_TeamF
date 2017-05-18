using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnSystem : MonoBehaviour {
    //
    public enum TurnState : int{
        nNone = 0,
        nEnemyForm,         //敵生成
        nMovePhase,         //移動
        nInterposePhase,    //挟む
        nBattlePhase        //バトル
    }
    public TurnState turnstate;
    
    int nTurnEnd;               //ターン終了待ち用変数

    //移動可能オブジェ格納用リスト
    public List<GameObject> PlayerMoveOkList = new List<GameObject>();              //プレイヤーの移動可能オブジェ
    private List<GameObject> EnemyMoveOkList = new List<GameObject>();              //敵の移動可能オブジェ
    List<GameObject> MoveOkProvisional = new List<GameObject>();                    //左右で移動待ちのオブジェ
    List<GameObject> MoveInitWaitObj = new List<GameObject>();                      //初期地点で沸くの待ちオブジェ
    int nPlayerOkCount;
    int nEnemyOkCount;
    int nPlayerCount = 0;                                                           //MoveDecisionで使う

    //ステージデータ格納
    StageForm stageform;
    List<string[]> StageEnemyData = new List<string[]>();
    int nEvacuate = 0;
    bool bFormEnd;

    int nTurn;              //現在のターン数

    Character character;
    CharaList charalist;        //生成されてるキャラの参照のため

    public GameObject[] InterposeObj = new GameObject[2];      //挟めるオブジェクト格納用
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


    void Start(){
        turnstate = TurnState.nNone;
        nPlayerOkCount = 0;
        nEnemyOkCount = 0;
        nTurnEnd = 0;
        bFormEnd = false;
        nTurn = 0;
        nCorrect = 0;

        character = GameObject.Find("Chara").GetComponent<Character>();
        charalist = GameObject.Find("Chara").GetComponent<CharaList>();
        stageform = GetComponent<StageForm>();
        StageEnemyData = stageform.StageData_Enemy();
    }
    

    void Update(){
        //ターン処理
        switch (turnstate){
            //
            case TurnState.nNone:
            break;

            //敵生成フェーズ
            case TurnState.nEnemyForm:
                nEvacuate = int.Parse(StageEnemyData[nTurn][0]);
                if (nEvacuate == 0 && !bFormEnd){
                    EnemyMoveOkList.Add(character.EnemyForm(0));
                    bFormEnd = true;
                }
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
                //BattlePhase();
            break;
        }

        //デバッグコマンド
        if (Input.GetKeyDown(KeyCode.Return)){
            turnstate++;
            nTurnEnd = 0;
            if (turnstate > TurnState.nBattlePhase){
                turnstate = TurnState.nNone;
                nTurn++;
                bFormEnd = false;
            }
        }

    }


    //移動フェーズ
    void MovePhase(){
        if (nTurnEnd == 0){
            if (nPlayerOkCount >= 2){
                    PlayerMoveOkList.Add(MoveOkProvisional[0]);
                    PlayerMoveOkList.Add(MoveOkProvisional[1]);
                    MoveOkProvisional.RemoveAt(0);
                    MoveOkProvisional.RemoveAt(0);
                    nPlayerCount = 0;
                nPlayerOkCount = 0;
            }

            if (nPlayerOkCount == 0){
                if (MoveInitWaitObj.Count >= 2){
                    MoveInitWaitObj[0].GetComponent<PlayerScr>().WaitClear();
                    MoveInitWaitObj[1].GetComponent<PlayerScr>().WaitClear();
                    MoveInitWaitObj.RemoveAt(0);
                    MoveInitWaitObj.RemoveAt(0);
                }
            }

            //プレイヤーの移動
            for (int i = 0; i < PlayerMoveOkList.Count; i++){
                PlayerMoveOkList[i].transform.position += new Vector3(-1, 0,0);
            }
            //敵キャラの移動
            for(int i = 0;i < EnemyMoveOkList.Count; i++){
                EnemyMoveOkList[i].transform.position += new Vector3(1, 0, 0);
            }
            nTurnEnd++;
        }else{
            if (Input.GetKeyDown(KeyCode.L)){
                nTurnEnd = 0;
                turnstate++;
            }
        }



    }

    //挟むフェーズ
    void InterposePhase()
    {
        int nNum = 0;           //画面を押してオブジェクトに触れている指の数
        GameObject[] ObjArray = new GameObject[5];          //当たってるオブジェクト格納用
        for (int i = 0; i < Input.touchCount; i++)
        {
            if (Input.touches[i].phase == TouchPhase.Began)
            {
                var ray = Camera.main.ScreenPointToRay(Input.touches[i].position);
                RaycastHit hit;
                if (Physics.SphereCast(ray,2.0f, out hit, 50.0f))
                {
                    GameObject obj = hit.transform.gameObject;
                    //当たったオブジェクトがキャラリストにあるかチェック
                    for (int v = 0; v < charalist.PlayerList.Count; v++)
                    {
                        if (obj == charalist.PlayerList[v])
                        {
                            ObjArray[nNum] = obj;
                            //ヒットしていたらカウント増やす
                            nNum++;
                        }
                    }
                }
            }
        }
        //当たったオブジェクトが二つ以上なら挟めるオブジェクトか判定      
        for (int i = 0; i < nNum; i++)
        {
            Vector3 pos = ObjArray[i].transform.position;
            for (int v = 0; v < nNum; v++)
            {
                if (pos.z != ObjArray[v].transform.position.z)
                {
                    if (pos.y - 1 <= ObjArray[v].transform.position.y && pos.y + 1 >= ObjArray[v].transform.position.y)
                    {
                        InterposeObj[0].GetComponent<Renderer>().material.color = Color.white;
                        InterposeObj[1].GetComponent<Renderer>().material.color = Color.white;
                        //ゼロが左でイチが右
                        if (ObjArray[i].transform.position.x < -1)
                        {
                            InterposeObj[0] = ObjArray[i].transform.gameObject;
                            InterposeObj[1] = ObjArray[v].transform.gameObject;
                            
                        }
                        if (ObjArray[i].transform.position.x > 1)
                        {
                            InterposeObj[1] = ObjArray[i].transform.gameObject;
                            InterposeObj[0] = ObjArray[v].transform.gameObject;
                        }
                        TouchStartPosLeft = InterposeObj[0].transform.position;
                        TouchStartPosRight = InterposeObj[1].transform.position;
                        InterposeObj[0].GetComponent<Renderer>().material.color = Color.red;
                        InterposeObj[1].GetComponent<Renderer>().material.color = Color.red;
                    }
                }
            }
        }
        //-----------------------------------------------------------------------------------------------------------  
        if (Input.touchCount >= 2)
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

        if(nCorrect == 2)
        {
            if (!bDestoryFlg){
                InterposeObj[0].GetComponent<PlayerScr>().PosZ();
                InterposeObj[1].GetComponent<PlayerScr>().PosZ();
                bDestoryFlg = true;
            }
            if (nowTime < endTime)
            {

                nowTime += Time.deltaTime;
            }else
            {
                turnstate++;
                for(int i = 0;i < EnemyMoveOkList.Count; i++)
                {
                    if(EnemyMoveOkList[i].transform.position.x == InterposeObj[0].transform.position.x)
                    {
                        BattlePhase(InterposeObj[0], InterposeObj[1], EnemyMoveOkList[i]);
                    }
                }
                InterposeObj[0] = null;
                InterposeObj[1] = null;
                nCorrect = 0;
                nowTime = 0;
                bDestoryFlg = false;
            }
        }
        else
        {
            nCorrect = 0;
        }

        //デバッグコマンド
        if (Input.GetKeyDown(KeyCode.L))
        {
            if (!bDestoryFlg)
            {
                PlayerMoveOkList[0].GetComponent<PlayerScr>().PosZ();
                PlayerMoveOkList[1].GetComponent<PlayerScr>().PosZ();
                bDestoryFlg = true;
            }

                turnstate++;
                for (int i = 0; i < EnemyMoveOkList.Count; i++)
                {
                    if (EnemyMoveOkList[i].transform.position.x == PlayerMoveOkList[0].transform.position.x)
                    {
                        BattlePhase(PlayerMoveOkList[0], PlayerMoveOkList[1], EnemyMoveOkList[i]);
                    }
                }

            //Destroy(InterposeObj[0].transform.gameObject);
            //Destroy(InterposeObj[1].transform.gameObject);
            InterposeObj[0] = null;
                InterposeObj[1] = null;
                nCorrect = 0;
                nowTime = 0;
                bDestoryFlg = false;
        }


    }
    //バトルフェーズ
    void BattlePhase(GameObject pObj1,GameObject pObj2,GameObject eObj){
        PlayerScr pObj1Scr = pObj1.GetComponent<PlayerScr>();
        PlayerScr pObj2Scr = pObj2.GetComponent<PlayerScr>();
        EnemyScr eObjScr = eObj.GetComponent<EnemyScr>();
        int nPlayerAttack = pObj1Scr.nAttack + pObj2Scr.nAttack;
        int nEnemyHp = eObjScr.nHp;

        nEnemyHp -= nPlayerAttack;
        Debug.Log(nEnemyHp);
        if(nEnemyHp <= 0)
        {
            for(int i = 0;i < EnemyMoveOkList.Count; i++)
            {
                if(EnemyMoveOkList[i] == eObj)
                {
                    EnemyMoveOkList.RemoveAt(i);
                }
            }
            Destroy(eObj);
        }
        //PlayerMoveOkListから消すオブジェクトを削除する
        for (int i = 0; i < PlayerMoveOkList.Count; i++){
            if (PlayerMoveOkList[i].gameObject == pObj1){
                PlayerMoveOkList.RemoveAt(i);
            }
        }

        for (int i = 0; i < PlayerMoveOkList.Count; i++){
            if (PlayerMoveOkList[i].gameObject == pObj2){
                PlayerMoveOkList.RemoveAt(i);
            }
        }
        Destroy(pObj1);
        Destroy(pObj2);

    }

    //フリックされた方向取得関数
    void GetDirection()
    {
    }

    public void TrunSkip(){
        turnstate++;
        nTurnEnd = 0;
        if (turnstate > TurnState.nBattlePhase){
            turnstate = TurnState.nNone;
            nTurn++;
            bFormEnd = false;
        }
    }

    public void PlayerMoveOk(GameObject obj){
        MoveOkProvisional.Add(obj);
        nPlayerOkCount++;
        //PlayerMoveOkList.Add(obj);
    }

    public bool MoveDecision(GameObject obj)
    {
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

        MoveInitWaitObj.Add(obj);
        return false;

    }

    public TurnState SetTurnState()
    {
        return turnstate;
    }

}
