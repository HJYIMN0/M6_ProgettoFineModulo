using UnityEngine;
using UnityEngine.SceneManagement;

public class TestLevels : MonoBehaviour {
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.L))
        {
            Debug.Log("Pressed L ? Loading next level");
            LevelManager.Instance.LoadNextLevel();
        }

        if (Input.GetKeyDown(KeyCode.K))
        {
            Debug.Log("Pressed K ? Loading previous level");

            string currentSceneName = SceneManager.GetActiveScene().name;
            var currentLevel = LevelManager.Instance.GetLevelInfo(currentSceneName);

            if (currentLevel == null)
            {
                Debug.LogWarning($"Current scene '{currentSceneName}' not found in LevelConfiguration.");
                return;
            }

            int previousOrder = currentLevel.order - 1;
            var previousLevel = LevelManager.Instance.GetLevelInfoByOrder(previousOrder);

            if (previousLevel != null)
            {
                LevelManager.Instance.LoadLevel(previousLevel.levelID);
            }
            else
            {
                Debug.Log("No previous level found.");
            }
        }
    }
}
