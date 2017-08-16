using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class CombatPhase  {
    ICharacter _attacker;
    ICharacter _defender;
    int _round = 0;
    int _maxGuards = 0; 
    int _maxRounds; 
    CombatNode _node;
    Action _concludePhase;
    bool _isPlayer; 

    public CombatPhase(ICharacter attacker, ICharacter defender, bool isPlayer, Action concludePhase)
    {
        _attacker = attacker;
        _defender = defender;
        _concludePhase = concludePhase;
        _maxRounds = _attacker.AttackRounds; 
        UIManager.BeginCombatPhase(_attacker, _defender);
        _isPlayer = isPlayer; 
        _node = new CombatNode(_round, CombatManager.NodeDuration, _attacker, _defender, ConcludeNode);
    }

    void ConcludeNode(CombatNodeResults _results)
    {
        _node = null;
        _round++;
        if(_defender.IsAlive && _results.Round < _maxRounds && _results.Damage > 0)
        {
            _node = new CombatNode(_round, CombatManager.NodeDuration, _attacker, _defender, ConcludeNode);
        }else
        {
            _concludePhase(); 
        }
    }

    public void Update(float _delta, Direction _dir)
    {
        if(_node != null)
        {
            _node.Update(_delta, _dir);
        }
    }

}
