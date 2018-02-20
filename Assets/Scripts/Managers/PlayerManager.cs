using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//handles all player stuff in a fight like party info and levelling up etc.
public class PlayerManager : Singleton<PlayerManager> {

    public List<PlayerFighter> players;
    public PlayerFighter currPlayer;
    public bool actionsSet;
    public int gold;
    
    public override void Awake()
    {
        base.Awake();
        foreach (Transform child in transform)
        {
            players.Add(child.GetComponent<PlayerFighter>());
        }
        StartCoroutine(SetPlayers());
    }

    IEnumerator SetPlayers()
    {
        yield return new WaitForSeconds(0.1f);
        FightManager.instance.players = players;
    }

    public void GetActions()
    {
        MenuManager.instance.defaultMenu.gameObject.SetActive(true);
        StartCoroutine(WaitOnActions());
    }

    IEnumerator WaitOnActions()
    {
        PlayerBattleUI.instance.gameObject.SetActive(true);
        yield return new WaitForSeconds(0.1f);  //Wait a little bit so PlayerBattleUI and currPlayer can set.
        foreach(PlayerFighter player in players)
        {
            if(player.status != Fighter.CONDITION.Dead)
            {
                currPlayer = player;
                FightManager.instance.currFighter = player;
                PlayerBattleUI.instance.nameBox.text = currPlayer.myName;
                PlayerBattleUI.instance.hpBox.text = currPlayer.currHp.ToString() + "/" + currPlayer.hp.ToString();
                PlayerBattleUI.instance.mpBox.text = currPlayer.currMp.ToString() + "/" + currPlayer.mp.ToString();
                MenuManager.instance.SetSkills();
                while(!player.actionSet)
                {
                    yield return null;
                }
                MenuManager.instance.ResetMenus();
            }
        }
        foreach (PlayerFighter player in players)
        {
            player.actionSet = false;
        }
        PlayerBattleUI.instance.gameObject.SetActive(false);
        MenuManager.instance.currentMenu.gameObject.SetActive(false);
        actionsSet = true;
    }
}
