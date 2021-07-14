using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomLoaderController : MonoBehaviour
{
    RoomLoader roomLoader;
    public static Texture2D tex;
    public static GameObject loaderObject;

    // Start is called before the first frame update
    void Start()
    {
        RoomLoaderController.loaderObject = gameObject;
        roomLoader = new RoomLoader();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
