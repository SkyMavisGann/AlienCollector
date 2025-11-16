using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class LoadInScene : MonoBehaviour
{
    private PlayerStats ps;
    public AlienSpawner spawner;
    public bool needsSpawnAlien = false;
    
    public void setupScene()
    {
        GameObject pso = GameObject.Find("SaveState");
        if (pso != null)
        {
            ps = pso.GetComponent<PlayerStats>();
            spawner = pso.GetComponent<AlienSpawner>();
        }
        GameObject saveManager = GameObject.Find("SaveState");
        if (saveManager != null)
        {
            //Application.platform == RuntimePlatform.WebGLPlayer
            if (Application.platform == RuntimePlatform.WebGLPlayer)
            {
                string name = "SaveData";
                if (PlayerPrefs.HasKey(name))
                {
                    unpackData(SaveManager.loadData());
                }
            } 
            else
            {
                string path = Application.persistentDataPath + "/game.save";
                if (File.Exists(path))
                {
                    unpackData(SaveManager.loadData());
                }
            }
            
            ps.InitialLoaded = true;
            needsSpawnAlien = true;
        }
    }
    private void Update()
    {
        GameObject pso = GameObject.FindGameObjectWithTag("SaveState");
        if (spawner == null)
        {
            spawner = pso.GetComponent<AlienSpawner>();
        }
    }
    public void unpackData(SaveState save)
    {
        ps.PlacematDecorations = save.placematDecorations;
        ps.Scrap = save.scrap;

        if (save.cows != null && save.cows.GetType()  == typeof(List<Tuple<string, int, string>>))
            ps.Cows = save.cows;

        ps.Aliens = save.aliens;
        ps.TimeTillCanSpawnAnAlien = save.timeTillCanSpawnAnAlien;
        ps.OwnedCows = save.ownedCows;
        ps.OwnedItems = save.ownedItems;
        ps.AlienGifts = save.alienGifts;
        ps.TimeLeftGame = save.timeLeftGame;
        ps.DiscoveredAliens = save.discoveredAliens;
        ps.TutorialState = save.tutorialState;
        ps.GameStartTime = save.gameStartTime;
        ps.PlanetTheta = save.planetTheta;
        ps.SpawningTimer = save.spawningTimer;
        ps.MusicState = save.musicState;
        ps.UpgradedShip = save.upgradedShip;
        ps.TravelLocation = save.travelLocation;
        ps.TargetPosition = save.targetPosition;
        ps.BeenToMercury = save.beenToMercury;
        ps.OwnedLamps = save.ownedLamps;
        ps.PlacedLampCurrent = save.placedLampCurrent;
    }
}
