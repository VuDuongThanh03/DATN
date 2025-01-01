using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime.Tree;
using UnityEngine;
[CreateAssetMenu(fileName = "CharacterStats", menuName = "Stat/CharacterStats")]
public class CharacterStats : ScriptableObject
{
    // Start is called before the first frame update
    public int health;
    public int armor;
    public int stamina;
}
