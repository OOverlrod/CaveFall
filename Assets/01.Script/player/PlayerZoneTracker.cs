using System.Collections.Generic;
using UnityEngine;

public class PlayerZoneTracker : MonoBehaviour
{
    public string[] allZoneNames =
      {
        "NextZone",
        "BossZone"
    };

    public List<string> currentZones = new List<string>();

    private void OnTriggerEnter2D(Collider2D collision)
    {
        ZoneInfo zone = collision.GetComponent<ZoneInfo>();

        if (zone != null)
        {
            if (!currentZones.Contains(zone.GetZoneName()))
            {
                currentZones.Add(zone.GetZoneName());
            }
        }

        if (currentZones.Contains("NextZone"))
        {
            if (StageProgressManager.Instance != null)
            {
                StageProgressManager.Instance.IncreaseLoopLevel();
            }
            GameSceneManager.Instance.LoadSceneByName("Stage");
        }

        if (currentZones.Contains("BossZone"))
        {
            GameSceneManager.Instance.LoadSceneByName("Boss Screen");
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        ZoneInfo zone = collision.GetComponent<ZoneInfo>();

        if (zone != null)
        {
            if (currentZones.Contains(zone.GetZoneName()))
            {
                currentZones.Remove(zone.GetZoneName());
            }
        }
    }
}
