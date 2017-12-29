using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class Save {
    public List<PlayerFighter> party = new List<PlayerFighter>();
    public List<int> recruitedFighters = new List<int>();

    public string currScene;
    //public Vector2 playerPos;

    public int hits = 0;
    public int shots = 0;
}
