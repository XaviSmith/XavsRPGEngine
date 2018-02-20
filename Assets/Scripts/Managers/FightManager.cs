using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

/* This Script is the overall "brain" for battles. It derives from Singleton as we only want 1 instance and for that instance to be easily referenceable. 

    FightManager.cs can be thought of having 4 phases: Setup, Selection, Fighting, and Resolution.

    Setup phase involves checking whether or not we just entered a battle, if so what players/enemies are in the fight, and populating the sprites/Menu options(e.g. Who we can attack, who we can heal etc)
    Selection phase involves waiting for the player to select each of his/her party member's actions (handled by PlayerManager.cs), then determining turn orders for all of the fighters.
    Fighting phase involves checking who's turn it is, having them take their action (or determining an action for them if they're an enemy), calculating/checking the outcome of their action, and continuing that loop until everyone has taken their turn (then go back to Selection Phase), or all players/enemies have been defeated (Go to resolution phase).
    Resolution phase happens when the fight is done, and either awards the player's party Exp/Gold and returns them to the overworld, or takes the player to the Game Over screen accordingly.

    The general idea is BattleSetup-> 
            GetActions() -> SortTurns() ->  { FightLoop() -> TakeAction() -> Calculate() -> FightLoop() }    -> Repeat until resolution
              
*/
public class FightManager : Singleton<FightManager> {

    public List<Fighter> fighters;  //All fighters in this battle
    public PlayerManager playerManager; //To handle players
    
    public List<PlayerFighter> players;
    public List<EnemyFighter> enemies;

    public List<Fighter> turnOrder; //This will be the fighters list sorted by speed to determine turn orders during the fight phase. Once a fighter has taken their turn, they're removed from the list, and the next fighter takes their turn until the list is exhausted or all the enemies/players die.
    public Fighter currFighter; //Who's turn is it currently?

    public Scene myScene; //So we can know what scene it is and act accordingly (e.g. populate enemyFighters and playerFighters if it's a battle scene)

    //Delegates. Due to how unity works, whenever we have a while loop it needs to be in a Coroutine, however sometimes we only want something to execute after a coroutine finishes (e.g. Waiting on a player to advance text before we end the fight)
    public delegate void NextAction();
    public delegate void NextAction<T>(T var);

    public NextAction nextAction;   //currently unused after a code rehaul, but keeping it around for now just in case.

    //exp and gold we gained from enemies so far
    public int expPool = 0;
    public int goldPool = 0;

    //Let us take actions as soon as certain scenes load
    void OnEnable()
    {
        //Tell our 'OnLevelFinishedLoading' function to start listening for a scene change as soon as this script is enabled.
        SceneManager.sceneLoaded += OnLevelFinishedLoading;
    }

    void OnDisable()
    {
        //Tell our 'OnLevelFinishedLoading' function to stop listening for a scene change as soon as this script is disabled. Remember to always have an unsubscription for every delegate you subscribe to!
        SceneManager.sceneLoaded -= OnLevelFinishedLoading;
    }

    //Check to see if we're in a battle scene, if so go through battle setup
    void OnLevelFinishedLoading(Scene scene, LoadSceneMode mode)
    {      
        scene = SceneManager.GetActiveScene();
        expPool = 0;
        goldPool = 0;
        if (scene.name == "Battle")
        {
            StartCoroutine(GetActions());
            BattleSetup();
        }
    }


    //Set our players and enemies
    void BattleSetup()
    {
        //Coroutines because we need to wait 0.1 seconds for all the awake functions to set something
        StartCoroutine(SetPlayers());
        StartCoroutine(SetEnemies());
    }

