using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyFighter : Fighter {

    //For any items they drop
    public List<Item> items;
    public List<Action> myActions;
    public List<string> flavorText;

    public Image battleSprite;

    public EnemyFighter()
    {
        fighterType = "Enemy";
    }
    public override void Die()
    {
        base.Die();
        battleSprite.color = new Color(0.4f, 0.4f, 0.4f, 0.4f);
    }

    public void SetTurn()
    {
        SelectAction();     
        SelectTarget();
    }
    public void SelectAction()
    {
        int choice = UnityEngine.Random.Range(0, myActions.Count);
        myAction = myActions[choice];
    }

    public void SelectTarget()
    {
        int choice;
        if (status == CONDITION.Confused)
        {
           choice = UnityEngine.Random.Range(0, FightManager.instance.fighters.Count);
           myTarget = FightManager.instance.fighters[choice];
        }
        else
        {
            choice = UnityEngine.Random.Range(0, FightManager.instance.players.Count);
            myTarget = FightManager.instance.players[choice];
        }
       
    }
}
