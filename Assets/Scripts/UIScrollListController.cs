using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class UIScrollListController : MonoBehaviour, IBeginDragHandler
{
    [Header("ScrollRect")]
    [SerializeField]
    private ScrollRect scrollRect;
    [SerializeField]
    private RectTransform scrollRectContent;
    [SerializeField]
    private RectTransform visibleArea;
    [SerializeField]
    private UIScrollListPool uIScrollListPool;
    [Header("Pooled elements")]
    [SerializeField]
    private int visibleElements;
    [SerializeField]
    private int offsetElements;
    [Header("Placement")]
    [SerializeField]
    private float itemHeight = 1;
    [SerializeField]
    private int offsetBetweenItems;

    private int maxElements;
    private float contentPosition = 0;
    private int firstCreatedElement = 0;
    private int lastCreatedElement = 0;
    private int startingElements;

    private void Start()
    {
        startingElements = visibleElements;
        Setup();
    }
    private void OnEnable()
    {
        DataConverter.Instance.OnRefreshButtonPressed += RefreshSize;
    }
    private void OnDisable()
    {
        DataConverter.Instance.OnRefreshButtonPressed += RefreshSize;
    }
    private void Setup()
    {
        maxElements = DataConverter.Instance.Info;
        scrollRect.onValueChanged.AddListener(ManageScrollRectView);
        uIScrollListPool.SetupPool(startingElements);
        if (visibleElements < maxElements)
        {
            startingElements += offsetElements;
        }
        else
        {
            startingElements = maxElements;
        }
        uIScrollListPool.SetupStartingElements(startingElements, scrollRectContent.transform);
        visibleArea.sizeDelta = new Vector2(visibleArea.sizeDelta.x, maxElements * (itemHeight + offsetBetweenItems));
        lastCreatedElement = startingElements;
    }

    private void ManageScrollRectView(Vector2 dragNormalizePos)
    {
        float dragDelta = visibleArea.anchoredPosition.y - contentPosition;
        scrollRectContent.anchoredPosition = new Vector2(scrollRectContent.anchoredPosition.x, scrollRectContent.anchoredPosition.y + dragDelta);
        UpdateContent();
        contentPosition = visibleArea.anchoredPosition.y;
    }
    public void OnBeginDrag(PointerEventData eventData)
    {
       contentPosition = visibleArea.anchoredPosition.y;
    }
    private void UpdateContent()
    {
        if (GetCurrentTopItemIndex() > offsetElements / 2 + 1)
        {
            if (lastCreatedElement >= maxElements)
            {
                return;
            }
            Transform firstChildT = scrollRectContent.GetChild(0);
            var newElement = firstChildT.gameObject;
            uIScrollListPool.ChangeElement(firstChildT.gameObject, scrollRectContent, lastCreatedElement);
            newElement.transform.SetSiblingIndex(scrollRectContent.childCount - 1);
            scrollRectContent.anchoredPosition = new Vector2(scrollRectContent.anchoredPosition.x, scrollRectContent.anchoredPosition.y - itemHeight);
            firstCreatedElement++;
            lastCreatedElement++;
        }
        else if (GetCurrentTopItemIndex() <= offsetElements / 2)
        {
            if (firstCreatedElement <= 0)
            {
                return;
            }
            Transform lastChildT = scrollRectContent.GetChild(scrollRectContent.childCount - 1);
            firstCreatedElement--;
            lastCreatedElement--;
            var newElement = lastChildT.gameObject;
            uIScrollListPool.ChangeElement(lastChildT.gameObject, scrollRectContent, firstCreatedElement);
            newElement.transform.SetSiblingIndex(0);
            scrollRectContent.anchoredPosition = new Vector2(scrollRectContent.anchoredPosition.x, scrollRectContent.anchoredPosition.y + itemHeight);
        }
    }
    private void RefreshCurrentElements()
    {
        if (maxElements > 0 && scrollRectContent.childCount < maxElements)
        {
            int iterator = maxElements > visibleElements + offsetElements ? visibleElements + offsetElements : maxElements;

            if (scrollRectContent.childCount < iterator)
            {
                for (int i = scrollRectContent.childCount; i > 0; i--)
                {
                    uIScrollListPool.ReturnItemToPool(scrollRectContent.GetChild(i - 1).gameObject);
                }
                firstCreatedElement = 0;
                uIScrollListPool.SetupStartingElements(iterator, scrollRectContent.transform);
                lastCreatedElement = iterator;
            }
            for (int i = 0; i < scrollRectContent.childCount - 1; i++)
            {
                uIScrollListPool.ChangeElement(scrollRectContent.GetChild(i).gameObject, scrollRectContent, firstCreatedElement + i);
            }
        }
        else
        {
            for (int i = scrollRectContent.childCount; i > maxElements; i--)
            {
                uIScrollListPool.ReturnItemToPool(scrollRectContent.GetChild(i - 1).gameObject);
                if(lastCreatedElement <= 7 && lastCreatedElement > 0)
                {
                    lastCreatedElement--;
                }
            }
            for (int i = 0; i < scrollRectContent.childCount; i++)
            {
                uIScrollListPool.ChangeElement(scrollRectContent.GetChild(i).gameObject, scrollRectContent, firstCreatedElement + i);
            } 
        }
    }
    private int GetCurrentTopItemIndex()
    {
        return Mathf.CeilToInt(scrollRectContent.anchoredPosition.y / itemHeight);
    }
    private void RefreshSize()
    { 
        maxElements = DataConverter.Instance.Info;
        if(maxElements == 0)
        {
            firstCreatedElement = 0;
            lastCreatedElement = 0;
        }
        visibleArea.sizeDelta = new Vector2(visibleArea.sizeDelta.x, maxElements * (itemHeight + offsetBetweenItems));
        RefreshCurrentElements();
    }
}
