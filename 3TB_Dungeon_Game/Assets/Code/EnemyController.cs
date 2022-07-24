using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class EnemyController : MonoBehaviour
{
    public Animator animator;
    public Enemy enemy = null; //Enemy virtual state
    public GameObject player; //Reference to player GameObject
    public int attackCooldown = 0;
    public int attackDurationRemaining = 0;
    public GameObject genericProjectile;
    public GameObject genericObject;
    public GameObject enemyContainer;
    public GameObject roomLoaderObject;
    public static System.Random random = new System.Random(); //Random object for proper generation

    // Start is called before the first frame update
    void Start()
    {
        this.enemyContainer = gameObject.transform.parent.gameObject; //Set parent container to grab attributes
        attackDurationRemaining = random.Next(0, ((int) enemy.cooldown)/2);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
       if(enemy != null) //Enemy Virtual state has been created.
       {
            if (enemy.destroy) //Time to clear enemy
            {
                destroy();
            }
            if (enemy.alive)
            {
                if (attackDurationRemaining == 0)
                {
                    //Do logic to determine attack
                    attackEndTrigger(); //Attack completed after duration
                    if (attackCooldown == enemy.cooldown)
                    {
                        if (enemy.attackType == EnemyAttack.Melee || enemy.attackType == EnemyAttack.Mini) 
                        {
                            this.classicMelee();
                        }
                        else if (enemy.attackType == EnemyAttack.Range && !(enemy.enemyType == (EnemyType)3 || enemy.enemyType == (EnemyType)4))
                        {
                            this.classicRange();
                        }
                        else if (enemy.attackType == EnemyAttack.Mage && (enemy.enemyType == (EnemyType)1 || enemy.enemyType == (EnemyType)2 || enemy.enemyType == (EnemyType)5 || enemy.enemyType == (EnemyType)6 || enemy.enemyType == (EnemyType)8 || enemy.enemyType == (EnemyType)9))
                        {
                            this.spawnMiniMelee();
                        }
                        else if (enemy.attackType == EnemyAttack.Range && enemy.enemyType == (EnemyType)3)
                        {
                            this.rangeHood();
                        }
                        else if ((enemy.attackType == EnemyAttack.Mage && (enemy.enemyType == (EnemyType)3 || enemy.enemyType == (EnemyType)7)) || (enemy.attackType == EnemyAttack.Range && enemy.enemyType == (EnemyType)4))
                        {
                            this.summonObject(true);
                        }
                        else if (enemy.attackType == EnemyAttack.Mage && enemy.enemyType == (EnemyType)10)
                        {
                            this.summonObject(false);
                        }
                        else if (enemy.attackType == EnemyAttack.Mage && enemy.enemyType == (EnemyType)4)
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
                    if(enemy.attackType == EnemyAttack.Range && enemy.enemyType == (EnemyType)3)
                    {
                        //Only attack that needs duration is range hood
                        this.rangeHood();
                    }
                    this.attackDurationRemaining--;
                }
            }
       } 
    }

    void classicMelee()
    {
        if ((this.player.transform.position - this.enemyContainer.transform.position).magnitude < 2.5f)
        {
            //Attack player
            player.GetComponent<PlayerController>().takeDamage(this.enemy.attackDamage);
            attackCooldown = 0; //Reset enemy cooldown to prevent constant attacking.
            attackStartTrigger();
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
            attackStartTrigger();
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
            attackStartTrigger();
        }
        
    }

    void spawnMiniMelee()
    {
        this.roomLoaderObject.GetComponent<RoomLoaderSpawner>().spawnMiniMelee(this.enemy, transform.position);
        attackCooldown = 0; //Reset enemy cooldown to prevent constant attacking.
        attackStartTrigger();
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
                player.GetComponent<PlayerController>().takeDamage(Mathf.Abs(3-playerDistance.magnitude)*(this.enemy.attackDamage / this.enemy.projectileSpeed)); // Do more damage the closer you are and scaled by the overall duration. 
            }
            if(attackDurationRemaining == 0) //First execution of range attack
            {
                attackDurationRemaining = (int)this.enemy.projectileSpeed; //Projectile speed represents how long the attack lasts
                attackCooldown = 0; //Reset enemy cooldown to prevent constant attacking.
                attackStartTrigger();
            }
        }
    }

    void summonObject(bool fallingType)
    {
        GameObject sObject = Instantiate(genericObject, player.transform.position, Quaternion.identity);
        SummonedObject objectController = sObject.GetComponent<SummonedObject>();
        objectController.fallingType = fallingType;
        objectController.setVisible(fallingType);
        objectController.player = player;
        objectController.duration = (int) enemy.projectileSpeed;
        objectController.endPosition = player.transform.position;
        objectController.damageRadius = 2.5f; //2.5 radius of damage
        objectController.damageDuration = 50; //50 frames of damage
        objectController.damage = enemy.attackDamage/((float) objectController.damageDuration); //Scaling damage per frame from max.
        attackCooldown = 0; //Reset enemy cooldown to prevent constant attacking.
        attackDurationRemaining = objectController.duration;
        attackStartTrigger();
    }

    public void attackStartTrigger()
    {
        //Use this trigger for detecting when an enemy attack starts.
        animator.SetBool("Attack", true);
    }
    
    public void attackEndTrigger()
    {
        //Use this trigger for detecting when an enemy attack ends.
        animator.SetBool("Attack", false);
    }
    
    public void deadTrigger()
    {
        //Use this trigger for detecting when an enemy dies.
        animator.SetBool("Dead", true);

        //Set Collisions off and Velocity off
        this.enemyContainer.GetComponent<BoxCollider2D>().enabled = false;
        this.enemyContainer.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezePosition;

        //Set Movement Controller Off
        this.enemyContainer.GetComponent<EnemyMovementController>().enabled = false;

        //Temporary Sprite deletion once enemy dies. Get rid of this once enemy animations are complete.
        //destroy();
    }

    public void takeDamage(float damage)
    {
        if (this.enemy.alive) //Only take damage while enemy is alive
        {
            //Add effects to show enemy taking damage
            this.enemy.takeDamage(damage);
            gameObject.GetComponent<SpriteRenderer>().color = new Color(255, 0, 0);
            Debug.Log("Damage Taken");
            if (!this.enemy.alive) //Enemy just died after taking damage
            {
                Debug.Log("Dead");
                this.player.GetComponent<PlayerController>().incrementEnemySlain();
                gameObject.SendMessageUpwards("removeEnemy", enemy);
                AIPath movementComponent = enemyContainer.GetComponent<AIPath>();
                if (movementComponent != null)
                {
                    movementComponent.canMove = false;
                    movementComponent.canSearch = false;
                }
                else //This is a range movement controller
                {
                    enemyContainer.GetComponent<GenericRangeAI>().enabled = false;
                }
                gameObject.GetComponent<SpriteRenderer>().color = new Color(0.25f, 0.25f, 0.25f);
                this.deadTrigger();
            }else
            {
                //Reset sprite renderer color since enemy not dead.
                Invoke("regular", 0.1f);
            }
        }
    }

    void regular()
    {
        gameObject.GetComponent<SpriteRenderer>().color = new Color(255, 255, 255);
    }

    void destroy()
    {
        Destroy(gameObject);
        Destroy(this.enemyContainer);
    }
}
