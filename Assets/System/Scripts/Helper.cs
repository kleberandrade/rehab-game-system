﻿using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class Helper
{
    public static float CameraDepht(Vector3 position)
    {
        return Mathf.Abs(Camera.main.transform.position.z - position.z);
    }

    public static float Normalization(float position, float min, float max)
    {
        return (position - min) / (max - min);
    }

    public static float InverseNormalization(float position, float min, float max)
    {
        return min + (max - min) * position;
    }

    public static float ViewportToWord(float position, float depth)
    {
        return Camera.main.ViewportToWorldPoint(new Vector3(position, 0, depth)).x;
    }

    public static float ViewportToWord(float position, float min, float max, float depth)
    {
        position = InverseNormalization(position, min, max);
        return Camera.main.ViewportToWorldPoint(new Vector3 (position, 0, depth)).x;
    }

    public static float WorldToViewport(Vector3 position, float depth)
    {
        Vector3 viewport = Camera.main.WorldToViewportPoint(position);
        return viewport.x;
    }

    public static float WorldToViewport(Vector3 position, float min, float max)
    {
        return Normalization(WorldToViewport(position, CameraDepht(position)), min, max);
    }

    public static float WorldToViewport(Vector3 position, float min, float max, float depth)
    {
        return Normalization(WorldToViewport(position, depth), min, max);
    }

    public static bool SaveJsonToFileText(string data, string filename)
    {
        try
        {
            using (StreamWriter writer = File.CreateText(Application.dataPath + "/" + filename))
            {
                Debug.Log(string.Format("Saving in {0} data {1}", Application.dataPath + "/" + filename, data));
                writer.WriteLine(data);
            }

            return true;
        }
        catch
        {
            return false;
        }
    }

    public static List<T> LoadJsonToFileText<T>(string filename, out int count)
    {
        List<T> list = new List<T>();
        count = 0;
        try
        {
            if (File.Exists(Application.dataPath + filename))
            {
                using (StreamReader reader = new StreamReader(Application.dataPath + "/" + filename))
                {
                    string line;
                    while ((line = reader.ReadLine()) != null)
                    {
                        T temp = JsonUtility.FromJson<T>(line);
                        list.Add(temp);
                        count++;
                    }
                }
            }
        }
        catch (Exception ex)
        {
            Debug.LogError(ex.Message);
        }

        return list;
    }

    public static float Point2Line(float x, float y, float a, float b, float c)
    {
        return Mathf.Abs(a * x + b * y + c) / Mathf.Sqrt(Mathf.Pow(a,2) + Mathf.Pow(b,2));
    }

    public static float JLerp(float from, float to, float time)
    {
        if (time < 0.0f)
            return from;

        if (time > 1.0f)
            return to;

        return Mathf.Abs(to - from) * (10.0f * Mathf.Pow(time, 3) - 15.0f * Mathf.Pow(time, 4) + 16.0f * Mathf.Pow(time, 5));
    }
}
