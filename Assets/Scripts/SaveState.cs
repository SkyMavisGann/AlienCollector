using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SaveState
{
    public Dictionary<string, string> placematDecorations = new Dictionary<string, string>();
    public int scrap;
    public List<Tuple<string, int, string>> cows = new List<Tuple<string, int, string>>();
    public List<Tuple<long, string, string>> aliens = new List<Tuple<long, string, string>>();
    public long timeTillCanSpawnAnAlien;
    public Dictionary<string, int> ownedCows = new Dictionary<string, int>();
    public List<string> ownedItems = new List<string>();
    public List<string> ownedLamps = new List<string>();
    public List<Tuple<string, int>> alienGifts = new List<Tuple<string, int>>();
    public long timeLeftGame = 0;
    public List<string> discoveredAliens = new List<string>();
    public int tutorialState;
    public long gameStartTime;
    public Dictionary<string, float> planetTheta = new Dictionary<string, float>();
    public float spawningTimer;
    public int musicState;
    public bool upgradedShip;
    public Tuple<string, long, long, Tuple<float, float>, Tuple<float, float>> travelLocation;
    public Tuple<float, float> targetPosition;
    public bool beenToMercury;
    public string placedLampCurrent = "LavaLampBlue";

    public SaveState(PlayerStats ps) 
    {
        placematDecorations = ps.PlacematDecorations;
        scrap = ps.Scrap;
        cows = ps.Cows;
        aliens = ps.Aliens;
        timeTillCanSpawnAnAlien = ps.TimeTillCanSpawnAnAlien;
        ownedCows = ps.OwnedCows;
        ownedItems = ps.OwnedItems;
        alienGifts = ps.AlienGifts;
        timeLeftGame = ps.TimeLeftGame;
        discoveredAliens = ps.DiscoveredAliens;
        tutorialState = ps.TutorialState;
        gameStartTime = ps.GameStartTime;
        planetTheta = ps.PlanetTheta;
        spawningTimer = ps.SpawningTimer;
        musicState = ps.MusicState;
        upgradedShip = ps.UpgradedShip;
        travelLocation = ps.TravelLocation;
        targetPosition = ps.TargetPosition;
        beenToMercury = ps.BeenToMercury;
        ownedLamps = ps.OwnedLamps;
        placedLampCurrent = ps.PlacedLampCurrent;
    }
}
