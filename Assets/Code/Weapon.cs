using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq; 

public class Weapon : MonoBehaviour {


    //Weapons contain most of the info about a character currently
    [SerializeField]
    List<Attacks> _up = new List<Attacks>();
    [SerializeField]
    List<Attacks> _right = new List<Attacks>();
    [SerializeField]
    List<Attacks> _down = new List<Attacks>();
    [SerializeField]
    List<Attacks> _left = new List<Attacks>();

    //All dirs will be filled later. But it's just a list of lists
    List<List<Attacks>> _allDirs;
    public List<List<Attacks>> AllAttacks { get { return _allDirs; } }

    [SerializeField]
    int _combosForSpecial;
    [SerializeField]
    int _specialDamage;

    //Setting values creates a default value.
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
        //Gets the longest attack combo. 
        return _allDirs.Max((_dir)=>{
            //The reason I do this instead of just checking the count is to make sure
            //that if you happen to have a bunch of blank attack slots at the end we don't 
            //just have a round wher eyou can't actually attack.
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

    //To understand this one you really need to read DamageFromHit and DefenseInDir
    public int GotHit(int _damDealt, Direction _facing, Direction _attackDir)
    {
        return DamageFromHit(_damDealt, DefenseInDir(_facing, _attackDir));
    }

    //PRetty straight forward, take raw damage, and mitigate it based on defense type
    public int DamageFromHit(int _damDealt, Defense _def)
    {
        switch (_def)
        {
            case Defense.Heavy: return Mathf.FloorToInt(_damDealt * _heavyMitigation);
            case Defense.Light: return Mathf.FloorToInt(_damDealt * _lightMitigation);
            default: return _damDealt; 
        }
    }
    //This is some trixy maths. I take advantage of the fact that there are only 4 directiosn we care about (0 - 3)
    //And that enum values are just numbers. Explaining this fully would get...messy
    public Defense DefenseInDir(Direction _facing, Direction _attackDir)
    {
        //using a type in parens says "hey convert to this type" so (int)_facing says "Take this direction enum and turn it into an int
        int _dir = ((int)_facing - (int)_attackDir) % 4;
        //This is a turnery value.  It's like a condensed if else statement. if the value is true, return the left. If it's false, return the right. 
        //C# % can return negative values, they aren't helpful.
        _dir = (_dir >= 0) ? _dir : 4 + _dir; 
        switch (_dir) {
            case 0: return _front;
            case 1: return _sideClockwise;
            case 2: return _back;
            case 3: return _sideCounterClock;
        }
        throw new System.Exception("Something weird happened with modulo");
    }

    //Simple helper function
    public bool CanAttackInDir(int _round, Direction _dir)
    {
        List<Attacks> _currentDir = _allDirs[(int)_dir];
        if (_currentDir.Count > _round && _currentDir[_round] != null)
        {
            return true; 
        }
        return false; 
    }

    //This returns the damage of a swing, in a direction, at a time
    public int Swing(int _round, float _time, Direction _dir)
    {
        if(CanAttackInDir(_round, _dir))
        {
            //we do the Direction to number trick here with (int)_dir 
            //an easier to read example would be how much damage did I do with _allAttacks[LeftAttack][Round 3].atSecond(2.5)
            return _allDirs[(int)_dir][_round].AttackedAt(_time); 
        }
        return 0;
    }

    void Awake()
    {
        _allDirs = new List<List<Attacks>> { _up, _right, _down, _left }; 
    }
}
