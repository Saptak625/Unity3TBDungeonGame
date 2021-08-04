using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
<<<<<<< Updated upstream
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
=======
    public float damage;
    public string primaryTarget;
    public bool isBouncing = false;
    public int bouncesLeft = 0;
<<<<<<< Updated upstream
=======
    public GameObject thisProjectile;

    void Start()
    {
        if (primaryTarget == "Enemy Container")
        {
            thisProjectile.layer = 8;
        }
    }
>>>>>>> Stashed changes

    void OnCollisionEnter2D(Collision2D collision)
    {
        Collider2D collider = collision.collider;
        if (collider.tag == primaryTarget)
        {
            if (primaryTarget == "Player")
            {
                collider.gameObject.GetComponent<PlayerController>().takeDamage(this.damage);
            }
            else
            {
                collider.gameObject.GetComponent<EnemyController>().takeDamage(this.damage);
            }
            this.destroy();
        }
        else if (collider.tag == "DestructableWall")
        {
            collider.gameObject.GetComponent<DestructableWall>().takeDamage(this.damage);
            this.destroy();
<<<<<<< Updated upstream
        }
        else if (collider.tag == "Wall")
        {
            if (isBouncing)
            {
                bouncesLeft--;
            }
            else
            {
                this.destroy();
            }
        }
=======
        }
        else if (collider.tag == "Wall")
        {
            if (isBouncing)
            {
                bouncesLeft--;
            }
            else
            {
                this.destroy();
            }
        }
>>>>>>> Stashed changes
        if (bouncesLeft <= 0)
        {
            this.destroy();
        }
    }

    void destroy()
    {
        //Add bullet impact effects if needed.
        Destroy(gameObject); //Destroy Bullet afterwards
>>>>>>> Stashed changes
    }
}