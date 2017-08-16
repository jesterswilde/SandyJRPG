using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System; 

public static class Util
{

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
}
