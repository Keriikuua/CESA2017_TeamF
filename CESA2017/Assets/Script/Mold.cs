using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    private Vector3 InitPos;
    private MoldMode mode;
    private float BestZone;
    private BoxCollider Collider;
    private float distance = 100f;

    // Use this for initialization
    void Start () {
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
                if (transform.position.y > obj.transform.position.y + (transform.localScale.y * 1.5f))
                    transform.position -= new Vector3(0, fSpeed, 0);
                else
                    mode = MoldMode.Up;
                break;

            case MoldMode.Stop:
                break;

            case MoldMode.Up:

                if (transform.position.y < InitPos.y)
                    transform.position += new Vector3(0, fSpeed/10.0f, 0);
                else
                    mode = MoldMode.None;

                break;
        }
    }

    public int HitCollider(Transform hitObj)
    {
        if (fever.GetFeverMode())   // Fever中なら
        {
            fever.PlusGauge(2);
            return 1;
        }

        float PosX = transform.position.x;
        float ScaleX = Collider.size.x;
        float HitZone = Mathf.Abs(hitObj.position.x - PosX);

        // Best判定
        if ((ScaleX - BadZone) - GoodZone > HitZone && (ScaleX - BadZone) - GoodZone - BestZone < HitZone)
        {
            Debug.Log("best");
            fever.PlusGauge(2);
            chara.CreatePlayer(MoldType, 1);
            return 1;
        }

        // Good判定
        if ((ScaleX - BadZone) > HitZone && (ScaleX - BadZone) - GoodZone <HitZone)
        {
            Debug.Log("good");
            fever.PlusGauge(1);
            chara.CreatePlayer(MoldType, 2);
            return 2;
        }

        // Bad判定
        //if (ScaleX > HitZone && (ScaleX - BadZone) < HitZone)
        //{
        Debug.Log("bad");
        fever.PlusGauge(0);
        chara.CreatePlayer(MoldType, 3);
        return 3;
        //}
        //return 0;
    }

    public Type.Chara GetType()
    {
        return MoldType;
    }
}
