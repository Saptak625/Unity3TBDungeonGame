using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovementController : MonoBehaviour
{
    public Vector3 lastPos;
    public Animator animator;
    public SpriteRenderer sr;

    void Start()
    {
        animator = transform.GetChild(0).GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (lastPos != null)
        {
            Vector3 distance = transform.position - lastPos;
            sr.flipX = distance.x < -0.001f;
            animator.SetFloat("Distance", Mathf.Abs(distance.magnitude));
        }
        lastPos = transform.position;
    }
}
