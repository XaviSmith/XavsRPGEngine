using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlayerFighter : Fighter {

    //Affects stat growth on level up (e.g. we'd want a tank to level up defence at a higher rate than mp)
    [Tooltip("Modifiers when levelling up hp, mp, atk, def, spAtk, spDef, and speed respectively for this character.")]
    public List<float> levelUpModifiers = new List<float>(new float[] { 1, 1, 1, 1, 1, 1, 1 });

    public List<PlayerFighter> partyMembers;
    public int expToNextLev = 100;

	public void GainExp(int expGain)
    {
        exp += expGain;

        if(exp > expToNextLev )
        {
            LevelUp();
        }
    }

    public void LevelUp()
    {
        StartCoroutine(LevelUpCoroutine());
    }

    IEnumerator LevelUpCoroutine()
    {
        lv += 1;
        expToNextLev += 100 * lv;
        AudioManager.instance.PlaySound(AudioManager.instance.levelUp);
        BattleTextManager.instance.SetText(myName + " hit level " + lv + "!");
        LevelUpStats();
        foreach(Attack skill in skills)
        {
            if(skill.levelLearned == lv)
            {
                skill.learned = true;
                BattleTextManager.instance.UpdateText(myName + " Learned " + skill.attackName + "!");
            }
        }

        //regain some hp and mp on level up
        if(currHp < hp)
        {
            UpdateHealth(hp / 10);
        }

        if (currMp < mp)
        {
            currMp += mp / 10;
            if (currMp > mp)
            {
                currMp = mp;
            }
        }
        while (!BattleTextManager.instance.textAdvanced)
        {
            yield return null;
        }

        //In case we gain enough to advance another level
        if(exp > expToNextLev)
        {
            LevelUp();
        }
    }

    //Simple alegbraic formulae using excel to make things more manageable
    void LevelUpStats()
    {
        int hpGain;
        int mpGain;
        hp += hpGain = (int)(29.747 * levelUpModifiers[0]);
        mp += mpGain = (int)(9.6612 * levelUpModifiers[1]);
        atk += (int)(2.2925 * levelUpModifiers[2]);
        def += (int)(2.2925 * levelUpModifiers[3]);
        spAtk += (int)(2.2925 * levelUpModifiers[4]);
        spDef += (int)(2.2925 * levelUpModifiers[5]);
        spd += (int)(2.2925 * levelUpModifiers[6]);

        UpdateHealth(hpGain);
        UpdateMana(mpGain);
    }

}
