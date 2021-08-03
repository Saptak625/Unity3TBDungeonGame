using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestructableWall : MonoBehaviour
{
    public float hp = 30f;

    public void takeDamage(float damage)
    {
        this.hp -= damage;
        if(this.hp <= 0f)
        {
            Destroy(gameObject); //Destroy Wall
            //Call Roomloader reloadAStarGrid
            gameObject.SendMessageUpwards("reloadAStarGrid");
        }
    }
}
