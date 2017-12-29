using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerNameBox : Singleton<PlayerNameBox> {

    public Text myText;

    void Start()
    {
        myText = GetComponent<Text>();
    }

}
