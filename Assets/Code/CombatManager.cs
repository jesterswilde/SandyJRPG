using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq; 

public class CombatManager : MonoBehaviour {

    //This is the top level component for combat. All other components react to this in some way

    //This is my singleton pattern. This allows everyone to reference methods on the manager without
    //needing to actually have it passed in
    public static CombatManager t;
    //Serializing the field allows a private field to show up in the inspector *see Reflection for more detail if interested*
    [SerializeField]
    float _nodeDuration = 2f;
    //This next bit is how the duration becomes publicly available thorugh my singleton pattern, you'll see it a lot
    public static float NodeDuration { get { return t._nodeDuration; } }
    [SerializeField]
    float _pauseBetweenPhases = 1f; 


    [SerializeField]
    Color _attackerColor;
    public static Color AttackerColor { get { return t._attackerColor; } }

    //In a more complex version move these to a sound manager.
    [SerializeField]
    AudioClip _missSFX;
    [SerializeField]
    AudioClip _hitSFX; 
    [SerializeField]
    AudioClip _heavyDefSFX; 


    List<Player> _players = new List<Player>();
    List<Enemy> _enemies = new List<Enemy>();

    //The I in front of a variable name is a convention to denote an Interface. Interface explanation in ICharacter
    ICharacter _currentAttacker;
    ICharacter _currentDefender;

    //This functions as the index (i) in whose turn it is. 
    int _initiative;

    /*
     * Combat is broken down into different levels. Each level only knows about one level down from it.
     * Each level is handled by it's own class. 
     * CombatManager - This controls the entire combat. Combat loop is a function that you can think of as a single initiative pass.
     *   Combat Phase - This any given characters actuall attack phase. One attacker and one defender.
     *     Combat Node - This is the actual attack and resolution of that attac. 
     */
     //Decleration of a combat phase that will be assigned later. 
    CombatPhase _phase;

    //CB stands for Callback. This is a class that allows me to easily have pauses happen.
    CB _cbs = new CB(); 


    //Interprets the outcome of an attack and plays a sound accordingly. Will be moved later
    public static void PlaySFX(int _damage, Defense _def)
    {
        if(_damage > 0)
        {
            if(_def != Defense.Heavy)
            {
                //Damage is greater than 0 (meaning they swung and hit) and didn't hit heavy defense
                //The position thing is a relic I didn't fix. But it allows the sound to be placed specifically if I cared about that.
                SFX.Create(t._hitSFX, Vector3.zero); 
            }else
            {
                //since we are still in the (_damage > 0) bracket, the else means 'we did damage, but it hit heavy guard'
                SFX.Create(t._heavyDefSFX, Vector3.zero);
            }
        }
        else
        {
            //no damage was dealt
            SFX.Create(t._missSFX, Vector3.zero); 
        }
    }

    //This is the meat of the combat
    public void CombatLoop()
    {

        //At the start of the turn, we are doing cleanup for any previous combat loops that may have happened.
        //The only instance in which _currentAttacker will be null is the very first time we call this.
        if(_currentAttacker != null)
        {
            //select and de select are just about colors, but could be more later. Important hook.
            _currentAttacker.Deselect(); 
        }
        /*The way I structured this, which is pretty hacky is I took the list of all the players, and then added the list of
         * enemies to it. So <Player, Player> <Enemy, Enemy> Even though this is 2 lists, I treat it like one
         * so if it is initiative 2, that mean we use the  1st enemy (remember, we start from 0)
         */ 
        //enemies turn
        if(_initiative >= _players.Count)
        {
            _currentAttacker = _enemies[_initiative - _players.Count];
            //This is a 'lambda' expression or an 'anonymous' function. 
            //The first thing to note about Find is that it's a method of _players. So we are talking about the list of _players
            //Find loops over every element in our list (players) and returns the first one to pass it's test. Meaning the first one that returns a true statement
            //so Find(....) is passed (_player) => _player.isAlive
            //In one liners, it has an implied return statement. expanded it would look like
            /*
             * bool FindPlayer(Player _player){
             *      return _player.isAlive; 
             * }
             */
            _currentDefender = _players.Find((_player) => _player.IsAlive); 
        }
        //players turn
        else
        {
            _currentAttacker = _players[_initiative];
            _currentDefender = _enemies.Find((_enemy) => _enemy.IsAlive); 
        }

        if (_currentAttacker.IsAlive)
        {
            _currentAttacker.Select();

            //now that we've figured out who is attacking and defedning, we can begin our combatPhase.
            //We pass in the result of IsPlayerAttacking so that combat phase will know who gets controlled by the player
            // and who gets controlled by the AI
            //The last argument is actually a function. C# calls it a delegate, I referred to it earlier as a callback.
            _phase = new CombatPhase(_currentAttacker, _currentDefender, IsPlayerAttacking(), ConcludePhase);
        }
        else
        {
            //if it is hte phase of a dead attacker, we move on.
            ConcludePhase(); 
        }
    }

    //This get's passed to a phase. And when has decided it's over, it executes this function. Passing control back up to the manager.
    void ConcludePhase()
    {
        //This is jsut like the _players.Find from earlier, but instead of returning the first thing to pass the truth test
        //All simply returns a boolean of true or false. Stating "did all elements pass or fail" 
        //I'm asking "Are all players dead (not alive)"
        if (_players.All((_player) => !_player.IsAlive))
        {
            Debug.Log("You've died, filthy casual");
            return;
        }
        if (_enemies.All((_enemy) => !_enemy.IsAlive))
        {
            Debug.Log("You've killed them, will you help their family mourn or steal all their money?");
            return;
        }
        //We then increment initiative so it will beocme the next persons turn
        _initiative++;
        //we make sure that initiative can't exceed the number of combatants. If it does, we reset the initiative to 0.
        _initiative = _initiative % (_enemies.Count + _players.Count);
        //Once more! But after a brief pause
        _cbs.AddCB(CombatLoop, t._pauseBetweenPhases); 
    }

    //Because of how we structured the initiative, players go first. 
    bool IsPlayerAttacking()
    {
        return _initiative < _players.Count; 
    }

    //This is a method all players call when created so I know they exist.
    public static void RegisterPlayer(Player _player)
    {
        t._players.Add(_player); 
    }
    public static void RegisterEnemy(Enemy _enemy)
    {
        t._enemies.Add(_enemy); 
    }
    void Awake()
    {
        //singleton set up
        t = this; 
    }
    void Start()
    {
        //kicks it all off
        CombatLoop(); 
    }


    void Update()
    {
        //Almost nothing else acts on Unity's default loop and take the time passed from it in the update step
        _cbs.Update(Time.deltaTime); 
        if(_phase != null)
        {
            _phase.Update(Time.deltaTime); 
        }
    }
}
