using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

// 型クラス
public class Mold : MonoBehaviour {

    // 
    public enum MoldMode
    {
        None = 0,
        Down,
        Stop,
        Up,
    }

    [SerializeField,Header("型のタイプ")]
    Type.Chara MoldType;

    [SerializeField, Header("型の落下速度")]
    float fSpeed;

    [SerializeField, Header("中心")]
    GameObject obj;

    [SerializeField,Header("対応キー")]
    string Botton;

    [SerializeField, Header("BAD判定ゾーン"),Range(0.0f,0.25f)]
    float BadZone;

    [SerializeField, Header("GOOD判定ゾーン"), Range(0.0f, 0.25f)]
    float GoodZone;

    [SerializeField, Header("フィーバーゲージ")]
    Fever fever;

    [SerializeField, Header("キャラ生成用")]
    Character chara;

    [SerializeField, Header("型で挟んで止まる時間"), Range(0.1f,1.0f)]
    float fStopTime = 0.5f;

    [SerializeField, Header("パーティクルスクリプト")]
    MoldParticle particle;

    private Vector3 InitPos;        // 初期位置
    private MoldMode mode;          // 型の遷移状態
    private float BestZone;         // Best判定ゾーン
    private BoxCollider Collider;   // 型のCollider
    private float distance = 100f;  // 距離

    // Use this for initialization
    void Start () {
        // 変数初期化
        mode = MoldMode.None;
        InitPos = transform.position;
        Collider = transform.GetComponent<BoxCollider>();
        BadZone = BadZone * Collider.size.x;
        GoodZone = GoodZone * Collider.size.x;
        BestZone = Collider.size.x - (BadZone + GoodZone);
    }
	
	// Update is called once per frame
	void Update ()
    {
        switch (mode)
        {
            case MoldMode.None:
                // キーで挟む
                if (Input.GetKeyDown(Botton))
                    mode = MoldMode.Down;

                // 左クリックを取得
                if (Input.GetMouseButtonDown(0))
                {
                    // クリックしたスクリーン座標をrayに変換
                    Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                    // Rayの当たったオブジェクトの情報を格納する
                    RaycastHit hit = new RaycastHit();
                    // オブジェクトにrayが当たった時
                    if (Physics.Raycast(ray, out hit, distance))
                    {
                        // rayが当たったオブジェクトの名前を取得
                        string objectName = hit.collider.gameObject.name;
                        if(objectName == gameObject.name)
                            mode = MoldMode.Down;
                    }
                }
                break;

            case MoldMode.Down:
                if (transform.position.y > obj.transform.position.y + (transform.localScale.y * 1.1f))
                    transform.position -= new Vector3(0, fSpeed, 0);
                else
                    mode = MoldMode.Stop;
                break;

            case MoldMode.Stop:
                // 落とした型を止める
                StartCoroutine(DelayMethod(fStopTime, () =>
                {
                    mode = MoldMode.Up;
                }));
                break;

            case MoldMode.Up:

                if (transform.position.y < InitPos.y)
                    transform.position += new Vector3(0, fSpeed/10.0f, 0);
                else
                    mode = MoldMode.None;

                break;
        }
    }

    // 型と素材の当たり判定
    public int HitCollider(Transform hitObj)
    {
        if (fever.GetFeverMode())   // Fever中なら
        {
            fever.PlusGauge(2);
            return 1;
        }

        // 格納変数
        float PosX = transform.position.x;
        float ScaleX = Collider.size.x;
        float HitZone = Mathf.Abs(hitObj.position.x - PosX);

        int nBest = 2;
        int nGood = 1;
        int nBad = 0;

        // Best判定
        if ((ScaleX - BadZone) - GoodZone > HitZone && (ScaleX - BadZone) - GoodZone - BestZone < HitZone)
        {
            //Debug.Log("best");
            particle.PlayParticle(nBest);
            fever.PlusGauge(nBest);
            chara.CreatePlayer(MoldType, 1);
            return 1;
        }

        // Good判定
        if ((ScaleX - BadZone) > HitZone && (ScaleX - BadZone) - GoodZone <HitZone)
        {
            //Debug.Log("good");
            particle.PlayParticle(nGood);
            fever.PlusGauge(nGood);
            chara.CreatePlayer(MoldType, 2);
            return 2;
        }

        // Bad判定
        //if (ScaleX > HitZone && (ScaleX - BadZone) < HitZone)
        //{
        //Debug.Log("bad");
        particle.PlayParticle(nBad);
        fever.PlusGauge(nBad);
        chara.CreatePlayer(MoldType, 3);
        return 3;
        //}
        //return 0;
    }

    // 型のタイプ取得
    public Type.Chara GetType()
    {
        return MoldType;
    }
    
    /// <summary>
    /// 渡された処理を指定時間後に実行する
    /// </summary>
    /// <param name="waitTime">遅延時間[ミリ秒]</param>
    /// <param name="action">実行したい処理</param>
    /// <returns></returns>
    private IEnumerator DelayMethod(float waitTime, Action action)
    {
        yield return new WaitForSeconds(waitTime);
        action();
    }
}
