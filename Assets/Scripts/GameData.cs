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
    public float Armor;
    public float Stamina;
    public float PlayedTime;
    public int CurrentWeapon;
    
    public GameData(){
        UserAlias = "Default";
        GamerAge = 0;
        Coin = 0;
        Arrow = 0;
        Health = 0;
        Armor = 0;
        Stamina = 0;
        PlayedTime = 0;
        CurrentWeapon = 0;
    }
}
