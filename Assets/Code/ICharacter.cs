using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ICharacter {

    void Select();
    void Deselect(); 
    Defense GotHit(int Damage, Direction _dir);
    int MaxAttackRounds { get; }
    bool IsAlive { get; }
    int Swing(int _round, float _time, Direction _dir);
    bool CanAttackInDir(int _round, Direction _dir);
    Weapon Weapon { get; }
    void ChangeFacing(Direction _direction);
    Transform transform { get; }
    GameObject gameObject { get; }
    Defense DefenseInDirection(Direction _dir);  
}

public enum Defense
{
    Heavy,
    Light,
    None    
}
public enum Direction
{
    Up,
    Right,
    Down,
    Left, 
    None
}
