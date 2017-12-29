using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fireball : SpellAttack {
    public Fireball(string _attackStat, string _defenceStat, int _manaCost) : base(_attackStat, _defenceStat, _manaCost)
    {
    }

    // Use this for initialization
    void Reset () {
        attackName = "Fireball";
        attackStat = "spAtk";
        defenseStat = "spDef";
        critThreshold = 99;
        modifier = 1;
        manaCost = 25;
	}
	
}
