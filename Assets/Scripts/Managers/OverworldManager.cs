using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Script to handle removing overworld elements, tripping flags, etc etc. 
public class OverworldManager : Singleton<OverworldManager> {

    public List<GameObject> removableObjs;  //List for objects that can be removed. Everything inside this will default to being turned off, and turn on at start. Remove objects from this list to keep them turned off.

    void Start()
    {
        /*foreach(GameObject g in removableObjs)
        {
            g.SetActive(true);
        }*/
    }

	
}
