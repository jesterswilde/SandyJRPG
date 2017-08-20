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
    bool _isAttacking; 

    public AIInput(ICharacter _attacker, ICharacter _defender, bool _isDefending)
    {
        _isAttacking = !_isDefending; 
        //Figure out which one I should watch
        if (_isDefending)
        {
            _enemyAI = _defender as Enemy;
            _opponent = _attacker;
        }else
        {
            _enemyAI = _attacker as Enemy;
            _opponent = _defender; 
            //Choose my first attack
            ChooseAttack(0); 
        }
    }
    void ChooseAttack(int _round)
    {
        //Get all possible attacks for this round
        List<AtDir> _options = _enemyAI.PossibleAttacks(_round);
        //Pick one
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
        //Right now we don't do anything while defending
        if (!_isAttacking)
        {
            return Direction.None; 
        }
        if (!_hasAttacked)
        {
            _duration += _delta; 
            if(_duration > _attackTime)
            {
                //The AI, at the very beggining of hte turn already decided how to attack. 
                return _attackDir;
            }
        }
        return Direction.None; 
    }
    
}
