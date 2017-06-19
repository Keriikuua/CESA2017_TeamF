using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuOpen : MonoBehaviour {

    private bool bOpen;
    public Image MenuImage;
    public Image BottonImage;

    [SerializeField, Header("大きくなる速度"), Range(0.01f, 0.2f)]
    private float fZoomSpeed;

    [SerializeField, Header("ボタンが落ちる速度")]
    private float fBottonSpeed;

    [SerializeField, Header("text")]
    private GameObject PauseText;

    [SerializeField, Header("Pause")]
    Pause pause;

    [SerializeField, Header("Convair")]
    Convair convair;

    // Use this for initialization
    void Start () {
        bOpen = false;
        PauseText.SetActive(false);

        if (MenuImage != null)
        {
            MenuImage.rectTransform.localScale = new Vector3(1.0f,0.0f,1.0f);
        }
    }
	
	// Update is called once per frame
	void Update () {
        if (bOpen)
        {
            if (MenuImage.rectTransform.localScale.y < 0.90f)
            {
                MenuImage.rectTransform.localScale += new Vector3(0.0f, fZoomSpeed, 0.0f);
                BottonImage.rectTransform.localPosition -= new Vector3(0.0f, fZoomSpeed * fBottonSpeed, 0.0f);
            }
            else
            {
                MenuImage.rectTransform.localScale = new Vector3(1.0f, 0.90f, 1.0f);    
            }
        }
        else
        {
            if (MenuImage.rectTransform.localScale.y > 0.0f)
            {
                MenuImage.rectTransform.localScale -= new Vector3(0.0f, fZoomSpeed, 0.0f);
                BottonImage.rectTransform.localPosition += new Vector3(0.0f, fZoomSpeed * fBottonSpeed, 0.0f);
            }
            else
                MenuImage.rectTransform.localScale = new Vector3(1.0f, 0.0f, 1.0f);
        }
	}

    // メニューを開く
    public void OpenMenu()
    {
        bOpen = !bOpen;
        PauseText.SetActive(bOpen);
        pause.pausing = bOpen;
        convair.ConvairSwitch();
    }
}
