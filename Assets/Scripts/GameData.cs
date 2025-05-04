using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;

public class GameData
{
    public string UserAlias;
    public int GamerAge;
    public int Coin;
    public int Arrow;
    public float Health;
    public int ItemHealth;
    public float Armor;
    public float Stamina;
    public int ItemStamina;
    public float AngryEnergy;
    public float PlayedTime;
    public bool IsHaveBow;
    public int EquipLevel;
    public bool HaveSave;
    public int CurrentLevel;
    public int CurrentGameMode;
    public GameData(){
        UserAlias = "Default";
        GamerAge = 0;
        Coin = 0;
        Arrow = 0;
        Health = 0;
        ItemHealth = 0;
        Armor = 0;
        Stamina = 0;
        ItemStamina = 0;
        AngryEnergy = 0;
        PlayedTime = 0;
        IsHaveBow = false;
        EquipLevel = 0;
        HaveSave = false;
        CurrentLevel = 1;
        CurrentGameMode = 0;
    }
}
