using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlanetManager : MonoBehaviour
{
    public List<Tuple<Planets.Planet, long>> PlanetsAndEta = new List<Tuple<Planets.Planet, long>>();
    private PlayerStats ps;
    public Planets.Planet currentPlanet;
    public Tuple<Planets.Planet, long> nextPlanet;
    public long gameStartTime;
    public int reverseTimeRN;
    private string previousTravelLocation;
    private bool prevSetStartOnce = true;
    // Start is called before the first frame update
    void Start()
    {
        PlanetsAndEta.Add(new Tuple<Planets.Planet, long>(Planets.Planet.Earth, 0));
        PlanetsAndEta.Add(new Tuple<Planets.Planet, long>(Planets.Planet.Moon, UnixTime.GetUnixTimeMinutes(4320)));
        PlanetsAndEta.Add(new Tuple<Planets.Planet, long>(Planets.Planet.Mars, UnixTime.GetUnixTimeMinutes(8640)));
        PlanetsAndEta.Add(new Tuple<Planets.Planet, long>(Planets.Planet.Asteroids, UnixTime.GetUnixTimeMinutes(12960)));
        PlanetsAndEta.Add(new Tuple<Planets.Planet, long>(Planets.Planet.Jupiter, UnixTime.GetUnixTimeMinutes(15840)));
        PlanetsAndEta.Add(new Tuple<Planets.Planet, long>(Planets.Planet.Saturn, UnixTime.GetUnixTimeMinutes(20160))); 
        PlanetsAndEta.Add(new Tuple<Planets.Planet, long>(Planets.Planet.Uranus, UnixTime.GetUnixTimeMinutes(25920)));
        PlanetsAndEta.Add(new Tuple<Planets.Planet, long>(Planets.Planet.Neptune, UnixTime.GetUnixTimeMinutes(33120)));
        //for upgraded behavior
        
    }

    private void Update()
    {
        

        reverseTimeRN = (int)(UnixTime.GetUnixTime(DateTime.Now) - gameStartTime) / 60;
        if (ps == null)
        {
            ps = GameObject.Find("SaveState").GetComponent<PlayerStats>();
        }
        gameStartTime = ps.GameStartTime;
        if (ps.TravelLocation != null && prevSetStartOnce && ps != null)
        {
            prevSetStartOnce = false;
            previousTravelLocation = ps.TravelLocation.Item1;
        }
        if (!ps.UpgradedShip)
        {
            currentPlanet = Planets.Planet.Space;
            for (int i = 0; i < PlanetsAndEta.Count; i++)
            {
                Tuple<Planets.Planet, long> tuple = PlanetsAndEta[i];
                long timeTillNext = (gameStartTime + tuple.Item2) - UnixTime.GetUnixTime(DateTime.Now);

                if (Mathf.Abs(timeTillNext) < UnixTime.GetUnixTimeMinutes(600))
                {
                    currentPlanet = tuple.Item1;
                }

            }

            nextPlanet = new Tuple<Planets.Planet, long>(Planets.Planet.Space, UnixTime.GetUnixTimeMinutes(35120));
            for (int i = 0; i < PlanetsAndEta.Count; i++)
            {
                if ((gameStartTime + PlanetsAndEta[i].Item2) > UnixTime.GetUnixTime(DateTime.Now))
                {
                    nextPlanet = PlanetsAndEta[i];
                    break;
                }
            }
        } else
        {
            if (ps.TravelLocation != null)
            {
                //I want to allow it to update current planet if you change travel locations
                if (previousTravelLocation != ps.TravelLocation.Item1)
                {
                    previousTravelLocation = ps.TravelLocation.Item1;
                    currentPlanet = Planets.Planet.Space;
                }

                UpdateCurrentPlanet();
            }
        }
    }

    private void UpdateCurrentPlanet()
    {

        TimeSpan Difference = UnixTime.GetDateTime(ps.TravelLocation.Item2) - UnixTime.GetDateTime(UnixTime.Now());
        //if you're 1 minute away update your current planet to this
        if (Difference < TimeSpan.FromSeconds(2))
        {
                
            int AmountOfCategories = System.Enum.GetValues(typeof(Planets.Planet)).Length;
            for (int i = 0; i < AmountOfCategories; i++)
            {
                Planets.Planet CurrentPlanet = (Planets.Planet)i;
                if (ps.TravelLocation.Item1 == CurrentPlanet.ToString())
                {
                    currentPlanet = CurrentPlanet;
                    break;
                }
            }
        }
    }

    public DateTime getEta(long offset)
    {
        return UnixTime.GetDateTime(gameStartTime + offset);
    }

}
