using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Fighter : MonoBehaviour {

    public string myName;
    public int myID;    //Unique ID that lets us distinguish characters, bosses, enemy types etc. 1-30 marks party memebers. 30+ marks enemies

    /* I originally tried to store stats in a Dictionary<string,int> e.g. stats["Hp" : 100], however Unity does not support showing Dictionaries or Hashtables in the inspector, 
     * even with System.Serializable. Storing them as a list/array of structs e.g. (Stat {string name, int value}) also looked messy and ugly in the inspector
     * For the user's convenience, stats are stored as ints, and we can match corresponding strings with those using GetStat and SetStat, e.g. GetStat("hp");
     * This allows us a small "dictionary" that's viewable in the inspector, but can still reference stats by string (e.g. BuffStat(string statName))
     */

    public int hp;
    public int currHp;
    public int mp;
    public int currMp;
    public int atk;
    public int def;
    public int spAtk;
    public int spDef;
    public int spd;
    public int lv;
    public int exp;
    public int gold;

    [Tooltip("For when we use skills like attack up/down or def up/down on atk, def, spAtk, spDef, and speed respectively for this character.")]
    public List<double> statModifiers = new List<double>(new double[] { 1, 1, 1, 1, 1 });
    [Tooltip("Default 'Fight->Attack' option")]
    public BasicAttack basicAttack; //Every fighter needs a basic attack.
    [Tooltip("NOTE: DUE TO HOW UNITY WORKS, EACH SKILL SCRIPT MUST BE ON THE CHARACTER OBJECT. All the skills our fighter can use. levelLearned tells us what level they learn the skill at (only used for players), learned lets us know if they know and can use the skill")]
    public List<Attack> skills;

    public int GetStat(string statName)
    {
        switch (statName)
        {
            case "hp":
                return hp;

            case "mp":
                return mp;

            case "currHp":
                return currHp;

            case "currMp":
                return currMp;

            case "atk":
                return (int)(atk * statModifiers[0]);

            case "def":
                return (int)(def * statModifiers[1]);

            case "spAtk":
                return (int)(spAtk * statModifiers[2]);

            case "spDef":
                return (int)(spDef * statModifiers[3]);

            case "spd":
                return (int)(spd * statModifiers[4]);

            default: return 1;
        }
    }

    public int GetRawStat(string statName)
    {
        switch (statName)
        {
            case "hp":
                return hp;

            case "mp":
                return mp;

            case "currHp":
                return currHp;

            case "currMp":
                return currMp;

            case "atk":
                return atk;

            case "def":
                return def;

            case "spAtk":
                return spAtk;

            case "spDef":
                return spDef;

            case "spd":
                return def;

            default: return 1;
        }
    }
    public void SetStat(string statName, int value)
    {
        switch (statName)
        {
            case "hp":
                hp = value;
                break;

            case "mp":
                mp = value;
                break;

            case "currHp":
                currHp = value;
                break;

            case "currMp":
                currMp = value;
                break;

            case "atk":
                atk = value;
                break;
            case "def":
                def = value;
                break;

            case "spAtk":
                spAtk = value;
                break;

            case "spDef":
                spDef = value;
                break;
            case "spd":
                spd = value;
                break;
            default:
                break;
        }
    }

    public enum CONDITION { Ok, Poison, Sleep, Dead }
    public CONDITION status;

    public bool turnSet;
    public bool actionSet;

    public Fighter myTarget;
    //public delegate void Action();
    public Action myAction;
    public MenuOption menuObj;
    
	public virtual void SetTarget(Fighter target)
    {
        if(target.status != CONDITION.Dead)
        {
            myTarget = target;
            actionSet = true;
        }
        
    }

    public virtual void SetAction(Action someAction)
    {
        myAction = someAction;
        myAction.user = this;
    }

    public virtual void clearAction()
    {
        myAction = null;
    }

    public void UpdateHealth(int amount)
    {
        currHp += amount;

        if (currHp > hp)
        {
            currHp = hp;
        }
        else if (currHp <= 0)
        {          
            Die();
        }
    }

    public void UpdateMana(int amount)
    {
        currMp += amount;
        if (currMp > mp)
        {
            currMp = mp;
        }
        else if(currMp < 0)
        {
            currMp = 0;
        }
    }
    
    public void ModifyStat(string statToMod, bool buff)
    {
        int sign;
        if(buff)
        {
            sign = 1;
        }
        else
        {
            sign = -1;
        }

        switch (statToMod)
        {
            case "atk":
                statModifiers[0] += 0.5 * sign;
                break;

            case "def":
                statModifiers[1] += 0.5 * sign;
                break;

            case "spAtk":
                statModifiers[2] += 0.5 * sign;
                break;

            case "spDef":
                statModifiers[3] += 0.5 * sign;
                break;

            case "spd":
                statModifiers[4] += 0.5 * sign;
                break;
        }
    }

    //To reset modifiers
    public void ResetStats()
    {
        statModifiers = new List<double>(new double[] { 1, 1, 1, 1, 1 });
    }

    public virtual void Die()
    {
        BattleTextManager.instance.UpdateText(this.myName + " Went Down!");
        menuObj.myText.color = new Color(0.5f, 0.5f, 0.5f, 0.8f);
        status = CONDITION.Dead;
        ResetStats();
        FightManager.instance.RemoveFighter(this);
    }

    [System.Serializable]
    public struct stat
    {
        string statName;
        int statValue;
    }
}
