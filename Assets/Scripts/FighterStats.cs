using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class FighterStats {

    public enum Stats { hp, mp, atk, def, spAtk, spDef, spd, crit  }
    public Stats key;
    public int value;

}
