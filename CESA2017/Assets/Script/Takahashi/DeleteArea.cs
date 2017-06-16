using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeleteArea : MonoBehaviour {

    void OnCollisionEnter(Collision hit)
    {
        Destroy(hit.gameObject);
    }
}
