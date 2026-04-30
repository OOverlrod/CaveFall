using UnityEngine;

public class SceneButton : MonoBehaviour
{
    public void GoToTitleScene()
    {
        Time.timeScale = 1f;

        if (DontDestroyPlayer.Instance != null)
        {
            Destroy(DontDestroyPlayer.Instance.gameObject);
        }

        if (StageProgressManager.Instance != null)
        {
            StageProgressManager.Instance.ResetLoopLevel();
        }

        if (RunStatsManager.Instance != null)
        {
            RunStatsManager.Instance.ResetStats();
        }

        GameSceneManager.Instance.LoadSceneByName("Title Screen");
    }

    public void GoToGameStartScene()
    {
        Time.timeScale = 1f;

        if (DontDestroyPlayer.Instance != null)
        {
            Destroy(DontDestroyPlayer.Instance.gameObject);
        }

        if (RunStatsManager.Instance != null)
        {
            RunStatsManager.Instance.ResetStats();
        }

        GameSceneManager.Instance.LoadSceneByName("Game Start Screen");
    }

    public void ReloadCurrentScene()
    {
        Time.timeScale = 1f;

        GameSceneManager.Instance.ReloadCurrentScene();
    }

    public void QuitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
    Application.Quit();
#endif
    }
}