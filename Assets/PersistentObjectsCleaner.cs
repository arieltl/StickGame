using UnityEngine;
using UnityEngine.SceneManagement;

public class PersistentObjectsCleaner : MonoBehaviour
{
    public static void DestroyAllPersistentObjects()
    {
        // Create a temporary scene to isolate all root objects
        Scene tempScene = SceneManager.CreateScene("TempScene");

        // Move all root GameObjects from the persistent scene to the temporary scene
        foreach (GameObject obj in GetDontDestroyOnLoadObjects())
        {
            SceneManager.MoveGameObjectToScene(obj, tempScene);
        }

        // Unload the temporary scene, destroying all objects within it
        SceneManager.UnloadSceneAsync(tempScene);
    }

    private static GameObject[] GetDontDestroyOnLoadObjects()
    {
        // Create an empty GameObject to temporarily enter the persistent scene
        GameObject temp = new GameObject("TempObject");
        DontDestroyOnLoad(temp);

        // Find the persistent scene by getting the scene of the temp object
        Scene persistentScene = temp.scene;

        // Get all root GameObjects in the persistent scene
        GameObject[] rootObjects = persistentScene.GetRootGameObjects();

        // Destroy the temp object before returning
        Destroy(temp);

        return rootObjects;
    }
}