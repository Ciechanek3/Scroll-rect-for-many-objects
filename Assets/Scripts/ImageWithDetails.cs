using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System;

public class ImageWithDetails : MonoBehaviour
{
    [SerializeField]
    private Image image;
    [SerializeField]
    private TextMeshProUGUI fileName;
    [SerializeField]
    private TextMeshProUGUI creationDate;

    private DateTime creationDateTime;

    private void OnEnable()
    {
        DataConverter.Instance.OnRefreshButtonPressed += RefreshTimer;
    }
    private void OnDisable()
    {
        DataConverter.Instance.OnRefreshButtonPressed += RefreshTimer;
    }

    public void SetVariables(ImageData imageData)
    {
        image.sprite = imageData.Image;
        fileName.text = imageData.FileName;
        creationDateTime = imageData.CreationDate;
        RefreshTimer();
    }
    public void RefreshTimer()
    {
        var timeSinceCreation = (DataConverter.Instance.startUpDateTime - creationDateTime).ToString();
        creationDate.text = timeSinceCreation.Substring(0, timeSinceCreation.LastIndexOf("."));
    }
}
