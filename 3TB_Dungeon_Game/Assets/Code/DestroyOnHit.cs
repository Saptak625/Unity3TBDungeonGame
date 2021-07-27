using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyOnHit : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        //Testing Purposes
        Invoke("OnTriggerEnter2D", 5f);

    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        gameObject.SendMessageUpwards("reloadAStarGrid");
        Destroy(gameObject);
    }
}
