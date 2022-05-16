using System;
using UnityEngine;

public class ImageData
{
    private Sprite image;
    private string fileName;
    private DateTime creationDate;

    public ImageData(Sprite i, string fn, DateTime cd)
    {
        image = i;
        fileName = fn;
        creationDate = cd;
    }
    public Sprite Image { get => image; }
    public string FileName { get => fileName; }
    public DateTime CreationDate { get => creationDate; }
}
