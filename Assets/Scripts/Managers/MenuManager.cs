using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class MenuManager : Singleton<MenuManager> {

    public Menu defaultMenu;
    public List<GameObject> playerTextObj;
    public List<GameObject> enemyTextObj;
    public Menu enemyMenu;
    public Menu playerMenu;
    public Menu currentMenu;
    public Menu previousMenu;
    public Menu skillMenu;


	public void ResetMenus()
    {
        currentMenu.gameObject.SetActive(false);
        defaultMenu.gameObject.SetActive(true);
        UnsetSkills();
        currentMenu = defaultMenu;
    }

    public void SetSkills()
    {
        //The skill menu is dynamic and depends on a player's learned skills. Therefore we have to clear out and repopulate the list at the start of each character's turn.
        skillMenu.myItems.Clear();

        //For each skill in the current player's skill list, check and see if the skill has been learned yet...
        for (int i = 0; i < PlayerManager.instance.currPlayer.skills.Count; i++)
        {
            if (PlayerManager.instance.currPlayer.skills[i].learned)
            {
                //...If so, add to to the list of selectable options in the skill menu, set the menu option active, and link it to that skill.
                skillMenu.myItems.Add(skillMenu.transform.GetChild(i).GetComponent<MenuOption>());
                skillMenu.myItems[i].gameObject.SetActive(true);
                //skillMenu.myItems[i].myAction.RemoveAllListeners();
                skillMenu.myItems[i].myAction.AddListener(PlayerManager.instance.currPlayer.skills[i].SetAction);
                skillMenu.myItems[i].myText.text = PlayerManager.instance.currPlayer.skills[i].attackName;
            }

        }
        
        //NOTE: ALWAYS HAVE THE BACK BUTTON BE AT THE BOTTOM OF THE CHILDREN SO WE ALWAYS KNOW WHERE IT IS
        //We always want to have a back button for our skills list. So after adding all the other skills, add the back button too.
        skillMenu.myItems.Add(skillMenu.transform.GetChild(skillMenu.transform.childCount - 1).GetComponent<MenuOption>());
    }

    public void UnsetSkills()
    {
        //last item is the back button so leave it alone
        for (int i = 0; i < skillMenu.myItems.Count - 1; i++ )
        {
            skillMenu.myItems[i].myAction.RemoveAllListeners();
            skillMenu.myItems[i].gameObject.SetActive(false);
        }
    }

    //When we want to switch to the menu to select who we attack/heal etc
    public void SelectFighter(bool affectsPlayers)
    {
        if(affectsPlayers)
        {
            currentMenu.ChangeMenu(playerMenu);           
        }
        else
        {
            currentMenu.ChangeMenu(enemyMenu);
        }
    }
}
