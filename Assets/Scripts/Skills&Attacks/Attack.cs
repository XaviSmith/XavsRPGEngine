using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Attack : Action {

    public string attackName;
    public int levelLearned;
    public bool learned = false;
    public string attackStat;    //what stat we use to attack (e.g. special attack or attack)
    public string defenseStat;   //what stat they defend with (e.g. defence or speed)

    public int critThreshold = 99;  //Crits are determined by a random number generator. The crit Threshold is the max. Anything above this number causes a critical hit

    public float modifier = 1f;  //e.g. cure 1x, cura 2x, curaga 3x
    protected string returnedText;

    public override void SetAction()
    {
        PlayerManager.instance.currPlayer.SetAction(this);
    }

    public Attack(string _attackStat, string _defenceStat)
    {
        attackStat = _attackStat;
        defenseStat = _defenceStat;
    }

    public override void Execute(Fighter target)
    {
        returnedText = "";
        if (target.status == Fighter.CONDITION.Dead)
        {
            target = FightManager.instance.enemies[0];
        }
        int damage = DamageCalculation(target);
        returnedText += user.myName + " hit " + target.myName + " For " + damage + " damage!";
        AudioManager.instance.PlaySound(AudioManager.instance.attackSound);
        BattleTextManager.instance.SetText(returnedText);
        target.UpdateHealth(-damage);
    }

    public virtual int DamageCalculation(Fighter target)
    {
        int damage;
        bool crit = critCalculation();

        //damage = Mathf.RoundToInt(user.stats[attackStat] * modifier - target.stats[defenseStat]);
        damage = Mathf.RoundToInt(user.GetStat(attackStat) * modifier - target.GetStat(defenseStat));
        if (damage <= 0)
        {
            damage = 1;
        }

        if (crit)
        {
            returnedText = "Critical Hit! ";
            damage = damage * 2;
        }

        return damage;
    }

    public virtual bool critCalculation()
    {
        int critVal = UnityEngine.Random.Range(0, critThreshold);
        //return (FightManager.instance.currFighter.stats["crit"] > critVal) ?  true : false;
        return (FightManager.instance.currFighter.GetStat("crit") > critVal) ? true : false;
    }

}
