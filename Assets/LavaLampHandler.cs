using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LavaLampHandler : MonoBehaviour
{
    private PlayerStats ps;
    public GameObject currentlySpawnedLamp;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (ps == null)
        {
            ps = GameObject.Find("SaveState").GetComponent<PlayerStats>();
        }
        if (ps != null)
        {
            if (ps.PlacedLampCurrent == null && ps.InitialLoaded)
            {
                ps.PlacedLampCurrent = "LavaLampBlue";
            }

            if (ps.PlacedLampCurrent != "" && currentlySpawnedLamp.name != ps.PlacedLampCurrent && ps.InitialLoaded)
            {
                Destroy(currentlySpawnedLamp);
                currentlySpawnedLamp = Instantiate(Resources.Load<GameObject>("LavaLamps/" + ps.PlacedLampCurrent), transform.position, Quaternion.identity, transform);
                currentlySpawnedLamp.name = ps.PlacedLampCurrent;
            }
        }
        
    }
}
