using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Entity : MonoBehaviour
{
    public float speed = 3;
    public float maxHealth = 10;
    [SerializeField]
    public float health = 10;
    public Image healthFilled;
    public bool dead = false;

    public float armor = 0;
    public GameObject armorObj;
    public Image armorFilled;

    [System.NonSerialized]
    public bool forward = false;
    [System.NonSerialized]
    public bool backward = false;
    [System.NonSerialized]
    public bool left = false;
    [System.NonSerialized]
    public bool right = false;

    [System.NonSerialized]
    public Animator anim;

    public void _init_()
    {
        anim = GetComponent<Animator>();


        health = maxHealth;
        healthFilled.fillAmount = health / maxHealth;
        if (armorObj)
        {
            armorFilled.fillAmount = armor / maxHealth;
        }
    }

    public virtual void GetDmg(int _amount)
    {
        if(armor > 0) {
            armor -= _amount;
            if(armor < 0) {
                health -= armor;
            }
        } else {
            health -= _amount;
        }

        if(health <= 0)
        {
            dead = true;
        }

        if (armorObj)
        {
            armorFilled.fillAmount = armor / maxHealth;
        }
        healthFilled.fillAmount = health / maxHealth;
    }
    public void GetHeal(int _amount)
    {
        health += _amount;
        if(health > maxHealth)
        {
            health = maxHealth;
        }

        if (armorObj)
        {
            armorFilled.fillAmount = armor / maxHealth;
        }
        healthFilled.fillAmount = health / maxHealth;
    }

    public void GetArmor(int _armor) {
        armor += _armor;
        if (armor > maxHealth) {
            armor = maxHealth;
        }
        if (armorObj)
        {
            armorFilled.fillAmount = armor / maxHealth;
        }
    }


    public void CheckAnim()
    {
        if (!anim)
        {
            return;
        }

        if ((forward && backward || (!forward && !backward)))
        {
            anim.SetBool("forward", false);
            anim.SetBool("backward", false);
        }
        else if (forward)
        {
            anim.SetBool("forward", true);
            anim.SetBool("backward", false);
        }
        else if (backward)
        {
            anim.SetBool("forward", false);
            anim.SetBool("backward", true);
        }

        if ((left && right) || (!left && !right))
        {
            anim.SetBool("left", false);
            anim.SetBool("right", false);
        }
        else if (left)
        {
            anim.SetBool("left", true);
            anim.SetBool("right", false);
        }
        else if (right)
        {
            anim.SetBool("left", false);
            anim.SetBool("right", true);
        }
        if (!forward && !backward && !left && !right)
        {
            anim.SetBool("idle", true);
        }
        else
        {
            anim.SetBool("idle", false);
        }
        if (dead)
        {
            anim.SetBool("idle", false);
            anim.SetBool("death", true);
        }
    }

    public virtual void Inputs()
    {

    }

    public virtual void Movement()
    {
        float _speed = Speed();
        if (forward)
        {
            transform.position += new Vector3(0, 0.5f) * _speed;
        }
        if (backward)
        {
            transform.position -= new Vector3(0, 0.5f) * _speed;
        }
        if (left)
        {
            transform.position -= new Vector3(1, 0) * _speed;
        }
        if (right)
        {
            transform.position += new Vector3(1, 0) * _speed;
        }
    }

    public float Speed()
    {
        float _speed = speed;
        if (forward)
        {
            if (left)
            {
                _speed *= 0.8f;
            }
            else if (right)
            {
                _speed *= 0.8f;
            }
        }
        else if (backward)
        {

            if (left)
            {
                _speed *= 0.8f;
            }
            else if (right)
            {
                _speed *= 0.8f;
            }
        }

        return _speed * Time.fixedDeltaTime;
    }
}
