using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public Enemy enemy = null; //Enemy virtual state
    public GameObject player; //Reference to player GameObject
    public int attackCooldown = 0;
    public GameObject enemyContainer;

    // Start is called before the first frame update
    void Start()
    {
        this.enemyContainer=gameObject.transform.parent.gameObject; //Set parent container to grab attributes

    }

    // Update is called once per frame
    void FixedUpdate()
    {
       if(enemy != null) //Enemy Virtual state has been created.
       {
            //Do logic to determine attack
            if (attackCooldown == enemy.cooldown)
            {
                if (enemy.attackType == EnemyAttack.Melee && !(enemy.enemyType == (EnemyType)5 || enemy.enemyType == (EnemyType)6 || enemy.enemyType == (EnemyType)8 || enemy.enemyType == (EnemyType)9))
                {
                    this.classicMelee();
                }
                else if (enemy.attackType == EnemyAttack.Range && !(enemy.enemyType == (EnemyType)2 || enemy.enemyType == (EnemyType)3 || enemy.enemyType == (EnemyType)4 || enemy.enemyType == (EnemyType)6))
                {
                    this.classicRange();
                }
            }
            else
            {
                attackCooldown+=(attackCooldown < enemy.cooldown? 1: 0); //Recharging attack to max
            }
            
            
       } 
    }

    void classicMelee()
    {
        if ((player.transform.position - transform.position).magnitude < 1.2f) //Change trigger to reflect AI
        {
            //Attack player
            attackCooldown = 0; //Reset enemy cooldown to prevent constant attacking.
        }
    }

    void classicRange()
    {
        if (this.enemyContainer.GetComponent<GenericRangeAI>().canShoot) //Once in position then shoot
        {
            //Create new enemy projectile towards player

            attackCooldown = 0; //Reset enemy cooldown to prevent constant attacking.
        }
        
    }
}
