using UnityEngine;

public class CameraTrigger : MonoBehaviour
{
    [Header("Ref")]
    [SerializeField] private CameraMove cameraMove;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.CompareTag("Player"))  return;
        if (cameraMove != null )
            cameraMove.StartFollowY();
    }
}
