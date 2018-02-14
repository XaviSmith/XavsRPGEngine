using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Action : MonoBehaviour {

    public Fighter user;
    public Fighter myTarget;

    // Use this for initialization
    void Start () {
        user = GetComponent<Fighter>();
	}

    public virtual void SetAction()
    {
        //PlayerManager.instance.currPlayer.SetAction(this);
        FightManager.instance.currFighter.SetAction(this);
    }

    public virtual void SetTarget(Fighter target)
    {
        myTarget = target;
    }

    public abstract void Execute(Fighter target); //String because we always want there to be some output telling the player what happened (e.g. Player healed 15 health! etc).
}
