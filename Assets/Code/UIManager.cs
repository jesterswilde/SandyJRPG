using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour {

    public static UIManager t;
    TimingArea _timingArea;
    [SerializeField]
    float _width; 
    public static float Width { get { return t._width; } }
    public static float ModWidth { get { return 1 / CombatManager.NodeDuration * Width; } }

    public static void RegisterTimingArea(TimingArea _area)
    {
        t._timingArea = _area; 
    }
    public static void BeginCombatPhase(ICharacter _attacker, ICharacter _defender)
    {
        
    }
    public static void BeginCombatNode(Weapon _weapon, int _round)
    {
        t._timingArea.SetTimings(
            TimingFromWep(_weapon, _round, 0),
            TimingFromWep(_weapon, _round, 1),
            TimingFromWep(_weapon, _round, 2),
            TimingFromWep(_weapon, _round, 3)
            );
    }
    public static void UpdateCombatNode(float _currentTime)
    {
        t._timingArea.UpdateSlider(_currentTime); 
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
    }
}
