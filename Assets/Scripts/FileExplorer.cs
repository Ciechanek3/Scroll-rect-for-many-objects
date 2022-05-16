using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class FileExplorer : MonoBehaviour
{
    private string path;
    public void OnButtonClick()
    {
        path = DataConverter.Instance.Dir.ToString();
        EditorUtility.OpenFilePanel("Image Explorer", path, "png");
    }
}
