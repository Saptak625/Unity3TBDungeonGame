using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class TextFade : MonoBehaviour
{
    public Text tc;
    public Color originalColor;
    public float fadeOutTime = 3f;
    public float t = 0.01f;

    void Start()
    {
        originalColor = tc.color;
    }

    // Update is called once per frame
    void Update()
    {
        tc.color = Color.Lerp(originalColor, Color.clear, Mathf.Min(1, t / fadeOutTime));
        t += Time.deltaTime;
        if (tc.color.a <= 0f)
        {
            Destroy(gameObject);
        }
    }
}
