using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public Enemy enemy = null; //Enemy virtual state
    public GameObject player; //Reference to player GameObject
    public Rigidbody2D rb; //Used to control enemy movement

    // Start is called before the first frame update
    void Start()
    {
        

    }

    // Update is called once per frame
    void FixedUpdate()
    {
       if(enemy != null) //Enemy Virtual state has been created.
       {
            //Do logic to determine attack
            if(enemy.attackType == EnemyAttack.Melee && !(enemy.enemyType == (EnemyType) 5 || enemy.enemyType == (EnemyType) 6 || enemy.enemyType == (EnemyType) 8 || enemy.enemyType == (EnemyType) 9))
            {
                this.classicMelee();
            }
       } 
    }

    void classicMelee()
    {
        if ((player.transform.position - transform.position).magnitude >= 1.2f)
        {
            //Vector Math for player honing
            Vector3 unitVector = player.transform.position - transform.position;
            Vector3.Normalize(unitVector);
            rb.velocity = new Vector2(unitVector.x, unitVector.y) * enemy.speed;
        }
        else
        {
            //Attack player
        }
    }
}
