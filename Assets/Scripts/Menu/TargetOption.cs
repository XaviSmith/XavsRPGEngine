using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//Menu Option for setting something as a target
public class TargetOption : MenuOption {

    void Start()
    {
        myText = GetComponent<Text>();
    }

    public void SetTarget()
    {
        PlayerManager.instance.currPlayer.SetTarget(myObj.GetComponent<Fighter>());
    }
}
