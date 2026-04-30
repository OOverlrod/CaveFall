using UnityEngine;

public class BreakableFloor : MonoBehaviour
{
    private bool isBroken = false;

    public void Break()
    {
        if (isBroken) return;
        isBroken = true;
        Destroy(gameObject);
    }
}
