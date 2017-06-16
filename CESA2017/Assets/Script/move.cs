using UnityEngine;
using System.Collections;

public class move : MonoBehaviour
{
    Rigidbody2D image;
    public float speed = 0;
   // public float cnt =0;

   // public GameObject titlelogo3;

    void Start()
    {
        image = GetComponent<Rigidbody2D>();

    //    Vector3 tmp = GameObject.Find("hogehoge").transform.position;
      //  GameObject.Find("hogehoge").transform.position = new Vector3(tmp.x + 100, tmp.y, tmp.z);
    }
 
    void Update()
    {

        
        image.velocity = new Vector2(speed, image.velocity.y);
        //cnt++;

        //if (cnt == 1000)
        //{
           // speed = speed * -1;
           
            
        //}
      
    }

    
    void OnCollisionEnter2D(Collision2D collision)
    {
        speed = speed * 0;
        //speed = speed * -1;
       // cnt=700;
       // Destroy(collision.gameObject);
    }
 
}