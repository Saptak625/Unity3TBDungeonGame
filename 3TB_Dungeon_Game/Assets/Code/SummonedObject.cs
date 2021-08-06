using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SummonedObject : MonoBehaviour
{
    public bool fallingType = true; //Default is falling object, not effect
    public bool summoned = true; //Variable to indicate whether UI is visible.
    public int duration; //Time till object summon
    public GameObject player; //Reference to Player
    public GameObject targetObject; //Target UI
    public Vector3 endPosition; //Target location
    public float damageRadius; //Damage Radius
    public int damageDuration; //Damage Time
    public float damage; //Damage per Frame
    float t = 0; //Lerping Time

    public GameObject targetSprite;

    // Start is called before the first frame update
    void Start()
    {
        if (!fallingType) //Effect type shouldn't be visible initially
        {
            GetComponent<SpriteRenderer>().enabled = false; //Object invisible
            summoned = false;
        }
        transform.position += new Vector3(0, 40, 0); //Setting height of object in 2D projection
        this.targetObject = Instantiate(targetSprite, endPosition, Quaternion.identity); //Creating target location
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (t > 1)
        {
            if (!summoned) //This is an effect and hasn't been spawned yet
            {
                GetComponent<SpriteRenderer>().enabled = true; //Object visible
                summoned = true;
            } 
            radialDamage();
            return;
        }
        transform.position += new Vector3();
        //transform.position = Vector2.Lerp(transform.position, this.endPosition, this.t);
        this.t += 1 / duration; //Range from 0 to 1 with slope of 1/x
    }

    void radialDamage()
    {
        if(damageDuration > 0)
        {
            if ((player.transform.position-transform.position).magnitude < damageRadius)
            {
                player.GetComponent<PlayerController>().takeDamage(this.damage);
            }
            damageDuration--;
        }
        else
        {
            //Object Summon and Damage Time Over
            destroy();
        }
    }

    void destroy()
    {
        //Add any effects later
        Destroy(targetObject);
        Destroy(gameObject);
    }
}
