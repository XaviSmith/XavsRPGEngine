using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class OverworldEnemy : MonoBehaviour {

    public List<EnemyFighter> enemies;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            StartBattle();
            //PlayerMovement.instance.canMove = false;
        }
        
    }
    // Use this for initialization
    void Start () {
		foreach(Transform child in transform)
        {
            enemies.Add(child.GetComponent<EnemyFighter>());
        }
	}
	
    public void StartBattle()
    {
        foreach(EnemyFighter enemy in enemies)
        {
            enemy.transform.parent = FightManager.instance.transform;
            FightManager.instance.enemies.Add(enemy);
        }
        gameObject.SetActive(false);
        SceneManager.LoadScene("Battle");
    }

}
