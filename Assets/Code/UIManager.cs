using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour {

    public static UIManager t;
    TimingArea _timingArea;
    [SerializeField]
    float _width;
    [SerializeField]
    Color _heavyDefense;
    public static Color HeavyDefense { get { return t._heavyDefense; } }
    [SerializeField]
    Color _lightDefense;
    public static Color LightDefense { get { return t._lightDefense; } }
    [SerializeField]
    Color _noDefense; 
    public static Color NoDefense { get { return t._noDefense; } }
    public static float Width { get { return t._width; } }
    public static float ModWidth { get { return 1 / CombatManager.NodeDuration * Width; } }

    [SerializeField]
    DefenseUI _defenseUI; 

    public static void RegisterTimingArea(TimingArea _area)
    {
        t._timingArea = _area; 
    }

    public static void BeginCombatPhase(ICharacter _attacker, ICharacter _defender)
    {
        
    }
    public static void BeginCombatNode(Weapon _weapon, int _round, ICharacter _defender)
    {
        if(t._timingArea != null){
            t._timingArea.SetTimings(
                TimingFromWep(_weapon, _round, 0),
                TimingFromWep(_weapon, _round, 1),
                TimingFromWep(_weapon, _round, 2),
                TimingFromWep(_weapon, _round, 3)
                );
        }
        t._defenseUI.SetDefender(_defender); 
    }
    public static void UpdateCombatNode(float _currentTime)
    {
        t._timingArea.UpdateSlider(_currentTime);
        t._defenseUI.UpdateFacing(); 
    }
    static float[] TimingFromWep(Weapon _weapon, int _round, int _index)
    {
        List<Attacks> _dir = _weapon.AllAttacks[_index]; 
        if(_dir.Count > _round && _dir[_round] != null)
        {
            return new float[] { _dir[_round].Range, _dir[_round].HitTime };
        }else
        {
            return new float[] { -1f, -1f }; 
        }
    }

    void Awake()
    {
        t = this;
        _defenseUI = Instantiate(_defenseUI); 
    }
}
