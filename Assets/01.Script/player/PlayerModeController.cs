using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerModeController : MonoBehaviour
{
    [Header("Ref")]
    [SerializeField] private PlayerMove2D playerMove2D;

    [Header("Visual")]
    [SerializeField] private GameObject VerticallySkin;
    [SerializeField] private GameObject SideSkin;

    private void Awake()
    {
        if (playerMove2D == null)
            playerMove2D = GetComponent<PlayerMove2D>();
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void Start()
    {
        ApplyModeBySceneName(SceneManager.GetActiveScene().name);
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        ApplyModeBySceneName(scene.name);
    }

    private void ApplyModeBySceneName(string sceneName)
    {
        if (sceneName == "Stage" || sceneName == "Game Start Screen")
        {
            SetVerticalMode();
        }
        else if (sceneName == "Boss Screen")
        {
            SetSideMode();
        }
    }

    public void SetVerticalMode()
    {
        if (playerMove2D != null && playerMove2D.CurrentMode != PlayerMove2D.PlayerMode.Vertically)
        {
            playerMove2D.SetMode(PlayerMove2D.PlayerMode.Vertically);

        }
        ApplyCurrentModeVisual();
    }

    public void SetSideMode()
    {
        if (playerMove2D != null && playerMove2D.CurrentMode != PlayerMove2D.PlayerMode.Side)
        {
            playerMove2D.SetMode(PlayerMove2D.PlayerMode.Side);

        }
        ApplyCurrentModeVisual();
    }

    private void ApplyCurrentModeVisual()
    {
        if (playerMove2D == null) return;

        bool isVertical = playerMove2D.CurrentMode == PlayerMove2D.PlayerMode.Vertically;

        if (VerticallySkin != null)
            VerticallySkin.SetActive(isVertical);

        if (SideSkin != null)
            SideSkin.SetActive(!isVertical);
    }
}