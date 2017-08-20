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

    //Moves the UI over the target character
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

        //switch statements are just a convention for a list of if statements
        //We are saying If(_def == defense.heavy) then for the next one (if _def == defense.light) etc
        switch (_def)
        {
            case Defense.Heavy: return UIManager.HeavyDefense;
            case Defense.Light: return UIManager.LightDefense;
            default: return UIManager.NoDefense; 
        }
    }
}
