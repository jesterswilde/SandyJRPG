using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq; 

public class CombatManager : MonoBehaviour {

    public static CombatManager t;
    [SerializeField]
    float _nodeDuration = 2f;
    public static float NodeDuration { get { return t._nodeDuration; } }


    [SerializeField]
    Color _attackerColor;
    public static Color AttackerColor { get { return t._attackerColor; } }
    [SerializeField]
    AudioClip _missSFX;
    public static AudioClip MissSFX { get { return t._missSFX; } }
    [SerializeField]
    AudioClip _hitSFX; 
    public static AudioClip HitSFX { get { return t._hitSFX; } }
    [SerializeField]
    AudioClip _heavyDefSFX; 
    List<Player> _players = new List<Player>();
    List<Enemy> _enemies = new List<Enemy>();
    ICharacter _currentAttacker;
    ICharacter _currentDefender;
    int _index;
    CombatPhase _phase; 


    public static void PlaySFX(int _damage, Defense _def)
    {
        if(_damage > 0)
        {
            if(_def != Defense.Heavy)
            {
                SFX.Create(t._hitSFX, Vector3.zero); 
            }else
            {
                SFX.Create(t._heavyDefSFX, Vector3.zero);
            }
        }
        else
        {
            SFX.Create(t._missSFX, Vector3.zero); 
        }
    }
    public void CombatLoop()
    {
        if(_currentAttacker != null)
        {
            _currentAttacker.Deselect(); 
        }
        //enemies turn
        if(_index >= _players.Count)
        {
            _currentAttacker = _enemies[_index - _players.Count];
            _currentDefender = _players.Find((_player) => _player.IsAlive); 
        }
        //players turn
        else
        {
            _currentAttacker = _players[_index];
            _currentDefender = _enemies.Find((_enemy) => _enemy.IsAlive); 
        }
        _currentAttacker.Select(); 
        _phase = new CombatPhase(_currentAttacker, _currentDefender, IsPlayerAttacking(),  ConcludePhase);
    }

    void ConcludePhase()
    {
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
        _index++;
        _index = _index % (_enemies.Count + _players.Count);
        CombatLoop(); 
    }
    bool IsPlayerAttacking()
    {
        return _index < _players.Count; 
    }

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
        t = this; 
    }
    void Start()
    {
        CombatLoop(); 
    }


    void Update()
    {
        if(_phase != null)
        {
            _phase.Update(Time.deltaTime); 
        }
    }
}
