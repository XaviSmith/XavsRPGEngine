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
        yield return new WaitForSeconds(0.1f);  //Wait a little bit so PlayerNameBox and currPlayer can set.
        foreach(PlayerFighter player in players)
        {
            if(player.status != Fighter.CONDITION.Dead)
            {
                currPlayer = player;
                PlayerNameBox.instance.myText.text = currPlayer.myName;
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
        PlayerNameBox.instance.myText.text = "";
        MenuManager.instance.currentMenu.gameObject.SetActive(false);
        actionsSet = true;
    }
}
