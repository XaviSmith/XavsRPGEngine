using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Heal2 : Heal {
    public Heal2(string _attackStat, string _defenceStat, int _manaCost) : base(_attackStat, _defenceStat, _manaCost)
    {
    }

    public new void Reset()
    {
        attackName = "Heal+";
        modifier = 2;
        manaCost = 60;
        targetsPlayers = true;
    }
}
