using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attacks : MonoBehaviour
{
    [SerializeField]
    int _damage; 
    public int Damage { get { return _damage; } }
    [SerializeField]
    float _hitTime = 0.5f;
    public float HitTime { get { return _hitTime; } }
    [SerializeField]
    float _range = 0.25f; 
    public float Range { get { return _range; } }
    
    public int AttackedAt(float _time)
    {
        if(_time > _hitTime - _range && _time < _hitTime + _range)
        {
            return _damage; 
        }
        return 0; 
    }

}