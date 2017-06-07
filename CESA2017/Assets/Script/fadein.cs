using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class fadein : MonoBehaviour
{
    public float alpha;        //透明度
    public float speed = 0.005f;  //フェードインする時間


    // Use this for initialization
    void Start()
    {
        alpha = 1;
    }


    // Update is called once per frame
    void Update()
    {
        if (alpha < 0)
            return;

        GetComponent<Image>().color = new Color(0, 0, 0, alpha);

        alpha -= speed;

    }
}