using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public Enemy enemy = null; //Enemy virtual state
    public GameObject player; //Reference to player GameObject
    public int attackCooldown = 0;
    public int attackDurationRemaining = 0;
    public GameObject genericProjectile;
    public GameObject enemyContainer;
    public GameObject roomLoaderObject;

    // Start is called before the first frame update
    void Start()
    {
        this.enemyContainer = gameObject.transform.parent.gameObject; //Set parent container to grab attributes
    }

    // Update is called once per frame
    void FixedUpdate()
    {
       if(enemy != null) //Enemy Virtual state has been created.
       {
            if (enemy.alive)
            {
                if (attackDurationRemaining == 0)
                {
                    //Do logic to determine attack
                    if (attackCooldown == enemy.cooldown)
                    {
                        if ((enemy.attackType == EnemyAttack.Melee && !(enemy.enemyType == (EnemyType)5 || enemy.enemyType == (EnemyType)6 || enemy.enemyType == (EnemyType)8 || enemy.enemyType == (EnemyType)9)) || enemy.attackType == EnemyAttack.Mini)
                        {
                            this.classicMelee();
                        }
                        else if (enemy.attackType == EnemyAttack.Range && !(enemy.enemyType == (EnemyType)2 || enemy.enemyType == (EnemyType)3 || enemy.enemyType == (EnemyType)4 || enemy.enemyType == (EnemyType)6))
                        {
                            this.classicRange();
                        }
                        else if (enemy.attackType == EnemyAttack.Mage && (enemy.enemyType == (EnemyType)1 || enemy.enemyType == (EnemyType)2 || enemy.enemyType == (EnemyType)5 || enemy.enemyType == (EnemyType)8 || enemy.enemyType == (EnemyType)9))
                        {
                            this.spawnMiniMelee();
                        }
                        else if (enemy.attackType == EnemyAttack.Mage && enemy.enemyType == (EnemyType)3)
                        {
                            this.rangeHood();
                        }else if (enemy.attackType == EnemyAttack.Mage && enemy.enemyType == (EnemyType)4)
                        {
                            this.bouncingBullet();
                        }
                    }
                    else
                    {
                        attackCooldown += (attackCooldown < enemy.cooldown ? 1 : 0); //Recharging attack to max
                    }
                }
                else
                {
                    //Only attack that has duration is range hood
                    this.rangeHood();
                    this.attackDurationRemaining--;
                }
            }
       } 
    }

    void classicMelee()
    {
        if ((this.player.transform.position - this.enemyContainer.transform.position).magnitude < 1.2f)
        {
            //Attack player
            player.GetComponent<PlayerController>().takeDamage(this.enemy.attackDamage);
            attackCooldown = 0; //Reset enemy cooldown to prevent constant attacking.
        }
    }

    void classicRange()
    {
        if (this.enemyContainer.GetComponent<GenericRangeAI>().canShoot) //Once in position then shoot
        {
            //Create new enemy projectile towards player
            Vector2 lookDir = player.transform.position - transform.position;
            GameObject projectile = Instantiate(genericProjectile, new Vector3(transform.position.x, transform.position.y, transform.position.z+1), Quaternion.LookRotation(Vector3.forward, lookDir), gameObject.transform);
            //projectile.GetComponent<Rigidbody2D>().AddForce(projectile.transform.up*this.enemy.projectileSpeed, ForceMode2D.Impulse);
            Projectile projectileController = projectile.GetComponent<Projectile>();
            projectileController.primaryTarget = "Player";
            projectileController.damage = this.enemy.attackDamage;
            projectileController.speed = this.enemy.projectileSpeed;
            projectileController.direction = projectile.transform.up;
            attackCooldown = 0; //Reset enemy cooldown to prevent constant attacking.
        }
        
    }
    
    void bouncingBullet()
    {
        if (this.enemyContainer.GetComponent<GenericRangeAI>().canShoot) //Once in position then shoot
        {
            //Create new enemy projectile towards player
            Vector2 lookDir = player.transform.position - transform.position;
            GameObject projectile = Instantiate(genericProjectile, new Vector3(transform.position.x, transform.position.y, transform.position.z+1), Quaternion.LookRotation(Vector3.forward, lookDir), gameObject.transform);
            //projectile.GetComponent<Rigidbody2D>().AddForce(projectile.transform.up*this.enemy.projectileSpeed, ForceMode2D.Impulse);
            Projectile projectileController = projectile.GetComponent<Projectile>();
            projectileController.primaryTarget = "Player";
            projectileController.damage = this.enemy.attackDamage;
            projectileController.speed = this.enemy.projectileSpeed;
            projectileController.direction = projectile.transform.up;
            projectileController.isBouncing = true;
            projectileController.numberOfTouchesRemaining = 3;
            attackCooldown = 0; //Reset enemy cooldown to prevent constant attacking.
        }
        
    }

    void spawnMiniMelee()
    {
        this.roomLoaderObject.GetComponent<RoomLoaderSpawner>().spawnMiniMelee(this.enemy, transform.position);
        attackCooldown = 0; //Reset enemy cooldown to prevent constant attacking.
    }

    void rangeHood()
    {
        Vector3 playerDistance = transform.position - player.transform.position;
        if ((this.enemyContainer.GetComponent<GenericRangeAI>().canShoot && playerDistance.magnitude < 8) || attackDurationRemaining > 0) //Once in position and in range, then shoot or attack underway
        {
            //Once in position then shoot
            player.transform.position += (10.0f/(playerDistance.magnitude*playerDistance.magnitude)) * playerDistance * Time.deltaTime; //Inverse Square Law of Gravitation
            if(playerDistance.magnitude < 3) //Damage radius
            {
                player.GetComponent<PlayerController>().takeDamage(Mathf.Abs(3-playerDistance.magnitude)*this.enemy.attackDamage); // Do more damage the closer you are. 
            }
            if(attackDurationRemaining == 0) //First execution of range attack
            {
                attackDurationRemaining = (int)this.enemy.projectileSpeed; //Projectile speed represents how long the attack lasts
            }
            attackCooldown = 0; //Reset enemy cooldown to prevent constant attacking.
        }
    }

    public void takeDamage(float damage)
    {
        if (this.enemy.alive) //Only take damage while enemy is alive
        {
            //Add effects to show enemy taking damage
            this.enemy.takeDamage(damage);
            if (this.enemy.alive) //Enemy just died after taking damage
            {
                this.player.GetComponent<PlayerController>().incrementEnemySlain();
                gameObject.SendMessageUpwards("removeEnemy", enemy);
            }
        }
    }
}
