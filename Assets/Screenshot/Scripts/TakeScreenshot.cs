using UnityEngine;
using System.Collections;
using System;

public class TakeScreenshot : MonoBehaviour
{
    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            string filename = string.Format("{0}/Screenshot/screen_{1}x{2}_{3}.png", 
                Application.dataPath,
                Screen.width,
                Screen.height,
                DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss"));

            Application.CaptureScreenshot(filename);

            Debug.Log("Take photo in " + filename);
        }
    }
}
