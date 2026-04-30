using UnityEngine;

public class BossIInvincibility : MonoBehaviour, IInvincibility
{
    [SerializeField] private bool isInvincible;

    public bool IsInvincible => isInvincible;

    public void SetInvincible(bool value)
    {
        isInvincible = value;
    }
}
