using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIInput {

    Enemy _enemyAI;
    ICharacter _opponent;
    int _round;
    float _duration;
    float _attackTime;
    Direction _attackDir; 
    bool _hasAttacked = false; 

    public AIInput(ICharacter _attacker, ICharacter _defender, bool _isDefending)
    {
        if (_isDefending)
        {
            _enemyAI = _defender as Enemy;
            _opponent = _attacker;
        }else
        {
            _enemyAI = _attacker as Enemy;
            _opponent = _defender; 
        }
        ChooseAttack(0); 
    }
    void ChooseAttack(int _round)
    {
        List<AtDir> _options = _enemyAI.PossibleAttacks(_round);
        AtDir _choice = _options[Random.Range(0, _options.Count)];
        _attackTime = _choice.Attack.HitTime;
        _attackDir = _choice.Direction;
        Debug.Log(_attackDir + " | " + _attackDir); 
    }
    public void StartRound(int round)
    {
        _round = round;
        _hasAttacked = false;
        _duration = 0; 
        ChooseAttack(round); 
    }
    public Direction GetDirection(float _delta)
    {
        if (!_hasAttacked)
        {
            _duration += _delta; 
            if(_duration > _attackTime)
            {
                return _attackDir;
            }
        }
        return Direction.None; 
    }
    
}
