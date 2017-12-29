using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Attacks that change stats (e.g. raise defence, lower enemy defence etc.)
public class ModifierAttack : SpellAttack {

    public string statToChange;

    public ModifierAttack(string _attackStat, string _defenceStat, int _manaCost) : base(_attackStat, _defenceStat, _manaCost)
    {
    }

    public override void Execute(Fighter target)
    {
        user.currMp -= manaCost;
        returnedText = "";
        if (target.status == Fighter.CONDITION.Dead)
        {
            if(targetsPlayers)
            {
                target = FightManager.instance.players[0];
            }
            else
            {
                target = FightManager.instance.enemies[0];
            }
            
        }
        int damage = DamageCalculation(target);
        returnedText += user.myName + " used "  + attackName + "! " + target.myName + "'s " + statToChange + " went ";
        if(targetsPlayers)
        {
            returnedText += "up!";
        }
        else
        {
            returnedText += "down!";
        }

        BattleTextManager.instance.SetText(returnedText);
        target.UpdateHealth(-damage);
    }

}
