using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public float damage;
    public string primaryTarget;
    
    void OnCollisionEnter2D(Collision2D collision)
    {
        Collider2D collider = collision.collider;
        if(collider.tag == primaryTarget)
        {
            if (primaryTarget == "Player")
            {
                collider.gameObject.GetComponent<PlayerController>().takeDamage(this.damage);
            }
            else
            {
                collider.gameObject.transform.GetChild(0).gameObject.GetComponent<EnemyController>().takeDamage(this.damage);
            }
        }else if(collider.tag == "DestructableWall")
        {
            collider.gameObject.GetComponent<DestructableWall>().takeDamage(this.damage);
        }
        //Add bullet impact effects if needed.
        Destroy(gameObject); //Destroy Bullet afterwards
    }
}
