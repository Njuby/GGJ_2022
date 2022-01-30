using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Healthbar : MonoBehaviour
{
    public Player player;
    public Image healthBar;

    void Update()
    {
        healthBar.fillAmount = player.Health / (float)player.MaxHealth;
    }
}
