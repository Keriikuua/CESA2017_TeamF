using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Fever : MonoBehaviour {

    [SerializeField, Header("フィーバーゲージUI")]
    Slider Gauge;

    [SerializeField, Header("")]
    float fGoodPlusValue;
    [SerializeField, Header("")]
    float fBadPlusValue;
    [SerializeField, Header("")]
    float fBestPlusValue;

    [SerializeField, Header("")]
    float fMinusValue;

    private bool bFever;
    private float fGauge;

    // Use this for initialization
    void Start () {
        Gauge.value = 0;
        bFever = false;
        fGauge = Gauge.value; ;
    }

    // Update is called once per frame
    void Update()
    {
        if (!bFever && Gauge.value >= Gauge.maxValue)   // フィーバートリガー
        {
            bFever = true;
        }

        if (bFever) // フィーバー中
        {
            MinusGauge();
            if (fGauge <= Gauge.minValue)
                bFever = false;
        }

        if (fGauge != Gauge.value)
        {
            float f = (fGauge - Gauge.value) / 10;

            if (f <= 0.00001f)
            {
                Gauge.value = fGauge;
            }

            Gauge.value += f;
        }
    }

    public void PlusGauge(int nSwitch)
    {
        if (bFever)
            return;

        switch (nSwitch)
        {
            case 0: // Bad
                fGauge += fBadPlusValue;
                break;
            case 1: // Good
                fGauge += fGoodPlusValue;
                break;
            case 2: // Best
                fGauge += fBestPlusValue;
                break;
        }

        if (fGauge > 1.0f)
            fGauge = 1.0f;
    }

    void MinusGauge()
    {
        fGauge -= fMinusValue;
        if (fGauge < 0.0f)
            fGauge = 0.0f;
    }

    public bool GetFeverMode()
    {
        return bFever;
    }
}
