using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public float damage;
    public float speed;
    public string primaryTarget;
    public Vector3 direction;
    public bool isBouncing = false;
    public int numberOfTouchesRemaining = 0;

    void FixedUpdate()
    {
        transform.position += this.direction * this.speed * Time.deltaTime;
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        Collider2D collider = collision.collider;
        if (collider.CompareTag(primaryTarget))
        {
            if (primaryTarget == "Player")
            {
                collider.gameObject.GetComponent<PlayerController>().takeDamage(this.damage);
            }
            else
            {
                EnemyController e = collider.gameObject.transform.GetChild(0).gameObject.GetComponent<EnemyController>();
                if (e != null)
                {
                    e.takeDamage(this.damage);
                }
            }
            this.destroy();
        }
        else if (collider.CompareTag("DestructableWall"))
        {
            collider.gameObject.GetComponent<DestructableWall>().takeDamage(this.damage);
            this.destroy();
        }
        else if (collider.CompareTag("Wall"))
        {
            if (this.isBouncing)
            {
                this.direction = Vector2.Reflect(this.direction, collision.contacts[0].normal);
                transform.rotation = Quaternion.LookRotation(Vector3.forward, this.direction);
                this.numberOfTouchesRemaining--;
            }
        }

        if (numberOfTouchesRemaining == 0) //Destroy once no more touches. Regular destroys instantly, while bouncing permits some bounces.
        {
            this.destroy();
        }
    }

    void destroy()
    {
        //Add bullet impact effects if needed.
        Destroy(gameObject); //Destroy Bullet afterwards
    }
}
