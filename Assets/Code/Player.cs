using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(DefenseUI))]
public class Player : MonoBehaviour, ICharacter {
    [SerializeField]
    Weapon _weapon;
    public Weapon Weapon { get { return _weapon; } }
    [SerializeField]
    int _health;
    Direction _facing;
    Renderer _rend;
    Color _baseColor; 

    //Weapon must not be null
    public int MaxAttackRounds { get { return _weapon.CalcMaxAttacks(); } }
    public bool IsAlive { get { return _health > 0; } }

    public void Select()
    {
        _rend.material.color = CombatManager.AttackerColor; 
    }
    public void Deselect()
    {
        _rend.material.color = _baseColor;
    }
    public Defense GotHit(int _damageDealt, Direction _attackDir)
    {
        Defense _defense = _weapon.DefenseInDir(_facing, _attackDir);
        _health -= _weapon.DamageFromHit(_damageDealt, _defense);
        return _defense; 
    }

    public int Swing(int _round, float _time, Direction _dir)
    {
        return _weapon.Swing(_round, _time, _dir); 
    }

    public bool CanAttackInDir(int _round, Direction _dir)
    {
        return _weapon.CanAttackInDir(_round, _dir);
    }
    public Defense DefenseInDirection(Direction _dir)
    {
        return _weapon.DefenseInDir(_facing, _dir);
    }
    public void ChangeFacing(Direction _dir)
    {
        _facing = _dir;
    }
    void Awake()
    {
        _rend = GetComponent<Renderer>();
        _baseColor = _rend.material.color; 
    }
    void Start()
    {
        CombatManager.RegisterPlayer(this); 
    }
}
