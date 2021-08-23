using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovementController : MonoBehaviour
{
    public Vector3 lastPos;
    public Animator animator;
    public SpriteRenderer sr;
    public Rigidbody2D rb;

    void Start()
    {
        animator = transform.GetChild(0).GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (lastPos != null)
        {
            sr.flipX = rb.velocity.x < -0.01f;
            animator.SetFloat("Distance", Mathf.Abs((transform.position - lastPos).magnitude));
        }
        lastPos = transform.position;
    }
}
