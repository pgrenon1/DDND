using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Damageable : MonoBehaviour
{
    public float maxHealth = 100f;
    public TextMeshProUGUI healthText;
    public TextMeshProUGUI maxHealthText;
    public Image healthBarFill;

    public delegate void OnDeath(Damageable damageable, Damage damage);
    public event OnDeath OnDamageableDeath;

    public float CurrentHealth { get; set; }

    private void Start()
    {
        CurrentHealth = maxHealth;
    }

    private void Update()
    {
        healthText.text = Mathf.CeilToInt(CurrentHealth).ToString();
        healthBarFill.fillAmount = CurrentHealth / maxHealth;
    }

    public bool TryDamage(Damage damage)
    {
        var healthBefore = CurrentHealth;

        // scale damage for resistances/vulnerabilities

        foreach (var typedDamage in damage.DamageData.typedDamages)
        {
            var damageAmount = typedDamage.scalableValue.GetValue(damage.ScalingFactor);
            CurrentHealth = Mathf.Max(0, CurrentHealth - damageAmount);
            Debug.Log(damage.Source + " deals " + damageAmount + " " + typedDamage.damageType + " damage!");
        }

        if (CurrentHealth <= 0)
        {
            Die(damage);
        }


        return healthBefore - CurrentHealth > 0f;
    }

    private void Die(Damage damage)
    {
        if (OnDamageableDeath != null)
            OnDamageableDeath(this, damage);
    }

    public bool TryHeal(float healAmount)
    {
        var amountHealed = Mathf.Min(healAmount, maxHealth - CurrentHealth);

        CurrentHealth += amountHealed;

        return amountHealed > 0f;
    }
}
