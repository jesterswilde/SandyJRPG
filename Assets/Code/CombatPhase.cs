using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class CombatPhase  {

    //basic property decleration. Most will get set immediately upon creation
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
        //Tell UI a combat phase has started, and who is attacking / defending.
        UIManager.BeginCombatPhase(_attacker, _defender);
        _isPlayer = isPlayer; 
        //Nodes are the next level down, the actual attacks. We immediately start a new node when creatded. 
        //We also give it a delegate that it can invoke to pass control back up to this, once it (the node) is done.
        _node = new CombatNode(_round, CombatManager.NodeDuration, _attacker, _defender, ConcludeNode);
        //Create the enemy AI. IsPlayer tells it who it is controlling
        _ai = new AIInput(_attacker, _defender, isPlayer); //This assumes always one player and one ai
    }

    void ConcludeNode(CombatNodeResults _results)
    {
        //Each round in combat creates it's own node, but not all nodes are the same. 
        _node = null;
        _round++;
        //THis long conditional statement is all the ways that a phase can end.
        if (_defender.IsAlive && _results.Round < _maxRounds && _results.Damage > 0 && _results.BlockType != Defense.Heavy)
        {
            //The combat phase continues so we make a new ai
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
