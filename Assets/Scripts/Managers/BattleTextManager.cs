using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BattleTextManager : TextManager {


    void OnEnable()
    {
        myTextObj.gameObject.SetActive(true);
    }

    void OnDisable()
    {
        myTextObj.gameObject.SetActive(false);
    }

   
}
