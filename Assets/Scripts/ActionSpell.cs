using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionSpell : MonoBehaviour
{
    public string spellId;
    public float levelMultiplier;
    Spell spell;
    public Entity target;
    public Vector3 toGo;
    public Entity launcher;
    bool canDmg = false;
    
    List<Entity> alreadyGetDamage = new List<Entity>();
    Animator anim;

    private void Start()
    {
        anim = GetComponent<Animator>();
        spell = SpellCollection.instance.GetSpell(spellId);

        if (target && spell.shot)
        {
            transform.position = target.transform.position;
        }
    }
    private void Update()
    {
        if (target && spell.shot)
        {
            transform.position = Vector3.Lerp(transform.position, toGo, Time.deltaTime);
        }
        else if (target)
        {
            transform.position = target.transform.position;
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (!canDmg)
        {
            return;
        }
        if (collision.GetComponent<Entity>())
        {
            if (alreadyGetDamage.IndexOf(collision.GetComponent<Entity>()) >= 0)
            {
                return;
            }

            if (spell.spellDamage > 0 
                && collision.GetComponent<Entity>() != launcher)
            {
                collision.GetComponent<Entity>().GetDmg((int)(spell.spellDamage * levelMultiplier));
            }
            alreadyGetDamage.Add(collision.GetComponent<Entity>());
        }
    }

    public void StartDmg()
    {
        canDmg = true;
    }
    public void StopDmg()
    {
        canDmg = false;
    }

    public void Action()
    {
        if (spell.spellHeal > 0 && target)
        {
            target.GetHeal((int)(spell.spellHeal * levelMultiplier));
        }

        if (spell.spellResistance > 0 && target) {
            target.GetArmor((int)(spell.spellResistance * levelMultiplier));
        }
    }

    public void Death()
    {
        Destroy(this.gameObject);
    }
}
