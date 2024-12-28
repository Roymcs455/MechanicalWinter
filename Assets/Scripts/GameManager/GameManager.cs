using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{
    public EnemyController[] enemyController;
    public Armor playerArmor;
    public int activeEnemies;
    public GameObject UIWin, UILose;
    public TextMeshProUGUI enemyCounter;



    private void Start()
    {

        activeEnemies = enemyController.Length;
        foreach (EnemyController enemy in enemyController)
        {
            enemy.OnDeath += HandleEnemyDeath;
        }
        playerArmor.OnDeathPlayer += PlayerArmor_OnDeathPlayer;
        enemyCounter.text = $"Objective: Defeat {activeEnemies} enemies";
    }

    private void PlayerArmor_OnDeathPlayer()
    {
        UILose.SetActive(true);
        Time.timeScale = 0.0f;
    }

    private void HandleEnemyDeath()
    {
        activeEnemies--;
        enemyCounter.text = $"Objective: Defeat {activeEnemies} enemies";
        if (activeEnemies == 0)
        {
            UIWin.SetActive(true);
            Time.timeScale = 0.0f;
        }
    }
}
