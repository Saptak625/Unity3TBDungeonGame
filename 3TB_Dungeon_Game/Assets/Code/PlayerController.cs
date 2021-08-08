using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public Rigidbody2D rb;
    public int speed = 5;
    public int enemiesSlain = 0;
    public int roomsCleared = 0;

    bool nearChest;
    GameObject Chest;
    bool ableToOpenChests = true; //Once false, players wont be able to open chests.

    bool nearItem;
    GameObject droppedItem;
    item currentWeapon;
    item currentShield;
    item inbetweenItem;

    public float health = 100f;
    public bool isAlive = true;
    public bool usingShield = false;
    public bool shieldOnCooldown = false;
    public int shieldDuration;
    public int shieldCooldown;

    public GameObject genericProjectile;
    public Camera camera;

    // Start is called before the first frame update
    void Start()
    {
        currentWeapon = Items.defaultWeapon;
        currentShield = Items.defaultShield;
    }

    // Update is called once per frame
    void Update()
    {
        //Debug.Log(this.health);
        if (!isAlive) //If dead
        {
            Debug.Log("Player Dead...\n Game Over...");
        }

        if (usingShield)
        {
            shieldDuration++;
            if (shieldDuration >= currentShield.duration)
            {
                usingShield = false;
                shieldOnCooldown = true;
                shieldDuration = 0;
            }
        }
        else if (shieldOnCooldown)
        {
            speed = 5;
            shieldCooldown++;
            if (shieldCooldown >= currentShield.cooldown)
            {
                shieldOnCooldown = false;
                shieldCooldown = 0;
            }
        }

        rb.velocity = new Vector2(Input.GetAxisRaw("Horizontal") * speed, Input.GetAxisRaw("Vertical") * speed);

        if (Input.GetMouseButtonDown(0))
        {
            shoot();
        }
        if (Input.GetMouseButtonDown(1)) //Checks if user Right Click
        {
            if (nearChest && ableToOpenChests) //Player will open chest
            {
                Chest.GetComponent<Chest>().dropObject();
                ableToOpenChests = false; //TODO: Saptak, once chest room is closed, you can turn this true
            }
            else if (nearItem)
            {
                if (droppedItem.GetComponent<DroppedItemScript>().heldItem.type == "Weapon")
                {
                    inbetweenItem = currentWeapon;
                    currentWeapon = droppedItem.GetComponent<DroppedItemScript>().heldItem;
                    droppedItem.GetComponent<DroppedItemScript>().heldItem = inbetweenItem;
                    inbetweenItem = null;
                    Debug.Log("Switched Weapon");
                }
                else
                {
                    inbetweenItem = currentShield;
                    currentShield = droppedItem.GetComponent<DroppedItemScript>().heldItem;
                    droppedItem.GetComponent<DroppedItemScript>().heldItem = inbetweenItem;
                    inbetweenItem = null;
                    Debug.Log("Switched Shield");
                }
            }
            else if (usingShield == false && shieldOnCooldown == false) //Player will shield
            {
                Debug.Log("Shield");
                usingShield = true;
                speed -= currentShield.speedReduction;
            }
        }
    }

    public void incrementEnemySlain()
    {
        enemiesSlain++;
    }

    public void incrementRoomsCleared()
    {
        roomsCleared++;
    }

    public void OnTriggerEnter2D(Collider2D collider)
    {
        GameObject other = collider.gameObject;

        if (other.CompareTag("Chest"))
        {
            Chest = other;
            nearChest = true;
        }
        if (other.CompareTag("Item"))
        {
            droppedItem = other;
            nearItem = true;
        }
    }

    public void OnTriggerExit2D(Collider2D collider)
    {
        if (collider.tag == "Chest")
        {
            Chest = null;
            nearChest = false;
        }
        if (collider.CompareTag("Item"))
        {
            droppedItem = null;
            nearItem = false;
        }
    }

    public void takeDamage(float damage)
    {
        if (this.isAlive)
        {
            Debug.Log("Executing");
            Debug.Log(damage);
            Debug.Log(this.health);
            if (usingShield)
            {
                this.health -= damage * currentShield.resistance;
            }
            else
            {
                this.health -= damage;
            }
            Debug.Log(this.health);
            this.isAlive = this.health > 0;
        }
    }

    public void shoot()
    {
        Vector2 lookDir = camera.ScreenToWorldPoint(Input.mousePosition) - transform.position;
        GameObject projectile = Instantiate(genericProjectile, new Vector3(transform.position.x, transform.position.y, transform.position.z + 1), Quaternion.LookRotation(Vector3.forward, lookDir), gameObject.transform);
        Projectile projectileController = projectile.GetComponent<Projectile>();
        projectileController.primaryTarget = "Enemy Container";
        projectile.layer = 8;
        projectileController.damage = this.currentWeapon.damage;
        projectileController.speed = this.currentWeapon.bulletSpeed;
        projectileController.direction = projectile.transform.up;
    }
}