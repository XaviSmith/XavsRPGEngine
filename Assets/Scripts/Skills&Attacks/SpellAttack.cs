﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Same as attacks, except we need to check if we have enough mp to select it as an action, then subtract the mp once it's used. 

public class SpellAttack : Attack {

    public int manaCost;
    [Tooltip("Check true if this skill targets party members (e.g. healing, buffing), false if it affects enemies (e.g. a fireball)")]
    public bool targetsPlayers; //true for if it's a spell that affects Players (e.g. heal), false if it affects enemies (e.g. fireball);

    public SpellAttack(string _attackStat, string _defenceStat, int _manaCost) : base(_attackStat, _defenceStat)
    {
        manaCost = _manaCost;
    }

    public override void SetAction()
    {
        if(user.currMp > manaCost)
        {
            base.SetAction();
            MenuManager.instance.SelectFighter(targetsPlayers);
        }
        
    }

    public override void Execute(Fighter target)
    {
        user.currMp -= manaCost;
        base.Execute(target);
    }
}
