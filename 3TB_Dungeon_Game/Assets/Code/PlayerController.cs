using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    public Rigidbody2D rb;
    public int speed = 5;
    public int enemiesSlain = 0;
    public int roomsCleared = 0;

    bool nearChest;
    GameObject Chest;
    public bool ableToOpenChests = true; //Once false, players wont be able to open chests.

    bool nearItem;
    GameObject droppedItem;
    item currentWeapon;
    item currentShield;
    item inbetweenItem;
    Items items;

    public float health = 100f;
    public bool isAlive = true;
    public bool usingShield = false;
    public bool shieldOnCooldown = false;
    public int shieldDuration;
    public int shieldCooldown;
    public bool weaponOnCooldown = false;
    public int weaponCooldown;

    public Camera camera;

    public GameObject canvas;
    public GameObject StartUI; //The Screen thats in the hierarchy to start
    public GameObject EndScreenPrefab;
    GameObject EndScreen;
    public GameObject InGameHUDPrefab;
    GameObject InGameHUD;

    RectTransform healthBarTransform;
    Image shieldImage;
    Image weaponImage;
    SpriteRenderer droppedObjectRenderer;
    float healthDecimal;

    public Sprite noShield, NCHPShield, HPShield, EarplugsShield, EarplugsHPShield; // Shields
    public Sprite headPhone, earplug, solar, alien, fireworks; // Weapons
    public GameObject headPhoneProjectile, earplugProjectile, solarProjectile, alienProjectile, fireworksProjectile; // Weapons

    public int playerState; //0 = Start, 1 = Alive, 2 = dead

    public Animator animator;

    public Transform playerSpriteTransform;

    public bool isPaused = false;

    // Start is called before the first frame update
    void Start()
    {
        currentWeapon = Items.headphoneLauncher;
        currentShield = Items.noShield;
        playerState = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (!isPaused)
        {
            if (playerState == 0) //Start
            {
                if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.D) || Input.GetMouseButtonDown(0) || Input.GetMouseButtonDown(1))
                {
                    playerState = 1;
                    DestroyImmediate(StartUI, true);
                    InGameHUD = Instantiate(InGameHUDPrefab);
                    InGameHUD.transform.SetParent(canvas.transform, false);
                    PauseMenu pauseMenu = InGameHUD.transform.GetChild(5).gameObject.GetComponent<PauseMenu>();
                    pauseMenu.pauseMenu = canvas.transform.GetChild(0).gameObject;
                    pauseMenu.player = gameObject;
                    canvas.transform.GetChild(0).GetChild(1).gameObject.GetComponent<Button>().onClick.AddListener(pauseMenu.Resume);
                    canvas.transform.GetChild(0).GetChild(2).gameObject.GetComponent<Button>().onClick.AddListener(pauseMenu.Quit);

                    healthBarTransform = InGameHUD.transform.GetChild(2).gameObject.GetComponent<RectTransform>();
                    shieldImage = InGameHUD.transform.GetChild(3).gameObject.GetComponent<Image>();
                    weaponImage = InGameHUD.transform.GetChild(4).gameObject.GetComponent<Image>();
                }
            }
            else if (playerState == 1) //Alive
            {
                if (!isAlive) //If dead
                {
                    playerState = 2;
                    Destroy(InGameHUD);
                    EndScreen = Instantiate(EndScreenPrefab);
                    EndScreen.transform.SetParent(canvas.transform, false);
                    EndScreen.transform.FindChild("Text (2)").gameObject.GetComponent<Text>().text = enemiesSlain.ToString() + (enemiesSlain == 1 ? " Enemy Slain" : " Enemies Slain");
                    EndScreen.transform.FindChild("Text (3)").gameObject.GetComponent<Text>().text = roomsCleared.ToString() + (roomsCleared == 1 ? " Room Cleared" : " Rooms Cleared");
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

                if (weaponOnCooldown)
                {
                    weaponCooldown++;
                    if (weaponCooldown >= currentWeapon.reloadSpeed)
                    {
                        weaponOnCooldown = false;
                        weaponCooldown = 0;
                    }
                }

                //Movement Controller
                float xInput = Input.GetAxisRaw("Horizontal");
                playerSpriteTransform.eulerAngles = new Vector3(0, (xInput < 0f ? 180 : 0), 0); //Change this flipping behavior if needed
                rb.velocity = new Vector2(xInput * speed, Input.GetAxisRaw("Vertical") * speed);
                animator.SetFloat("Distance", rb.velocity.magnitude);


                if (Input.GetMouseButtonDown(0))
                {
                    if (weaponOnCooldown == false)
                    {
                        shoot();
                        weaponOnCooldown = true;
                    }
                }
                if (Input.GetMouseButtonDown(1)) //Checks if user Right Click
                {
                    Debug.Log("Player Chest");
                    Debug.Log(nearChest);
                    Debug.Log(ableToOpenChests);
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
                            if (currentWeapon.ID == 0)
                            {
                                weaponImage.sprite = headPhone;
                            }
                            else if (currentWeapon.ID == 1)
                            {
                                weaponImage.sprite = earplug;
                            }
                            else if (currentWeapon.ID == 2)
                            {
                                weaponImage.sprite = solar;
                            }
                            else if (currentWeapon.ID == 3)
                            {
                                weaponImage.sprite = alien;
                            }
                            else if (currentWeapon.ID == 4)
                            {
                                weaponImage.sprite = fireworks;
                            }
                        }
                        else
                        {
                            inbetweenItem = currentShield;
                            currentShield = droppedItem.GetComponent<DroppedItemScript>().heldItem;
                            droppedItem.GetComponent<DroppedItemScript>().heldItem = inbetweenItem;
                            inbetweenItem = null;

                        
                            if (currentShield.ID == 5)
                            {
                                shieldImage.sprite = EarplugsShield;
                            }
                            else if (currentShield.ID == 6)
                            {
                                shieldImage.sprite = HPShield;
                            }
                            else if (currentShield.ID == 7)
                            {
                                shieldImage.sprite = NCHPShield;
                            }
                            else if (currentShield.ID == 8)
                            {
                                shieldImage.sprite = EarplugsHPShield;
                            }
                            else if (currentShield.ID == 9)
                            {
                                shieldImage.sprite = noShield;
                            }
                            //Debug.Log("Switched Shield");
                        }
                    }
                    else if (usingShield == false && shieldOnCooldown == false) //Player will shield
                    {
                        //Debug.Log("Shield");
                        usingShield = true;
                        speed -= currentShield.speedReduction;
                    }
                }
            }
            else if (playerState == 2) //Dead
            {
                if (Input.GetMouseButtonDown(0))
                {
                    Scene scene = SceneManager.GetActiveScene();
                    SceneManager.LoadScene(scene.name);
                }
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

    public void deadTrigger()
    {
        Debug.Log("Dead Results");
        Debug.Log(enemiesSlain);
        Debug.Log(roomsCleared);
        //Use this trigger for detecting when an enemy dies.
        animator.SetBool("Dead", true);
    }

    public void takeDamage(float damage)
    {
        if (this.isAlive)
        {
            //Debug.Log("Executing");
            Debug.Log(damage);
            Debug.Log(this.health);
            playerSpriteTransform.gameObject.GetComponent<SpriteRenderer>().color = new Color(255, 0, 0);
            Invoke("regular", 0.1f);
            if (usingShield)
            {
                this.health -= damage * currentShield.resistance;
            }
            else
            {
                this.health -= damage;
            }
            //Debug.Log(this.health);

            this.healthDecimal = this.health * 0.01f;

            healthBarTransform.sizeDelta = new Vector2(325 * this.healthDecimal, 50);
            this.isAlive = this.health > 0;

            if (this.isAlive == false) {
                this.deadTrigger();
            }
        }
    }

    void regular()
    {
        playerSpriteTransform.gameObject.GetComponent<SpriteRenderer>().color = new Color(255, 255, 255);
    }

    public void shoot()
    {
        GameObject projectileChoice = headPhoneProjectile;
        if(this.currentWeapon.ID == 1)
        {
            projectileChoice = earplugProjectile;
        }else if(this.currentWeapon.ID == 2)
        {
            projectileChoice = solarProjectile;
        }
        else if (this.currentWeapon.ID == 3)
        {
            projectileChoice = alienProjectile;
        }
        else if (this.currentWeapon.ID == 4)
        {
            projectileChoice = fireworksProjectile;
        }

        Vector2 lookDir = camera.ScreenToWorldPoint(Input.mousePosition) - transform.position;
        GameObject projectile = Instantiate(projectileChoice, new Vector3(transform.position.x, transform.position.y, transform.position.z + 1), Quaternion.LookRotation(Vector3.forward, lookDir), gameObject.transform);
        Projectile projectileController = projectile.GetComponent<Projectile>();
        projectileController.primaryTarget = "Enemy Container";
        projectile.layer = 8;
        projectileController.damage = this.currentWeapon.damage;
        projectileController.speed = this.currentWeapon.bulletSpeed;
        projectileController.direction = projectile.transform.up;

        if (currentWeapon.bulletsBounce == true)
        {
            projectileController.isBouncing = true;
            projectileController.numberOfTouchesRemaining = 2;
        }
    }
}