using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BattleSpriteManager : Singleton<BattleSpriteManager> {
    public List<GameObject> spritePos;
    public List<Image> myImg;
    // Use this for initialization
    public override void Awake() {
        base.Awake();
        foreach(GameObject obj in spritePos)
        {
            myImg.Add(obj.GetComponent<Image>());
        }
    }

    // Update is called once per frame
    void Update() {

    }


}
