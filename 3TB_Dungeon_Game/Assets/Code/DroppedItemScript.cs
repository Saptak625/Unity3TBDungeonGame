using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DroppedItemScript : MonoBehaviour
{
    public item heldItem;

    public SpriteRenderer sr;

    public Sprite headphoneLauncher;
    public Sprite automaticEarplugGun;
    public Sprite solarRayGun; //future laser
    public Sprite alienGun;
    public Sprite fireworksLauncher;

    public Sprite earplugShield;
    public Sprite headphoneShield;
    public Sprite noiseCancellingHPShield;
    public Sprite headphonesAndEarplugsShield;
    public Sprite noShield; //default

    public void Update()
    {
        if (heldItem != null)
        {
            Sprite chosenSprite;
            if (heldItem.ID == 0)
            {
                chosenSprite = headphoneLauncher;
            }
            else if (heldItem.ID == 1)
            {
                chosenSprite = automaticEarplugGun;
            }
            else if (heldItem.ID == 2)
            {
                chosenSprite = solarRayGun;
            }
            else if (heldItem.ID == 3)
            {
                chosenSprite = alienGun;
            }
            else if (heldItem.ID == 4)
            {
                chosenSprite = fireworksLauncher;
            }
            else if (heldItem.ID == 5)
            {
                chosenSprite = earplugShield;
            }
            else if (heldItem.ID == 6)
            {
                chosenSprite = headphoneShield;
            }
            else if (heldItem.ID == 7)
            {
                chosenSprite = noiseCancellingHPShield;
            }
            else if (heldItem.ID == 8)
            {
                chosenSprite = headphonesAndEarplugsShield;
            }
            else
            {
                chosenSprite = noShield;
            }
            sr.sprite = chosenSprite;
        }
    }
}
