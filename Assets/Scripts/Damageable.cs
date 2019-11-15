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

    public bool TryDamage(DamageData damageData)
    {
        var hasDealtDamage = false;
        // scale damage for resistances/vulnarabilities

        foreach (var typedDamage in damageData.typedDamages)
        {
            CurrentHealth -= typedDamage.value;

            if (typedDamage.value > 0)
                hasDealtDamage = true;
        }

        return hasDealtDamage;
    }
}
