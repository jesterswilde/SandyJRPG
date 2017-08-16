using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq; 

public class Weapon : MonoBehaviour {

    [SerializeField]
    List<Attacks> _up = new List<Attacks>();
    [SerializeField]
    List<Attacks> _right = new List<Attacks>();
    [SerializeField]
    List<Attacks> _down = new List<Attacks>();
    [SerializeField]
    List<Attacks> _left = new List<Attacks>();
    List<List<Attacks>> _allDirs;
    public List<List<Attacks>> AllAttacks { get { return _allDirs; } }

    [SerializeField]
    int _combosForSpecial;
    [SerializeField]
    int _specialDamage;

    [SerializeField]
    Defense _front = Defense.Light;
    [SerializeField]
    Defense _sideClockwise = Defense.None;
    [SerializeField]
    Defense _sideCounterClock = Defense.None;
    [SerializeField]
    Defense _back = Defense.None;

    [SerializeField]
    float _heavyMitigation = 0.75f;
    [SerializeField]
    float _lightMitigation = 0.25f; 


    public int CalcMaxAttacks()
    {
        return _allDirs.Max((_dir)=>{
            int _highestIndex = 0; 
            for(int i = 0; i < _dir.Count; i++)
            {
                if(_dir[i] != null)
                {
                    _highestIndex = i; 
                }
            }
            return _highestIndex; 
        });
    }

    public int GotHit(int _damDealt, Direction _facing, Direction _attackDir)
    {
        return DamageFromHit(_damDealt, DefenseInDir(_facing, _attackDir));
    }

    public int DamageFromHit(int _damDealt, Defense _def)
    {
        switch (_def)
        {
            case Defense.Heavy: return Mathf.FloorToInt(_damDealt * _heavyMitigation);
            case Defense.Light: return Mathf.FloorToInt(_damDealt * _lightMitigation);
            default: return _damDealt; 
        }
    }
    public Defense DefenseInDir(Direction _facing, Direction _attackDir)
    {
        int _dir = ((int)_facing - (int)_attackDir) % 4;
        _dir = (_dir >= 0) ? _dir : 4 + _dir; 
        switch (_dir) {
            case 0: return _front;
            case 1: return _sideClockwise;
            case 2: return _back;
            case 3: return _sideCounterClock;
        }
        throw new System.Exception("Something weird happened with modulo");
    }
    public bool CanAttackInDir(int _round, Direction _dir)
    {
        List<Attacks> _currentDir = _allDirs[(int)_dir];
        if (_currentDir.Count > _round && _currentDir[_round] != null)
        {
            return true; 
        }
        return false; 
    }
    public int Swing(int _round, float _time, Direction _dir)
    {
        if(CanAttackInDir(_round, _dir))
        {
            return _allDirs[(int)_dir][_round].AttackedAt(_time); 
        }
        return 0;
    }

    void Awake()
    {
        _allDirs = new List<List<Attacks>> { _up, _right, _down, _left }; 
    }
}
