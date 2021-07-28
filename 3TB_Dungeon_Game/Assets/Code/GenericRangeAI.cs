using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class GenericRangeAI : MonoBehaviour
{
    public Transform target;

    public float speed = 200f;
    public float nextWaypointDistance = 3f;

    Path path = null;
    int currentWayPoint = 0;
    bool reachedEndOfPath = true;
    bool canShoot;

    Seeker seeker;
    Rigidbody2D rb;

    // Start is called before the first frame update
    void Start()
    {
        this.seeker = GetComponent<Seeker>();
        this.rb = GetComponent<Rigidbody2D>();
        InvokeRepeating("generatePath", 0f, 0.5f);
    }

    void generatePath()
    {
        if (seeker.IsDone())
            this.seeker.StartPath(rb.position, target.position, OnPathComplete);
    }

    void OnPathComplete(Path p)
    {
        if (!p.error)
        {
            this.path = p;
            this.currentWayPoint = 0;
            reachedEndOfPath = false;
        }
    }

    void checkIfCanShoot()
    {
        //Detect all wall hits
        RaycastHit2D[] hits = Physics2D.LinecastAll(this.rb.position, new Vector2(this.target.position.x, this.target.position.y));
        this.canShoot = true;
        foreach (RaycastHit2D r in hits)
        {
            if (r.collider.tag == "Wall")
            {
                this.canShoot = false;
                break;
            }
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (!this.canShoot) //Need to move to new position to fire effectively
        {
            //Iterate through waypoints
            if (path == null || reachedEndOfPath)
                return;

            if(this.currentWayPoint >= path.vectorPath.Count)
            {
                reachedEndOfPath = true;
                return;
            }
            else
            {
                reachedEndOfPath = false;
            }

            Vector2 direction = ((Vector2) path.vectorPath[currentWayPoint] - rb.position).normalized;

            rb.AddForce(direction * speed * Time.deltaTime);

            float distance = Vector2.Distance(rb.position, path.vectorPath[currentWayPoint]);

            if (distance < nextWaypointDistance)
            {
                currentWayPoint++;
            }
        }
        this.checkIfCanShoot();
    }
}
