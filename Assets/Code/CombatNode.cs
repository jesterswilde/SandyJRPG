using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System; 

public class CombatNode  {

    //Combat node is responsobile for the actual attack. 
    //Most of this we've seen before.
    float _currentTime = 0;
    float _maxTime; 
    Action<CombatNodeResults> _end;
    ICharacter _attacker;
    ICharacter _defender;
    int _round;
    //All the info about what happened in this combat gets stored here so we can pass it around and not care 
    //about who we are passing it to
    CombatNodeResults _results = new CombatNodeResults(); 

    public CombatNode(int round, float maxTime, ICharacter attacker, ICharacter defender, Action<CombatNodeResults> nodeEnd)
    {
        _round = round; 
        _maxTime = maxTime;
        _end = nodeEnd;
        _attacker = attacker;
        _defender = defender;
        _results.Round = round;
        //Just like in combatPhase, UIManager has some shit it does when a node starts.
        UIManager.BeginCombatNode(_attacker.Weapon, _round, defender); 
    }

    //The Directions get passed in from combatPhase. One is from a player, one is from AI.
    //Combat node doesn't care about where they came from.
    public void Update(float _delta, Direction _attackerDir, Direction _defenderDir)
    {
        //The attacker didn't press anything
        if(_currentTime > _maxTime)
        {
            _end(_results); 
        }
        HandleDefender(_defenderDir);
        HandleAttacker(_attackerDir); 
        //update the slider (and maybe other things later.
        UIManager.UpdateCombatNode(_currentTime); 
        _currentTime += _delta; 
    }
    void HandleAttacker(Direction _dir)
    {
        //Both the player and AI shoot out Direction.None every turn nothing is pressed
        //First we check to make sure we get a direction that means somethign is happening.
        //If something is happening, we then check if it's valid. 
        if (_dir != Direction.None && _attacker.CanAttackInDir(_round, _dir))
        {
            //If it's valid, we first have hte attacker face the dir they are attacking (this affects their defense later)
            _attacker.ChangeFacing(_dir);
            //we start setting the results object
            _results.AttackDir = _dir;
            _results.Damage = _attacker.Swing(_round, _currentTime, _dir);
            _results.BlockType = _defender.GotHit(_results.Damage, _results.AttackDir);
            CombatManager.PlaySFX(_results.Damage, _results.BlockType);
            //This delegate was given to us by the combat phase and means we are done.
            _end(_results);
        }
    }
    void HandleDefender(Direction _dir)
    {
        if(_dir != Direction.None)
        {
            _defender.ChangeFacing(_dir); 
        }
    }
}

public class CombatNodeResults
{
    public Direction AttackDir = Direction.None;
    public int Damage = 0;
    public Defense BlockType = Defense.None;
    public int Round = 0; 

}
