using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System; 

public class CombatNode  {
    float _currentTime = 0;
    float _maxTime; 
    Action<CombatNodeResults> _end;
    ICharacter _attacker;
    ICharacter _defender;
    int _round;
    CombatNodeResults _results = new CombatNodeResults(); 

    public CombatNode(int round, float maxTime, ICharacter attacker, ICharacter defender, Action<CombatNodeResults> nodeEnd)
    {
        _round = round; 
        _maxTime = maxTime;
        _end = nodeEnd;
        _attacker = attacker;
        _defender = defender;
        _results.Round = round;
        UIManager.BeginCombatNode(_attacker.Weapon, _round, defender); 
    }
    public void Update(float _delta, Direction _attackerDir, Direction _defenderDir)
    {
        if(_currentTime > _maxTime)
        {
            _end(_results); 
        }
        HandleDefender(_defenderDir);
        HandleAttacker(_attackerDir); 
        UIManager.UpdateCombatNode(_currentTime); 
        _currentTime += _delta; 
    }
    void HandleAttacker(Direction _dir)
    {
        if (_dir != Direction.None && _attacker.CanAttackInDir(_round, _dir))
        {
            _attacker.ChangeFacing(_dir);
            _results.AttackDir = _dir;
            _results.Damage = _attacker.Swing(_round, _currentTime, _dir);
            _results.BlockType = _defender.GotHit(_results.Damage, _results.AttackDir);
            CombatManager.PlaySFX(_results.Damage, _results.BlockType);
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
