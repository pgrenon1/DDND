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

    private void Update()
    {
        healthText.text = Mathf.CeilToInt(CurrentHealth).ToString();
    }
}
