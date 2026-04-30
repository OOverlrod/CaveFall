using UnityEngine;

public class ZoneInfo : MonoBehaviour
{
    [SerializeField]
    string zoneName;

    public string GetZoneName() { return zoneName; }
}
