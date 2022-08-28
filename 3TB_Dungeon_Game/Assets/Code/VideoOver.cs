using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

public class VideoOver : MonoBehaviour
{
    public string url;
    public double time;
    public double currentTime;
    public GameObject player;
    public GameObject startScreen;

    // Use this for initialization
    void Start()
    {
        VideoPlayer vp = gameObject.GetComponent<VideoPlayer>();
        vp.url = url;
        time = vp.clip.length;
    }


    // Update is called once per frame
    void Update()
    {
        currentTime = gameObject.GetComponent<VideoPlayer>().time;
        if (Math.Round(currentTime*10)/10 >= Math.Round(time*10)/10)
        {
            startScreen.SetActive(true);
            player.GetComponent<PlayerController>().enabled = true;
            gameObject.transform.parent.gameObject.SetActive(false);
        }
    }
}
