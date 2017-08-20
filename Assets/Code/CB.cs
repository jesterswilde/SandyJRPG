using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq; 


//I am making this so I can store actions and time in one list. 
public class DoIn
{
    public Action Action;
    public float RemainingTime { get { return _remainingTime; } }
    float _remainingTime; 
    public void PassTime(float _delta)
    {
        _remainingTime -= _delta; 
    }
    public DoIn(Action _action, float _delay)
    {
        Debug.Log("making a cb"); 
        Action = _action;
        _remainingTime = _delay;
    }
}
public class CB  {
    //We have a list of the above structs
    List<DoIn> _cbs = new List<DoIn>(); 
    public void AddCB(Action _action, float _delay)
    {
        _cbs.Add(new DoIn(_action, _delay));
    }

    public void Update(float _delta)
    {
        //This will be used later for an efficiency thing.
        bool _shouldFilter = false; 
        _cbs.ForEach((_cb) =>
        {
            //we take how much time is remaining until I am supposed to execute this callback
            //lower it by how much time has passed.
            _cb.PassTime(_delta);
            Debug.Log(_cb.RemainingTime + " | " + _delta); 
            //if We are now below 0, it's time to call the callback.
            if (_cb.RemainingTime < 0)
            {
                _cb.Action();
                //We also want to filter it out, since it no longer is doing anything useful
                _shouldFilter = true; 
            }
        });
        if (_shouldFilter)
        {
            Debug.Log("filtering"); 
            //Where creates a new List-like-thing of everything that passes it's boolean test.
            //Since it's not actually a list, we convert it to a list with .toList()
            _cbs = _cbs.Where((_cb) => _cb.RemainingTime > 0).ToList(); 
        }
    }
	
}
