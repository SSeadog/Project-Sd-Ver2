using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public abstract class Stat : MonoBehaviour
{
    [SerializeField] protected int _maxHp;
    [SerializeField] protected int _hp;
    [SerializeField] protected int _power;
    [SerializeField] protected float _speed;
    [SerializeField] protected float _size;

    public int MaxHp { get { return _maxHp; } set { _maxHp = value; } }
    public int Hp { get { return _hp; } set { _hp = value; } }
    public int Power { get { return _power; } set { _power = value; } }
    public float Speed { get { return _speed; } set { _speed = value; } }
    public float Size { get { return _size;} set { _size = value; } }

    public UnityAction<float> OnAttacktedAction;
    public UnityAction OnDeadAction;

    void Start()
    {
        
    }

    public void GetAttacked(Stat attacker)
    {
        int damage = attacker.Power;
        float stiffTime = damage > 20 ? 0.2f : 0f;
        
        Hp -= damage;

        if (Hp > 0)
        {
            if (OnAttacktedAction!= null)
                OnAttacktedAction.Invoke(stiffTime);
        }
        else
        {
            Hp = 0;
            OnDead();
        }
    }

    public virtual void OnDead()
    {
        Debug.Log("OnDead");
        // 죽었을 때 할 행동
        if (OnDeadAction != null)
            OnDeadAction.Invoke();
    }
}
