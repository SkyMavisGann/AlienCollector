using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class PlayerStats : MonoBehaviour
{
    public bool InitialLoaded = false;
    public bool AllowSpawnAliens = true;
    public int MusicStateCount;

    //I need to calculate time since you closed the app, gotten when app opens

    private void Update()
    {
        if (InitialLoaded == false)
        {
            GameObject.Find("LoadIn").GetComponent<LoadInScene>().setupScene();
        }
    }
    //to be saved
    //placemat decoration Dictionary<ThePlaceMat, TheObject>
    public Dictionary<string, string> PlacematDecorations = new Dictionary<string, string>();
    public int Scrap;
    //cowtype, risk, id
    public List<Tuple<string, int, string>> Cows = new List<Tuple<string, int, string>>();
    // time when to despawn, Object attached to, alien name
    public List<Tuple<long, string, string>> Aliens = new List<Tuple<long, string, string>>();
    // DateTime.Now in unix; the alien will have a time when it is supposed to despawn, say it will leave at 12
    public long TimeTillCanSpawnAnAlien = 0;
    //The time it at which you closed the game last, used to determmine how
    public long TimeLeftGame;
    //id, amount of cows owned;
    public Dictionary<string, int> OwnedCows = new Dictionary<string, int>();
    public List<string> OwnedItems = new List<string>();
    public List<string> OwnedLamps = new List<string>();
    public string PlacedLampCurrent = "LavaLampBlue";

    public List<Tuple<string, int>> AlienGifts = new List<Tuple<string, int>>();
    public List<string> DiscoveredAliens = new List<string>();
    public long GameStartTime;
    //timer for spawning scrap and cows outside the window
    public float SpawningTimer;

    //planet name and theta
    public Dictionary<string, float> PlanetTheta = new Dictionary<string, float>();

    public int TutorialState;

    public int MusicState;

    public float TimeScale = 1;

    public bool UpgradedShip = false;
    public bool BeenToMercury = false;
    //locationName, ETA arriving, TimeLeft, Destination Coords of, startPositon coords
    public Tuple<string, long, long, Tuple<float, float>, Tuple<float, float>> TravelLocation;
    public Tuple<float, float> TargetPosition;

    void OnApplicationPause(bool pauseStatus)
    {
        if (pauseStatus ==  true)
        {
            save();
        }
    }

    private void OnApplicationQuit()
    {
        save();
    }
    public void save()
    {
        TimeLeftGame = UnixTime.GetUnixTime(DateTime.Now);
        SaveManager.saveData(this);
    }
}
