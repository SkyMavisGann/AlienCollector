using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DecorButton : MonoBehaviour
{
    public GameObject ContainedObject;
    public TMP_Text title;
    public Image image;
    private PlacementManager placementManager;
    public int cost = 0;
    public TMP_Text costText;
    private PlayerStats ps;
    public TMP_Text Quantity;
    public GameObject PlacedText;
    // Start is called before the first frame update
    void Start()
    {
        GameObject pso = GameObject.Find("SaveState");
        if (pso != null)
        {
            ps = pso.GetComponent<PlayerStats>();
        }
        placementManager = GameObject.Find("PlacementManager").GetComponent<PlacementManager>();
        title.text = ContainedObject.name;
        image.sprite = ContainedObject.GetComponent<Image>().sprite;
        if (costText != null)
        {
            costText.text = cost.ToString();
        }
        if (Quantity != null)
        {
            if (ps.OwnedCows.ContainsKey(ContainedObject.name))
            {
                Quantity.text = "x" + ps.OwnedCows[ContainedObject.name].ToString();
            } 
            else
            {
                Quantity.text = "x0";
            }
            
        }
        if (PlacedText != null)
        {
            if (ps.PlacematDecorations.ContainsValue(ContainedObject.name) || ps.PlacedLampCurrent == ContainedObject.name)
            {PlacedText.SetActive(true);} else
            {PlacedText.SetActive(false);}
        }
    }

    public void triggerPurchase()
    {
        if (ps.Scrap >= cost)
        {
            if (ps.OwnedItems == null)
            {
                ps.OwnedItems = new List<string>();
            }
            if (!ps.OwnedItems.Contains(ContainedObject.name))
            {
                ps.Scrap -= cost;
                ps.OwnedItems.Add(ContainedObject.name);
                LockButton lbo = GetComponent<LockButton>();
                if (lbo != null)
                {
                    lbo.lockButton();
                }
            }

        }
    }
    public void triggerLampPurchase()
    {
        if (ps.Scrap >= cost)
        {
            if (ps.OwnedLamps == null)
            {
                ps.OwnedLamps = new List<string>();
            }
            if (!ps.OwnedLamps.Contains(ContainedObject.name))
            {
                ps.Scrap -= cost;
                ps.OwnedLamps.Add(ContainedObject.name);
                LockButton lbo = GetComponent<LockButton>();
                if (lbo != null)
                {
                    lbo.lockButton();
                }
            }

        }
    }
    public void triggerPurchaseCow()
    {
        if (ps.Scrap >= cost)
        {
            ps.Scrap -= cost;
            if (ps.OwnedCows.ContainsKey(ContainedObject.name))
            {
                ps.OwnedCows[ContainedObject.name] += 1;
            }
            else
            {
                ps.OwnedCows.Add(ContainedObject.name, 1);
            }
            
        }
    }


    public void triggerPlacement()
    {
        placementManager.ObjectToPlace = ContainedObject;
        placementManager.CurrentlyPlacing = true;
    }

    public void triggerCowPlacement()
    {
        placementManager.ObjectToPlace = ContainedObject;
        placementManager.CurrentlyPlacingCow = true;
        ps.OwnedCows[ContainedObject.name] = ps.OwnedCows[ContainedObject.name] - 1;
    }

    public void triggerLampPlacement()
    {
        placementManager.ObjectToPlace = ContainedObject;
        ps.PlacedLampCurrent = ContainedObject.name;
    }
}
