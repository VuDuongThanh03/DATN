using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerStaminaBar : Singleton<PlayerStaminaBar>
{
    // Start is called before the first frame update
    [SerializeField]private Slider staminaSlider;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void SetupStaminaBar(float currentStamina, float maxStamina){
        staminaSlider.minValue = 0;
        staminaSlider.maxValue = maxStamina;
        staminaSlider.value = currentStamina;
    }
    public void OnStaminaChange(float currentStamina){
        staminaSlider.value = currentStamina;
    }
}
