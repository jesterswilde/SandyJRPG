using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInput : MonoBehaviour {

    public static Direction GetDirection()
    {
        if (Input.GetKeyDown(KeyCode.W))
        {
            return Direction.Up;
        }
        if (Input.GetKeyDown(KeyCode.S))
        {
            return Direction.Down;
        }
        if (Input.GetKeyDown(KeyCode.A))
        {
            return Direction.Left;
        }
        if (Input.GetKeyDown(KeyCode.D))
        {
            return Direction.Right;
        }
        return Direction.None;
    }
}
