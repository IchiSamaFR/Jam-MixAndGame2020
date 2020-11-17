using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Spell
{
    public string spellName;
    public string spellUid;
    public float spellDamage;
    public float spellResistance;
    public float spellHeal;
    public int spellCooldown;
    public string spellElement;
    public bool himself;
    public bool shot;
    public float area;
    public GameObject groundPrefab;
    public GameObject usePrefab;

    [Header("Typing")]
    public List<SpellType> typingSys;
}

[System.Serializable]
public class SpellType
{
    public string word;
    public float spellMultiplier;
    public float spellTimeType;
}
