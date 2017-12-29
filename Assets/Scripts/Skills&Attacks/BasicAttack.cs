using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BasicAttack : Attack {

    public string myText;

    public BasicAttack(string _attackStat, string _defenceStat) : base(_attackStat, _defenceStat)
    {
        learned = true;
    }

    public override void SetAction()
    {
        PlayerManager.instance.currPlayer.SetAction(PlayerManager.instance.currPlayer.basicAttack);
    }

    public override void Execute(Fighter target)
    {
        base.Execute(target);
        
    }


}
