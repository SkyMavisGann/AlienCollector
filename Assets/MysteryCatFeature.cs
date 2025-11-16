using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Burst;
using UnityEngine;

public class MysteryCatFeature : MonoBehaviour
{
    private PlanetManager planetManager;
    private PlayerStats ps;
    public GameObject catImage;
    public GameObject stickyNoteImage;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (planetManager == null)
        {
            planetManager = GameObject.Find("SaveState").GetComponent<PlanetManager>();
        }
        if (ps == null)
        {
            ps = GameObject.Find("SaveState").GetComponent<PlayerStats>();
        }
        if (planetManager.currentPlanet == Planets.Planet.Mercury)
        {
            if (ps.BeenToMercury == false)
            {
                ps.AlienGifts.Add(new Tuple<string, int>("MysteryCat", UnityEngine.Random.Range(1 + 100, 100 * 2)));
                ps.BeenToMercury = true;
            }
            
        }

        if (ps.BeenToMercury)
        {
            stickyNoteImage.SetActive(true);
            catImage.SetActive(false);
        }
    }
}
