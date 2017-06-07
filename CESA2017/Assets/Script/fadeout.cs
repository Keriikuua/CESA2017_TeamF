using UnityEngine;
using System.Collections;
using UnityEngine.UI;

using System.Collections.Generic;

public class fadeout : MonoBehaviour
{
    public bool go;         //フェードアウト用
    public bool go2;        //オーディオ用
    

    public float alpha;        //透明度
    public float speed = 0.005f;  //フェードインする時間


    //AudioSource audioSource;
    //public List<AudioClip> audioClip = new List<AudioClip>();



    // Use this for initialization
    void Start()
    {
        alpha = 0;
        go = false;
        go2 = false;
        
        //audioSource = gameObject.AddComponent<AudioSource>();
    }


    // Update is called once per frame
    void Update()
    {
        if (alpha > 1)
            return;

        GetComponent<Image>().color = new Color(0, 0, 0, alpha);

        if (go)
        {
            alpha += speed;
        }

        if (go2)
        {

            //audioSource.PlayOneShot(audioClip[0]);
            go2 = false;

        }
    }
}