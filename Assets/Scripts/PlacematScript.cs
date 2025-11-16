using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlacematScript : MonoBehaviour
{
    public GameObject SpawnedDecoration;
    public int SpawningRandomRange = 4;
    private PlacementManager placementManager;
    private Image image;
    private PlayerStats ps;

    private string PlacematAllowedToSpawn;
    private bool needsToSpawn;
    // Start is called before the first frame update
    void Start()
    {
        image = GetComponent<Image>();
        
    }

    // Update is called once per frame
    void Update()
    {
        if (ps == null)
        {
            ps = GameObject.Find("SaveState").GetComponent<PlayerStats>();
        }
        if (placementManager == null)
        {
            GameObject placeOjb = GameObject.Find("PlacementManager");
            if (placeOjb != null)
            {
                placementManager = placeOjb.GetComponent<PlacementManager>();
                
            }
        }
        ColorInMats();

        //when the object exists in playerstats but is not spawned, then spawn it
        if (ps.PlacematDecorations.ContainsKey(transform.gameObject.name))
        {
            string ResourceName = ps.PlacematDecorations[transform.gameObject.name];
            if (ResourceName != null && SpawnedDecoration == null && Resources.Load<GameObject>("Decor/" + ResourceName) != null)
            {
                Debug.Log("Respawning prop: " + ResourceName);
                SpawnResource(ResourceName);

                RespawnAlien();

                //SpawnAlienWhenGone();
            }
        }
        

    }
    private void setActiveAlienButtons(bool active)
    {
        List<AlienData> AllAliensOnField = new List<AlienData>(GameObject.FindObjectsOfType<AlienData>());
        foreach(AlienData alienData in AllAliensOnField)
        {
            alienData.gameObject.GetComponent<Button>().targetGraphic.raycastTarget = active;
        }
    }
    private void ColorInMats()
    {
        //Spawn in yellow mats
        if (placementManager != null)
        {
            if (placementManager.CurrentlyPlacing)
            {
                image.color = new Color(image.color.r, image.color.g, image.color.b, 50);
                GetComponent<CustomButton>().enabled = true;
                setActiveAlienButtons(false);
            }
            else
            {
                image.color = new Color(image.color.r, image.color.g, image.color.b, 0);
                GetComponent<CustomButton>().enabled = false;
                setActiveAlienButtons(true);
            }
        }
        else
        {
            image.color = new Color(image.color.r, image.color.g, image.color.b, 0);
        }
    }
    
    //it spawns without leaving the game, its not supposed to do that
    //i did this to fix some bug  that i don't remember, wish i used github =((((
    //at line 144 i made a comment on fixing the problem it is when i switched from spawning at each place to spawning above in placematManager
    // how could this have caused the problem? you remembered before,
    public void SpawnAlienWhenOutOfFocus(PlayerStats ps)
    {
        //needs to be done after loading in placemat decor
        LoadInScene loader = GameObject.Find("LoadIn").GetComponent<LoadInScene>();

        //in the tutorial it should 100% likely to spawn
        if (ps.TutorialState == 9)
        {
            SpawningRandomRange = 0;
        } else
        {
            SpawningRandomRange = 3;
        }
        int rand = UnityEngine.Random.Range(0, SpawningRandomRange);
        if (UnixTime.GetUnixTime(DateTime.Now) > ps.TimeTillCanSpawnAnAlien)
        {
            if (SpawnedDecoration != null && rand == 0)
            {
                Debug.Log("AlienTime");
                if (loader.spawner != null)
                {
                    Debug.Log(loader.spawner != null);
                    loader.spawner.SpawnAnyAlien(SpawnedDecoration);
                }
            }
        }
        else
        {
            Debug.Log("Time Before Can spawn next alien: " + (ps.TimeTillCanSpawnAnAlien - UnixTime.GetUnixTime(DateTime.Now)).ToString());
        }
    }

    private void showData()
    {
        FileLogger.Log("Current Time: " + DateTime.Now.ToString() + "\nTime left game: " + UnixTime.GetDateTime(ps.TimeLeftGame) +
            ", Seconds passed " + (UnixTime.GetUnixTime(DateTime.Now) - ps.TimeLeftGame).ToString() +
            " Time since last logged on " + (UnixTime.GetUnixTime(DateTime.Now) - ps.TimeLeftGame) / UnixTime.GetUnixTimeMinutes(1));
    }

    //depricated
    private void SpawnGiftsWhenGone()
    {
        showData();
        
        if (SpawnedDecoration != null)
        {
            //spawn aliens if gone for more then 20 minutes, spawn one for each 20
            long amountOfAliensToSpawn = (UnixTime.GetUnixTime(DateTime.Now) - ps.TimeLeftGame) / UnixTime.GetUnixTimeMinutes((long)(20.0f / ps.TimeScale));
            FileLogger.Log("Amount of aliens spawned when left: " + amountOfAliensToSpawn.ToString());
            int rand;
            //because this can run like 10 times each individually on a placemat one placemat can hog all the cows most of the time
            //parallel programming semaphores maybe
            for (int i = 0; i < amountOfAliensToSpawn; ++i)
            {
                rand = UnityEngine.Random.Range(0, 2);
                if (rand == 0)
                {
                
                FileLogger.Log("Cows Count: " + ps.Cows.Count);
                    if (ps.Cows.Count > 0)
                    {
                        GameObject cow = null;
                        //cow = a cow with less then 100 risk
                        foreach (GameObject co in GameObject.FindGameObjectsWithTag("Cow"))
                        {
                            if (co.GetComponent<CowData>().risk < 100)
                            {
                                cow = co;
                            }
                        }
                        FileLogger.Log("Cows with less then 100 risk exists: " + (cow != null).ToString());
                        if (cow != null)
                        {
                            DecorData data = SpawnedDecoration.GetComponent<DecorData>();

                            PlanetManager pm = GameObject.Find("SaveState").GetComponent<PlanetManager>();

                            List<GameObject> FilteredListWithPlanet = GetAliensPossible(data, pm);

                            int randAlien = UnityEngine.Random.Range(0, FilteredListWithPlanet.Count);
                            AlienData alienData = FilteredListWithPlanet[randAlien].GetComponentInChildren<AlienData>();

                            if (cow != null)
                            {
                                cow.GetComponent<CowData>().risk += (30 - cow.GetComponent<CowData>().defense);
                                for (int j = 0; j < ps.Cows.Count; j++)
                                {
                                    Tuple<string, int, string> thisCow = ps.Cows[j];
                                    if (thisCow.Item1 == cow.name)
                                    {
                                        ps.Cows[j] = new Tuple<string, int, string>(thisCow.Item1, cow.GetComponent<CowData>().risk, thisCow.Item3);
                                    }
                                }
                            }
                            FileLogger.Log("adding offering: " + alienData.transform.parent.gameObject.name);
                            ps.AlienGifts.Add(new Tuple<string, int>(alienData.transform.parent.gameObject.name, UnityEngine.Random.Range(1 + alienData.wealth, alienData.wealth * 2)));

                        }
                        else
                        {
                            break;
                        }
                    }
                    else
                    {
                        break;
                    }
                }
            }
        }
    }

    public bool SpawnOneGift(PlayerStats ps)
    {
        int rand;
        rand = UnityEngine.Random.Range(0, 2);
        if (rand == 0)
        {
            if (SpawnedDecoration != null)
            {

                FileLogger.Log("Cows Count: " + ps.Cows.Count);
                if (ps.Cows.Count > 0)
                {
                    GameObject cow = null;
                    //cow = a cow with less then 100 risk
                    foreach (GameObject co in GameObject.FindGameObjectsWithTag("Cow"))
                    {
                        if (co.GetComponent<CowData>().risk < 100)
                        {
                            cow = co;
                        }
                    }
                    FileLogger.Log("Cows with less then 100 risk exists: " + (cow != null).ToString());
                    if (cow != null)
                    {
                        DecorData data = SpawnedDecoration.GetComponent<DecorData>();

                        PlanetManager pm = GameObject.Find("SaveState").GetComponent<PlanetManager>();

                        List<GameObject> FilteredListWithPlanet = GetAliensPossible(data, pm);

                        int randAlien = UnityEngine.Random.Range(0, FilteredListWithPlanet.Count);
                        AlienData alienData = FilteredListWithPlanet[randAlien].GetComponentInChildren<AlienData>();

                        if (cow != null)
                        {
                            cow.GetComponent<CowData>().risk += (30 - cow.GetComponent<CowData>().defense);
                            for (int j = 0; j < ps.Cows.Count; j++)
                            {
                                Tuple<string, int, string> thisCow = ps.Cows[j];
                                if (thisCow.Item1 == cow.name)
                                {
                                    ps.Cows[j] = new Tuple<string, int, string>(thisCow.Item1, cow.GetComponent<CowData>().risk, thisCow.Item3);
                                }
                            }
                        }
                        FileLogger.Log("adding offering: " + alienData.transform.parent.gameObject.name);
                        ps.AlienGifts.Add(new Tuple<string, int>(alienData.transform.parent.gameObject.name, UnityEngine.Random.Range(1 + alienData.wealth, alienData.wealth * 2)));
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                } else
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
        } else
        {
            return true;
        }
    }

    private List<GameObject> GetAliensPossible(DecorData data, PlanetManager pm)
    {
        List<GameObject> FilteredListWithPlanet = new List<GameObject>();
        for (int k = 0; k < data.PossibleAliensToSpawn.Count; k++)
        {
            long favoritePlanetTime = -1;
            for (int m = 0; m < pm.PlanetsAndEta.Count; m++)
            {
                if (data.PossibleAliensToSpawn[k].GetComponentInChildren<AlienData>().FavoritePlanet == pm.PlanetsAndEta[m].Item1)
                {
                    favoritePlanetTime = ps.GameStartTime + pm.PlanetsAndEta[m].Item2;
                }
            }

            long EndDateForPlanet = favoritePlanetTime + UnixTime.GetUnixTimeMinutes(600);
            long StartDateForPlanet = favoritePlanetTime + UnixTime.GetUnixTimeMinutes(600);

            if (data.PossibleAliensToSpawn[k].GetComponentInChildren<AlienData>().FavoritePlanet == pm.currentPlanet || data.PossibleAliensToSpawn[k].GetComponentInChildren<AlienData>().FavoritePlanet == Planets.Planet.Space ||
                 (ps.TimeLeftGame < EndDateForPlanet && UnixTime.GetUnixTime(DateTime.Now) > StartDateForPlanet))
            {
                FilteredListWithPlanet.Add(data.PossibleAliensToSpawn[k]);
            }
        }
        return FilteredListWithPlanet;
    }

    //when the alien exists on one of the decorations but isn't spawned on it
    private void RespawnAlien()
    {
        if (SpawnedDecoration != null)
        {
            DecorData data = SpawnedDecoration.GetComponent<DecorData>();
            if (data.AlienAttached == null)
            {
                foreach (Tuple<long, string, string> alien in ps.Aliens)
                {
                    if (alien.Item2 == SpawnedDecoration.name)
                    {
                        GameObject alienObject = Resources.Load<GameObject>("Aliens/" + alien.Item3);
                        data.AlienAttached = Instantiate(alienObject,
                                SpawnedDecoration.transform.position,
                                SpawnedDecoration.transform.rotation, SpawnedDecoration.transform);

                        data.AlienAttached.name = alienObject.name;
                        if (data.AlienAttached.GetComponentInChildren<AlienData>() != null)
                        {
                            data.AlienAttached.GetComponentInChildren<AlienData>().decorAttachedTo = data;
                        }
                    }
                }
            }
        }
    }
    public void placeHere()
    {
        if (placementManager != null && placementManager.CurrentlyPlacing)
        {
            //delete all already existant objects
            List<PlacematScript> AlreadyExistantObject = new List<PlacematScript>(transform.parent.GetComponentsInChildren<PlacematScript>());
            foreach (PlacematScript pms in AlreadyExistantObject)
            {
                if (pms.SpawnedDecoration != null && pms.SpawnedDecoration.name == placementManager.ObjectToPlace.name)
                {

                    GameObject obj = pms.SpawnedDecoration;
                    ps.PlacematDecorations.Remove(pms.gameObject.name);
                    pms.SpawnedDecoration = null;
                    Destroy(obj);
                }
            }

            //fill out array
            if (ps.PlacematDecorations.ContainsKey(transform.gameObject.name))
            {
                ps.PlacematDecorations[transform.gameObject.name] = placementManager.ObjectToPlace.name;
            } 
            else
            {
                ps.PlacematDecorations.Add(transform.gameObject.name, placementManager.ObjectToPlace.name);
            }
            
            string ResourceName = ps.PlacematDecorations[transform.gameObject.name];
            if (SpawnedDecoration != null)
            {
                if (SpawnedDecoration.GetComponent<DecorData>().AlienAttached != null)
                {
                    foreach(Tuple<long, string, string> alienInformaiton in ps.Aliens)
                    {
                        if (alienInformaiton.Item2 == SpawnedDecoration.name)
                        {
                            ps.Aliens.Remove(alienInformaiton);
                            break;
                        }
                    }
                }
                Destroy(SpawnedDecoration);
                
            }

            

            
            SpawnResource(ResourceName);
            placementManager.ObjectToPlace = null;
            placementManager.CurrentlyPlacing = false;
        }
        
    }

    private void SpawnResource(string ResourceName)
    {
        GameObject Resource = Resources.Load<GameObject>("Decor/" + ResourceName);
        SpawnedDecoration = Instantiate(Resource, transform.position, transform.rotation, transform);
        SpawnedDecoration.transform.localPosition = Resource.transform.localPosition;
        SpawnedDecoration.name = ResourceName;
    }
}
