using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Heal : SpellAttack {

    //Reset() lets us give a different default value to subclass
    public void Reset()
    {
        attackName = "Heal";
        modifier = 1;
        manaCost = 40;
        targetsPlayers = true;
    }

    public Heal(string _attackStat, string _defenceStat, int _manaCost) : base(_attackStat, _defenceStat, _manaCost)
    {
    }

    public override void SetAction()
    {
        base.SetAction();
    }

    public override void Execute(Fighter target)
    {
        int gain = (int)( ( Mathf.CeilToInt(user.spAtk / 20) * 50 + (target.hp / 4) ) *modifier);
        user.currMp -= manaCost;
        returnedText = user.myName + " healed " + target.myName + " For " + gain + " hp!";
        BattleTextManager.instance.SetText(returnedText);
        target.UpdateHealth(gain);
    }
}
