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
    //This is a bit weird math. We are making a number that is the inverse of duration. 
    //Since we get passed in time a bunch, such as "at second 1.50, place the hit bar" We want to just 
    //slam 1.50 in and have that be at (in this case) and have be at 3/4 of the way down the bar already
    public static float ModWidth { get { return 1 / CombatManager.NodeDuration * Width; } }

    [SerializeField]
    DefenseUI _defenseUI; 

    public static void RegisterTimingArea(TimingArea _area)
    {
        t._timingArea = _area; 
    }

    //A hook for later
    public static void BeginCombatPhase(ICharacter _attacker, ICharacter _defender)
    {
        
    }
    public static void BeginCombatNode(Weapon _weapon, int _round, ICharacter _defender)
    {
        if(t._timingArea != null){
            //When combat starts, each bar needs to know if it exists, and where it can be clicked
            //The only thing that knows this is the weapon. We need which round we are in, and then which direction. 
            //The directions have been very specifically set so that 0 = up, 1 = right, 2 = down, and 3 = left.
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
        //Not all directions have the same lengths
        //and there aren't attacks at all rounds
        if(_dir.Count > _round && _dir[_round] != null)
        {
            //if we can attack at this round, in the set direction, return those values
            return new float[] { _dir[_round].Range, _dir[_round].HitTime };
        }else
        {
            //This isn't the best, but I'm using negative numbers to say 'nothing here' as I can't use null
            return new float[] { -1f, -1f }; 
        }
    }

    void Awake()
    {
        t = this;
        _defenseUI = Instantiate(_defenseUI); 
    }
}
