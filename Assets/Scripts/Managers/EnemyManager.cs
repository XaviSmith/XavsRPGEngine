using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour {

    public List<EnemyFighter> enemies;

    void Awake()
    {
        FightManager.instance.enemies = enemies;
    }

}
