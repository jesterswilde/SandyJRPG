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
    AIInput _ai; 

    public CombatPhase(ICharacter attacker, ICharacter defender, bool isPlayer, Action concludePhase)
    {
        _attacker = attacker;
        _defender = defender;
        _concludePhase = concludePhase;
        _maxRounds = _attacker.MaxAttackRounds; 
        UIManager.BeginCombatPhase(_attacker, _defender);
        _isPlayer = isPlayer; 
        _node = new CombatNode(_round, CombatManager.NodeDuration, _attacker, _defender, ConcludeNode);
        _ai = new AIInput(_attacker, _defender, isPlayer); //This assumes always one player and one ai
    }

    void ConcludeNode(CombatNodeResults _results)
    {
        _node = null;
        _round++;
        if (_defender.IsAlive && _results.Round < _maxRounds && _results.Damage > 0 && _results.BlockType != Defense.Heavy)
        {
            _node = new CombatNode(_round, CombatManager.NodeDuration, _attacker, _defender, ConcludeNode);
            _ai.StartRound(_round); 
        }else
        {
            _concludePhase(); 
        }
    }

    public void Update(float _delta)
    {
        if(_node != null)
        {
            if (_isPlayer)
            {
                _node.Update(_delta, PlayerInput.GetDirection(), _ai.GetDirection(_delta));
            }else
            {
                _node.Update(_delta, _ai.GetDirection(_delta), PlayerInput.GetDirection()); 
            }
        }
    }

}
