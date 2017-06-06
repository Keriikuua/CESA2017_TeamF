using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class AutoMold : MonoBehaviour {

    [SerializeField, Header("動く型")]
    Mold mold;

    [SerializeField, Header("型を落とした後に停止する時間")]
    Vector2 vRandomTime;
    
    bool bHit;
    float fStopTime;

    void Start()
    {
        bHit = false;
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

    // 当たった瞬間呼ばれる
    void OnTriggerStay(Collider hit)
    {
        if (!bHit)
        {
            mold.DownMold();
            bHit = true;
            fStopTime = UnityEngine.Random.Range(vRandomTime.x, vRandomTime.y);
            StartCoroutine(DelayMethod(fStopTime, () =>
            {
                bHit = false;
            }));
        }

        if (hit.gameObject.tag == "Elementary" && !bHit)
        {
        }
    }


}
