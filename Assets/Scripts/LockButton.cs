using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LockButton : MonoBehaviour
{
    private PlayerStats ps;
    public bool lockOnOwned = true;
    private DecorButton decor;
    public GameObject lockObject;
    // Start is called before the first frame update
    void Start()
    {
        decor = GetComponent<DecorButton>();
        GameObject pso = GameObject.Find("SaveState");
        if (pso != null)
        {
            ps = pso.GetComponent<PlayerStats>();
        }
        if (ps.OwnedLamps == null)
        {
            ps.OwnedLamps = new List<string>();
        }

        if (lockOnOwned)
        {
            if (ps.OwnedItems == null)
            {
                ps.OwnedItems = new List<string>();
            }
            if (ps.OwnedItems.Contains(decor.ContainedObject.name) || ps.OwnedLamps.Contains(decor.ContainedObject.name))
            {
                lockButton();
            }
        }
    }

    public void lockButton()
    {
        if (lockObject != null)
        {
            lockObject.SetActive(true);
        }
    }

}
