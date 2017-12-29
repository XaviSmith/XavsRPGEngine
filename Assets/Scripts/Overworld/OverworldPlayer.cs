using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OverworldPlayer : MonoBehaviour {

    public List<PlayerFighter> players;
    public int id; //Unique ID that lets us distinguish characters, bosses, enemy types etc. 1-30 marks party memebers. 30+ marks enemies
    public List<string> myText;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            AddPartyMember();        
            //PlayerMovement.instance.canMove = false;
        }

    }
    // Use this for initialization
    void Start()
    {
        if(GameManager.instance.recruitedFighters.Contains(id))
        {
            Destroy(gameObject);
        }
        foreach (Transform child in transform)
        {
            players.Add(child.GetComponent<PlayerFighter>());
        }
    }

    public void AddPartyMember()
    {
        DialogueManager.instance.SetText(myText);
        foreach(PlayerFighter p in players)
        {
            p.transform.parent = PlayerManager.instance.transform;
            PlayerManager.instance.players.Add(p);
        }
        GameManager.instance.recruitedFighters.Add(id);
        gameObject.SetActive(false);
    }
}
