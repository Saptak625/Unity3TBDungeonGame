using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DetectTrigger : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        gameObject.SendMessageUpwards("enteredDungeon", gameObject);
    }
}
