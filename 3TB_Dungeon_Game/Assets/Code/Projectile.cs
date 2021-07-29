using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public int damage; //Default damage
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
                collider.gameObject.GetComponent<EnemyController>().takeDamage(this.damage);
            }
        }else if(collider.tag == "DestructableWall")
        {
            Destroy(collider.gameObject); //Destroy Wall
            //Call Roomloader reloadAStarGrid
            gameObject.SendMessageUpwards("reloadAStarGrid", gameObject);
        }
        //Add bullet impact effects if needed.
        Destroy(gameObject); //Destroy Bullet afterwards
    }
}
