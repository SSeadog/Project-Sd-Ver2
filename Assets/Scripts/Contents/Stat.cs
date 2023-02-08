using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Stat : MonoBehaviour
{
    protected int _maxHp;
    protected int _hp;
    protected int _power;
    protected float _speed;

    public int MaxHp { get { return _maxHp; } set { _maxHp = value; } }
    public int Hp { get { return _hp; } set { _maxHp = value; } }
    public int Power { get { return _power; } set { _maxHp = value; } }
    public float Speed { get { return _speed; } set { _speed = value; } }

    void Start()
    {
        
    }

    public void OnAttacked(Stat attacker)
    {
        int damage = attacker.Power;
        Hp -= damage;

        if (Hp < 0)
        {
            Hp = 0;
            OnDead();
        }
    }

    public virtual void OnDead()
    {
        // 죽었을 때 할 행동
    }
}
