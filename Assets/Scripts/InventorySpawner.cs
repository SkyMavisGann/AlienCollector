using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InventorySpawner : MonoBehaviour
{
    public Transform Item1;
    public Transform Item2;
    public GameObject TemplateCow;
    public GameObject TemplateItem;
    public GameObject TemplateLamp;
    public Transform MarkerTemplate;

    private PlayerStats ps;
    private Vector3 distance;
    private int fullDistanceDown = 0;
    public List<GameObject> items = new List<GameObject>();
    // Start is called before the first frame update
    void Start()
    {
        Item1.gameObject.SetActive(false);
        Item2.gameObject.SetActive(false);
        GameObject pso = GameObject.Find("SaveState");
        if (pso != null)
        {
            ps = pso.GetComponent<PlayerStats>();
        }
        distance = Item1.transform.position - Item2.transform.position;

        for (int i = 0; i < ps.OwnedCows.Count; i++)
        {
            if (ps.OwnedCows.ElementAt(i).Value > 0)
            {
                Debug.Log("spawning cow");
                items.Add(Instantiate(TemplateCow, Item1.position - (distance * (items.Count)), Item1.rotation, transform));
                GameObject cowObject = Resources.Load<GameObject>(ps.OwnedCows.ElementAt(i).Key);
                if (cowObject != null)
                {
                    items[items.Count - 1].SetActive(true);
                    items[items.Count - 1].GetComponentInChildren<DecorButton>().ContainedObject = cowObject;
                }
                else
                {
                    Debug.Log("Unable to find " + ps.OwnedCows.ElementAt(i).Key);
                }
                fullDistanceDown++;
            }
        }

        for (int i = 0; i < ps.OwnedItems.Count; i++)
        {
            Debug.Log("spawning Item");
            items.Add(Instantiate(TemplateItem, Item1.position - (distance * fullDistanceDown), Item1.rotation, transform));
            GameObject itemObject = Resources.Load<GameObject>("Decor/" + ps.OwnedItems[i]);
            items[fullDistanceDown].SetActive(true);
            items[fullDistanceDown].GetComponentInChildren<DecorButton>().ContainedObject = itemObject;
            fullDistanceDown++;
        }
        if (ps.OwnedLamps == null)
        {
            ps.OwnedLamps = new List<string>();
        }
        for (int i = 0; i < ps.OwnedLamps.Count; i++)
        {
            Debug.Log("spawning Item");
            items.Add(Instantiate(TemplateLamp, Item1.position - (distance * fullDistanceDown), Item1.rotation, transform));
            GameObject itemObject = Resources.Load<GameObject>("LavaLamps/" + ps.OwnedLamps[i]);
            items[fullDistanceDown].SetActive(true);
            items[fullDistanceDown].GetComponentInChildren<DecorButton>().ContainedObject = itemObject;
            fullDistanceDown++;
        }


        //set amount for dragging menu

        ClickSlidePosition CSP = transform.parent.GetComponent<ClickSlidePosition>();
        if (ps.OwnedItems.Count > 0)
        {
            MarkerTemplate.localPosition = new Vector3(
                items[ps.OwnedItems.Count - 1].transform.localPosition.x,
               (transform.localPosition.y) + ((items[Mathf.Clamp(CSP.AllowedBoxesToSee - 1, 0, items.Count - 1)].transform.localPosition.y) - (items[items.Count - 1].transform.localPosition.y)),
                items[ps.OwnedItems.Count - 1].transform.localPosition.z);
        }

        GameObject emptyGO = new GameObject();
        emptyGO.name = "SecondMarker";
        Transform newTransform = emptyGO.transform;

        CSP.ClampObject = Instantiate(newTransform, MarkerTemplate.position, MarkerTemplate.rotation, CSP.transform.parent);
    }
}
