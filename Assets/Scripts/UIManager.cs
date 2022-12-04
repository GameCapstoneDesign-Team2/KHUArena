using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField]
    private Slider playerHP;
    [SerializeField]
    private Slider enemyHP;

    private int maxHP = 100;
    private int playerCurrentHP;
    private int enemyCurrentHP;

    void Start()
    {
        playerHP.value = (float)playerCurrentHP / (float)maxHP;
    }

    void Update()
    {
        playerCurrentHP = GameObject.Find("Player").GetComponent<PlayerController>().currentHP;
        enemyCurrentHP = (int)GameObject.Find("EnemyBody").GetComponent<EnemyController>().health;
        HandleHP();
    }

    void HandleHP()
    {
        playerHP.value = Mathf.Lerp(playerHP.value, (float)playerCurrentHP / (float)maxHP, Time.deltaTime * 10);
        enemyHP.value = Mathf.Lerp(enemyHP.value, (float)enemyCurrentHP / (float)maxHP, Time.deltaTime * 10);
    }
}
