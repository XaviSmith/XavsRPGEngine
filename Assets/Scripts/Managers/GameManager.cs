using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;
using UnityEngine.SceneManagement;

//anything we need to manage throughout the game, like party members already recruited
public class GameManager : Singleton<GameManager>
{

    public List<int> recruitedFighters;

    private Save CreateSaveGameObject()
    {
        Save save = new Save();
        foreach (PlayerFighter partyMember in PlayerManager.instance.players)
        {
            save.party.Add(partyMember);
        }

        save.currScene = SceneManager.GetActiveScene().name;
        save.recruitedFighters = recruitedFighters;
        //save.playerPos = 
        return save;
    }

    public void SaveGame()
    {
        // 1
        Save save = CreateSaveGameObject();

        // 2
        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Create(Application.persistentDataPath + "/gamesave.save");
        bf.Serialize(file, save);
        file.Close();

        // 3

        Debug.Log("Game Saved");
    }

    public void LoadGame()
    {
        // 1
        if (File.Exists(Application.persistentDataPath + "/gamesave.save"))
        {

            // 2
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(Application.persistentDataPath + "/gamesave.save", FileMode.Open);
            Save save = (Save)bf.Deserialize(file);
            file.Close();

            // 3
            recruitedFighters = save.recruitedFighters;
            foreach (PlayerFighter partyMember in save.party)
            {
                PlayerManager.instance.players.Add(partyMember);
            }

            // 4
            //ClearFighters();

            Debug.Log("Game Loaded");

        }
        else
        {
            Debug.Log("No game saved!");
        }
    }

    public void ClearFighters()
    {
    }

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
        if (scene.name == "Overworld")
        {
            //ClearFighters();
        }
    }
}
