using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monster : Entity
{
    [Header("Monster")]
    public string nameMonster;
    public Transform target;

    [Header("Stats")]
    public float rangeAttack;
    public float attackEverySecondes;
    public float timerAttack;
    public int damage;

    [Header("Other")]
    public WaveGen parent;

    // Start is called before the first frame update
    void Start()
    {
        _init_();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        FollowPlayer();
        AttackPlayer();
        CheckAnim();

        if (health <= 0)
        {
            parent.RemoveMonster(this.gameObject);
            anim.SetBool("death", true);
        }
    }

    void FollowPlayer() {
        if (!target.GetComponent<Entity>().dead && !target.GetComponent<PlayerMovement>().pause)
        {
            if (transform.position.x + rangeAttack < target.position.x)
            {
                right = true;
            }
            else
            {
                right = false;
            }
            if (transform.position.x - rangeAttack > target.position.x)
            {
                left = true;
            }
            else
            {
                left = false;
            }
            if (transform.position.y + rangeAttack < target.position.y)
            {
                forward = true;
            }
            else
            {
                forward = false;
            }
            if (transform.position.y - rangeAttack > target.position.y)
            {
                backward = true;
            }
            else
            {
                backward = false;
            }
        }
        else
        {
            right = false;
            left = false;
            backward = false;
            forward = false;
        }
        Movement();
    }

    public override void GetDmg(int _amount)
    {
        base.GetDmg(_amount);
        if(health <= 0)
        {
            parent.RemoveMonster(this.gameObject);
            anim.SetBool("death", true);
        }
    }

    public void DestroyMonster()
    {
        Destroy(gameObject);
    }

    void AttackPlayer() {
        if(!forward && !backward && !left && !right && !target.GetComponent<Entity>().dead)
        {
            if (Time.time > timerAttack + attackEverySecondes) {
                timerAttack = Time.time;
                target.GetComponent<Entity>().GetDmg(damage);
                anim.SetBool("attack", true);
            }
            else
            {
                anim.SetBool("attack", false);
            }
        }
    }
}
