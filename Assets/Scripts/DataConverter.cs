using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class DataConverter : MonoBehaviour
{
    public static DataConverter Instance;

    public event Action OnRefreshButtonPressed;
    public DateTime startUpDateTime;
    private FileInfo[] info;
    private DirectoryInfo dir;

    public int Info { get => info.Length; }

    private void Awake()
    {
        if (!Directory.Exists("Assets/Images/"))
        {
            Directory.CreateDirectory("Assets/Images/");
        }
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }
        startUpDateTime = DateTime.Now;
        CreateDataList();
    }
    public void CreateDataList()
    {
        dir = new DirectoryInfo("Assets/Images/");
        info = dir.GetFiles("*.png");
    }
    public ImageData GetSpecificElement(int index)
    {
        Texture2D texture = new Texture2D(1, 1);
        byte[] bytes = File.ReadAllBytes(info[index].FullName);
        var name = info[index].Name.Substring(0, info[index].Name.IndexOf("."));
        texture.LoadImage(bytes);
        Sprite sprite = Sprite.Create(texture, new Rect(new Vector2(0, 0), new Vector2(texture.width, texture.height)), new Vector2(0, 0), 1, 0, SpriteMeshType.FullRect);
        return new ImageData(sprite, name, info[index].CreationTime);
    }
    public void OnButtonPressed()
    {
        startUpDateTime = DateTime.Now;
        CreateDataList();
        OnRefreshButtonPressed?.Invoke();
    }
}