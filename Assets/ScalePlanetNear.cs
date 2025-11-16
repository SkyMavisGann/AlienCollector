using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScalePlanetNear : MonoBehaviour
{
    private Vector3 originalScale;
    private PlayerStats ps;
    public float scaleAmount = 0.001f;
    public float totalMinutes;
    public string planetName = string.Empty;
    // Start is called before the first frame update
    void Start()
    {
        
        originalScale = transform.localScale;
        transform.localScale = Vector3.zero;
        if (planetName == string.Empty)
        {
            planetName = gameObject.name;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (ps == null)
        {
            ps = GameObject.Find("SaveState").GetComponent<PlayerStats>();
        }
        if (ps.UpgradedShip)
        {
            if (ps.TravelLocation != null && ps.TravelLocation.Item1 == planetName)
            {
                DateTime eta = UnixTime.GetDateTime(ps.TravelLocation.Item2);
                TimeSpan Difference = eta - UnixTime.GetDateTime(UnixTime.Now());
                totalMinutes = Mathf.Clamp((float)Difference.TotalMinutes, 0, Mathf.Infinity);
                float multiplier = Mathf.Clamp(1 - totalMinutes * scaleAmount, 0, Mathf.Infinity);
                transform.localScale = new Vector3(originalScale.x * multiplier, originalScale.y * multiplier, originalScale.z);
            }
            
        }
        
    }
    public DateTime getEta(long offset)
    {
        return UnixTime.GetDateTime(ps.GameStartTime + offset);
    }
}
