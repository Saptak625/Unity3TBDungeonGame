using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chest : MonoBehaviour
{
    Items Items;
    public item givenItem;
    public GameObject droppedItem;

    public SpriteRenderer sr;
    public Sprite openChest;

    // Start is called before the first frame update
    void Start()
    {
        System.Random random = new System.Random();
        givenItem = Items.items[random.Next(0, Items.itemsListMax)];
    }

    public void dropObject()
    {
        GameObject groundItem = Instantiate(droppedItem, new Vector3(transform.position.x, transform.position.y - 1.25f, -2),transform.rotation);
        groundItem.GetComponent<DroppedItemScript>().heldItem = givenItem;
        groundItem.transform.parent = gameObject.transform;

        sr.sprite = openChest;
    }
}