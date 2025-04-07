using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealthBar : Singleton<PlayerHealthBar>
{
    // Start is called before the first frame update
    [SerializeField]private Slider healthSlider;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void SetupHealthBar(float currentHealth, float maxHealth){
        healthSlider.minValue = 0;
        healthSlider.maxValue = maxHealth;
        healthSlider.value = currentHealth;
    }
    public void OnHealthChange(float currentHealth){
        healthSlider.value = currentHealth;
    }
}
