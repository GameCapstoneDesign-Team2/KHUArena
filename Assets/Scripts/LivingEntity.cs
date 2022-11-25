using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class LivingEntity : MonoBehaviour
{
    public float startingHealth = 100f;
    public float startingMana = 0f;
    public float health { get; protected set; }
    public float mana { get; protected set; }
    public bool dead { get; protected set; }

    public event Action onDeath;

    protected virtual void OnEnable() //Ȱ��ȭ�Ǹ� ���� ����
    {
        dead = false;
        health = startingHealth;
        mana = startingMana;
    }

    //������ ���� �� 
    public virtual void OnDamage(float damage)
    {
        health = health - damage;

        if (health <= 0 && !dead)
            Die();
    }

    //���
    public virtual void Die()
    {
        if(onDeath != null)
            onDeath();

        dead = true;
    }

}
