using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEditor;
using UnityEngine;

public class FileExplorer : MonoBehaviour
{
    private string path;
    public void OnButtonClick()
    {
        path = DataConverter.Instance.Dir.ToString();
#if UNITY_EDITOR
        EditorUtility.OpenFilePanel("Image Explorer", path, "png");
#endif
    }
}
