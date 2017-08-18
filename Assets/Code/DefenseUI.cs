using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; 

public class DefenseUI : MonoBehaviour {

    [SerializeField]
    Image _up;
    [SerializeField]
    Image _down;
    [SerializeField]
    Image _left;
    [SerializeField]
    Image _right;

    ICharacter _char;

    public void SetDefender(ICharacter character)
    {
        transform.position = character.transform.position;
        _char = character; 
    }
    public void UpdateFacing()
    {
        _up.color = ColorDefense(_char.DefenseInDirection(Direction.Up));
        _right.color = ColorDefense(_char.DefenseInDirection(Direction.Right));
        _down.color = ColorDefense(_char.DefenseInDirection(Direction.Down));
        _left.color = ColorDefense(_char.DefenseInDirection(Direction.Left));

    }
    Color ColorDefense(Defense _def)
    {
        switch (_def)
        {
            case Defense.Heavy: return UIManager.HeavyDefense;
            case Defense.Light: return UIManager.LightDefense;
            default: return UIManager.NoDefense; 
        }
    }
}
