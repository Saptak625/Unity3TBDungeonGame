using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovementController : MonoBehaviour
{
    public GameObject player;
    public Vector3 lastPos;
    public Animator animator;
    public Transform t;

    void Start()
    {
        animator = transform.GetChild(0).GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (lastPos != null)
        {
            
            t.eulerAngles = new Vector3(0, ((player.transform.position.x - t.position.x) < -0f ? 180: 0), 0);
            Vector3 distance = transform.position - lastPos;
            animator.SetFloat("Distance", Mathf.Abs(distance.magnitude));
        }
        lastPos = transform.position;
    }
}
