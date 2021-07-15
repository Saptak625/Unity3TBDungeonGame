using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public Rigidbody2D rb;
    public int speed = 5;
    public int enemiesSlain = 0;
    public int roomsCleared = 0;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        rb.velocity = new Vector2(Input.GetAxisRaw("Horizontal") * speed, Input.GetAxisRaw("Vertical") * speed);
    }

    public void incrementEnemySlain()
    {
        enemiesSlain++;
    }

    public void incrementRoomsCleared()
    {
        roomsCleared++;
    }
}
