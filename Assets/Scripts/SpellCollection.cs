using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpellCollection : MonoBehaviour
{
    public static SpellCollection instance;
    [SerializeField]
    public List<Spell> listSpell = new List<Spell>();

    public Spell GetFirstSpell() {
        return listSpell[0];
    }

    public Spell GetSpell(string id) {
        foreach (Spell spell in listSpell) {
            if(spell.spellUid == id) {
                return spell;
            }
        }
        return null;
    }

    public Spell GetRandomSpell()
    {
        int _rdm = Random.Range(0, listSpell.Count);
        return listSpell[_rdm];
    }

    public GameObject GetPrefab(string id) {
         foreach (Spell spell in listSpell) {
            if(spell.spellUid == id) {
                return spell.groundPrefab;
            }
        }
        return null;
    }

    void Awake() {
        instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }

}
