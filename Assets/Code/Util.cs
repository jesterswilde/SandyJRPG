using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System; 

public static class Util
{
    public static void Loop(int _length, Action<int> _action)
    {
        for(int i = 0; i < _length; i++)
        {
            _action(i); 
        }
    }

}

public static class EnumExten
{
   
    public static IEnumerable<T> ForEach<T>(this IEnumerable<T> _list, Action<T, int> _action)
    {
        int i = 0; 
        foreach(T _item in _list)
        {
            _action(_item, i++); 
        }
        return _list; 
    }
    public static IEnumerable<T> ForEach<T>(this IEnumerable<T> _list, Action<T> _action)
    {
        foreach (T _item in _list)
        {
            _action(_item);
        }
        return _list;
    }
    public static List<T> FilterI<T>(this IEnumerable<T> _list, Func<T, int, bool> _action)
    {
        int i = 0;
        List<T> _map = new List<T>(); 
        foreach(T _item in _list)
        {
            if(_action(_item, i++))
            {
                _map.Add(_item); 
            } 
        }
        return _map; 
    }
    public static List<R> SelectI<T, R>(this IEnumerable<T> _list, Func<T, int, R> _action) where T : class
    {
        int i = 0;
        List<R> _map = new List<R>();
        foreach (T _item in _list)
        {
            R _result = _action(_item, i++); 
            if (_result != null)
            {
                _map.Add(_result);
            }
        }
        return _map;
    }
}

public struct AtDir
{
    public Attacks Attack;
    public Direction Direction;
    public AtDir(Attacks attack, Direction direction)
    {
        Attack = attack;
        Direction = direction;
    }
}
