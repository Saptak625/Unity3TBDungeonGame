using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestructableWall : MonoBehaviour
{
    public int hp = 30;

    public void takeDamage(int damage)
    {
        this.hp -= damage;
        if(this.hp <= 0)
        {
            Destroy(gameObject); //Destroy Wall
            //Call Roomloader reloadAStarGrid
            gameObject.SendMessageUpwards("reloadAStarGrid");
        }
    }
}