    //Ienumerator because we need to wait a second for players to copy over and all Awake() functions to set some of their variables 
    IEnumerator SetPlayers()
    {
        yield return new WaitForSeconds(0.1f);

        //Populate all the text objects that let us select this player in a battle (e.g. heal->(this)) and populate our players list from playerManager.cs
        Menu playerMenu = PlayerOptions.instance.GetComponent<Menu>();
        PlayerOptions.instance.gameObject.SetActive(false);
        players = playerManager.players;
        for (int i = 0; i < playerMenu.myItems.Count; i++)
        {
            //populating menus and text
            if (i < players.Count)
            {
                fighters.Add(players[i]);
                players[i].menuObj = playerMenu.myItems[i];
                players[i].menuObj.gameObject.SetActive(true);
                yield return new WaitForSeconds(0.05f); //takes a second for the playerMenu.myItems' gameObject and myName to populate


                players[i].menuObj.myObj = players[i].gameObject;
                players[i].menuObj.myText = players[i].menuObj.GetComponent<Text>(); //We have to set the menuItem's nameBox because unity does not let it call any functions like Awake() etc when it's inactive.
                players[i].menuObj.myText.text = players[i].myName;
            }
            else //Remove extraneous options from menus (it's a lot easier to remove extra options than dynamically populate new ones)
            {
                playerMenu.RemoveOptions(i);
                break;
            }
        }
    }

    //Basically the same as SetPlayers, but also sets enemy sprites
    IEnumerator SetEnemies()
    {
        yield return new WaitForSeconds(0.1f);
        Menu enemyMenu = EnemyOptions.instance.GetComponent<Menu>();
        enemyMenu.optionCount = enemies.Count;
        EnemyOptions.instance.gameObject.SetActive(false);

        for (int i = 0; i <= enemyMenu.myItems.Count; i++)
        {
            if(i < enemies.Count)
            {
                fighters.Add(enemies[i]);
                enemies[i].menuObj = enemyMenu.myItems[i];
                enemies[i].menuObj.gameObject.SetActive(true);
                BattleSpriteManager.instance.myImg[i].sprite = enemies[i].battleSprite.sprite;
                enemies[i].battleSprite = BattleSpriteManager.instance.myImg[i];

                BattleSpriteManager.instance.spritePos[i].SetActive(true);
                yield return new WaitForSeconds(0.05f); //takes a second for the playerMenu.myItems' gameObject and myName to populate


                enemies[i].menuObj.myObj = enemies[i].gameObject;
                enemies[i].menuObj.myText = enemies[i].menuObj.GetComponent<Text>(); //We have to set the menuItem's nameBox because unity does not let it call any functions like Awake() etc when it's inactive.
                enemies[i].menuObj.myText.text = enemies[i].myName;
            }
            else
            {
                enemyMenu.RemoveOptions(i);
                break;
            }
            
        }
        yield return new WaitForSeconds(0.1f);
    }

    //wait for all players to set their actions then sort turn order.
    public IEnumerator GetActions()
    {
        //FlavorTextObj.instance.gameObject.SetActive(false); TODO: reinstate flavour text.
        BattleTextManager.instance.gameObject.SetActive(false);
        playerManager.GetActions();
        while(!playerManager.actionsSet)
        {
            yield return null;
        }
        playerManager.actionsSet = false;
        //FlavorTextObj.instance.gameObject.SetActive(true);
        BattleTextManager.instance.gameObject.SetActive(true);
        SortTurns();

    }
    
    

    //sort turn order by speed then start the fight loop;
    public void SortTurns()
    {
        turnOrder = new List<Fighter>(fighters);
        turnOrder.Sort((a, b) => -1 * a.spd.CompareTo(b.spd)); // descending sort
        FightLoop();
    }


    //See what state the fight is in (players/enemies dead, someone's turn, or everyone took their turn) and act accordingly 
    public void FightLoop()
    {
        
        if (turnOrder.Count >= 1) //If we still have fighters that need to take their turn, do so
        {
            currFighter = turnOrder[0];
            TakeAction();
        }
        else  //Otherwise reset to GetActions().
        {
            PlayerManager.instance.actionsSet = false;
            
            //MenuManager.instance.currentMenu.gameObject.SetActive(true);
            StopAllCoroutines();
            StartCoroutine(GetActions());
        }
    }

