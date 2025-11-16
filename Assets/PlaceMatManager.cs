using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaceMatManager : MonoBehaviour
{
    private PlayerStats ps;
    private bool once = true;
    public List<PlacematScript> PlacematScripts = new List<PlacematScript>();

    private void Start()
    {
        if (PlacematScripts.Count == 0)
        {
            for (int i = 0; i < transform.childCount; i++)
            {
                Transform child = transform.GetChild(i);
                PlacematScripts.Add(child.GetComponent<PlacematScript>());
            }
        }
    }
    private void Update()
    {
        if (ps == null)
        {
            ps = GameObject.Find("SaveState").GetComponent<PlayerStats>();
        }
        if (ps.TimeLeftGame > 0 && once && ps.InitialLoaded)
        {

            long amountOfAliensToSpawn = (UnixTime.GetUnixTime(DateTime.Now) - ps.TimeLeftGame) / UnixTime.GetUnixTimeMinutes((long)(20.0f / ps.TimeScale));
            
            for (int i = 0; i < amountOfAliensToSpawn; ++i)
            {
                foreach (PlacematScript script in PlacematScripts)
                {
                    if (script.SpawnedDecoration != null)
                    {
                        script.SpawnOneGift(ps);
                    }
                    
                }
            }

            foreach (PlacematScript script in PlacematScripts)
            {
                script.SpawnAlienWhenOutOfFocus(ps);

                //if one of them has loaded then set the once to false
                if (script.SpawnedDecoration != null)
                {
                    //this causes slight issues in the tutorial
                    once = false;
                }
            }
        }
    }

    private void OnApplicationFocus(bool focus)
    {
        if (focus)
        {
            once = true;
        }
    }
    private void OnApplicationPause(bool paused)
    {
        if (!paused)
        {
            once = true;
        }
    }
}
