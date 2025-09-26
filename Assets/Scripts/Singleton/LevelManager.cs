using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : AbstractSingleton<LevelManager>
{
    [Header("Lista delle scene di gioco")]
    [SerializeField] private string[] scenes;

    public string[] Scenes => scenes;

    public override bool IsDestroyedOnLoad() => false;
    public override bool ShouldDetatchFromParent() => true;

    public void LoadLevel(string sceneName)
    {
        if (string.IsNullOrEmpty(sceneName))
        {
            Debug.LogWarning("Scene name nullo o vuoto.");
            return;
        }

        Debug.Log($"Caricamento scena: {sceneName}");
        SceneManager.LoadScene(sceneName);
    }

    public void SaveCompletedLevel(string sceneName)
    {
        SaveData data = SaveSystem.LoadOrInitialize();

        var level = data.levelsProgress.Find(l => l.levelID == sceneName);
        if (level == null)
        {
            Debug.LogWarning($"Il livello {sceneName} non è stato trovato nei progressi.");
            return;
        }

        // Marca il livello come completato
        level.isCompleted = true;
        Debug.Log($"Livello completato: {sceneName}");

        // Sblocca il prossimo livello se esiste
        int nextOrder = level.order + 1;
        var nextLevel = data.levelsProgress.Find(l => l.order == nextOrder);
        if (nextLevel != null && !nextLevel.isUnlocked)
        {
            nextLevel.isUnlocked = true;
            Debug.Log($"Sbloccato il livello successivo: {nextLevel.levelID}");
        }

        SaveSystem.Save(data);
    }
}