    //Execute a player's selected action, or decide an action for an enemy.
    public void TakeAction()
    {
        if (currFighter.fighterType == "Enemy") //If enemy, decide their action
        {
            EnemyFighter tempFighter = currFighter as EnemyFighter;
            tempFighter.SetTurn();
        }

        currFighter.myAction.Execute(currFighter.myTarget);
        turnOrder.Remove(currFighter);  //They took their action, remove them from the turnOrder list.
        StartCoroutine(WaitOnInput(Calculate)); //check the fight is over or not after the player advances text.

    }

    public void Calculate()
    {
        //If all enemies or players are dead, handle accordingly 
        if (enemies.Count <= 0)
        {
            StartCoroutine(WinFight());
        }
        else if (players.Count <= 0)
        {
            BattleTextManager.instance.SetText("Everyone was knocked out!");
            StartCoroutine(WaitOnInput(LoseFight));
        }
        else
        {
            FightLoop();
        }

    }

    //If we won, inform the player, award them exp/gold (levelling up is handled by the PlayerFighter class), reset everything and take us back to the overworld.
    IEnumerator WinFight()
    {
        BattleTextManager.instance.SetText("YOU WIN!");
        AudioManager.instance.PlaySound(AudioManager.instance.winFight);
        while (!BattleTextManager.instance.textAdvanced)
        {
            yield return null;
        }

        foreach (PlayerFighter player in players)
        {
            player.ResetStats();    //For if we used any status modifiers (e.g. Attack up, Def up etc.), [Needs testing] 
            player.GainExp(expPool);    //also handles levelling up and learning moves.
            while (!BattleTextManager.instance.textAdvanced)
            {
                yield return null;
            }
        }
        PlayerManager.instance.actionsSet = false;
        PlayerManager.instance.gold += goldPool;
        ResetState();
        SceneManager.LoadScene("Overworld");
    }

    //If we lose, reset everything and take us to the game over screen.
    public void LoseFight()
    {
        foreach (PlayerFighter player in players)
        {
            player.ResetStats();
        }
        ResetState();
        SceneManager.LoadScene("GameOver");
    }
	
    //Once we finish a fight, reset our fightManager to normal so that nothing arries over.
    public void ResetState()
    {
        fighters.Clear();
        enemies.Clear();
        turnOrder.Clear();
        currFighter = null;
        nextAction = null;
        expPool = 0;
        goldPool = 0;
        foreach (Transform child in transform)
        {
            Destroy(child.gameObject);
        }
    }

    
    //When we want to wait on the player to advance text before doing something.
    IEnumerator WaitOnInput(NextAction someAction)
    {
        while (!BattleTextManager.instance.textAdvanced)
        {
            yield return null;

        }
        someAction.Invoke();
        
    }

    //Wrapper so that other classes can call the WaitOnInputWithParameters()
    public void WaitOnInputWithParametersWrapper<T>(NextAction<T> someAction, T someVar)
    {
        StartCoroutine(WaitOnInputWithParameters(someAction, someVar));
    }

    //Same as above, but if we want the method to have a parameter with it.
    public IEnumerator WaitOnInputWithParameters<T>(NextAction<T> someAction, T someVar)
    {
        bool buttonPress = false;
        while (!buttonPress)
        {
            if (Input.GetKeyDown(KeyCode.Z))
            {
                buttonPress = true;

                someAction.Invoke(someVar);

                yield return new WaitForSeconds(0.2f);
            }
        }
    }

    //When a player/enemy dies, remove them from the fight.
    public void RemoveFighter(Fighter f)
    {
        if(f.fighterType == "Enemy")
        {
            EnemyFighter e = f as EnemyFighter;
            RemoveEnemy(e);
        }

        else if(f.fighterType == "Player")
        {
            PlayerFighter p = f as PlayerFighter;
            RemovePlayer(p);
        }
    }

    void RemoveEnemy(EnemyFighter enemy)
    {
        enemies.Remove(enemy);
        fighters.Remove(enemy);
        turnOrder.Remove(enemy);

        expPool += enemy.exp;
        goldPool += enemy.gold;

        if (enemies.Count <= 0)
        { 
            StopAllCoroutines();
        }
    }

    void RemovePlayer(PlayerFighter player)
    {
        fighters.Remove(player);
        players.Remove(player);
        turnOrder.Remove(player);
    }

}
