using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class UIScrollListPool : MonoBehaviour
{
    [SerializeField]
    private GameObject listElement;

    private List<GameObject> poolOfElements = new List<GameObject>();

    public void SetupPool(int visibleElements)
    {
        for (int i = 0; i < visibleElements; i++)
        {
            var pooledObject = Instantiate(listElement, transform);
            pooledObject.SetActive(false);
            poolOfElements.Add(pooledObject);
        }
    }
    public async void SetupStartingElements(int visibleElements, Transform parent)
    {
        var takeItems = new Task[visibleElements];
        for (int i = 0; i < visibleElements; i++)
        {
            var item = TakeItemFromPool(parent);
            takeItems[i] = ChangeElement(item, parent, i);
        }
        await Task.WhenAll(takeItems);
    }
    public GameObject TakeItemFromPool(Transform parent)
    {
        GameObject element;
        if (poolOfElements.Count == 0)
        {
            element = Instantiate(listElement, parent);
        }
        else
        {
            element = poolOfElements[0];
        }
        return element;
    }
    public async Task ChangeElement(GameObject element, Transform parent, int dataIndex)
    {
        element.transform.SetParent(parent, false);
        element.SetActive(true);
        poolOfElements.Remove(element);
        element.GetComponent<ImageWithDetails>().SetVariables(DataConverter.Instance.GetSpecificElement(dataIndex));
        await Task.Delay(1);
    }
    public void ReturnItemToPool(GameObject item)
    {
        poolOfElements.Add(item);
        item.transform.SetParent(transform);
        item.SetActive(false);
    }
}
