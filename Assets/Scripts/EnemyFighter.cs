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

    public override void Die()
    {
        base.Die();
        battleSprite.color = new Color(0.4f, 0.4f, 0.4f, 0.4f);
    }
}
