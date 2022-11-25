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

    protected virtual void OnEnable() //활성화되면 상태 리셋
    {
        dead = false;
        health = startingHealth;
        mana = startingMana;
    }

    //데미지 입을 때 
    public virtual void OnDamage(float damage)
    {
        health = health - damage;

        if (health <= 0 && !dead)
            Die();
    }

    //사망
    public virtual void Die()
    {
        if(onDeath != null)
            onDeath();

        dead = true;
    }

}
