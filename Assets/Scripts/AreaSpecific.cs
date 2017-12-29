using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

//For objects we only want active in certain scenes. If we go to a scene not listed here, destroy that object.
[RequireComponent(typeof(DontDestroyOnLoad))]
public class AreaSpecific : MonoBehaviour {

    public List<string> activeAreas;

    //Let us take actions as soon as certain scenes load
    void OnEnable()
    {
        //Tell our 'OnLevelFinishedLoading' function to start listening for a scene change as soon as this script is enabled.
        SceneManager.sceneLoaded += OnLevelFinishedLoading;
    }

    void OnDisable()
    {
        //Tell our 'OnLevelFinishedLoading' function to stop listening for a scene change as soon as this script is disabled. Remember to always have an unsubscription for every delegate you subscribe to!
        SceneManager.sceneLoaded -= OnLevelFinishedLoading;
    }

    void OnLevelFinishedLoading(Scene scene, LoadSceneMode mode)
    {
        scene = SceneManager.GetActiveScene();
        if (!activeAreas.Contains(scene.name))
        {
            Destroy(gameObject);
        }
    }
}
