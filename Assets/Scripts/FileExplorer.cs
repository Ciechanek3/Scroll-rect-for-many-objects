using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FileExplorer : MonoBehaviour
{
    private string path;
    private void Start()
    {
        path = DataConverter.Instance.Dir.ToString(); 
    }
}
