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
            CurrentHealth -= typedDamage.scalableValue.GetValue(damage.ScalingFactor);
        }

        return healthBefore - CurrentHealth > 0f;
    }

    public bool TryHeal(float healAmount)
    {
        var amountHealed = Mathf.Min(healAmount, maxHealth - CurrentHealth);

        CurrentHealth += amountHealed;

        return amountHealed > 0f;
    }
}
