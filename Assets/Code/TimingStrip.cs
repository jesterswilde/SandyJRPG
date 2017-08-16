using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TimingStrip : MonoBehaviour {

    [SerializeField]
    Transform _bg;
    [SerializeField]
    Transform _fill;
    Vector3 _basePos; 


    void Awake()
    {
        _basePos = _bg.position; 
    }
    void Start()
    {
        _bg.localScale = new Vector3(UIManager.Width, _bg.localScale.y, _bg.localScale.z);
        _bg.position += new Vector3(UIManager.Width * 0.5f, 0, 0);
    }
    public void SetFill(float _range, float _point)
    {
        _fill.transform.position = _basePos + new Vector3(_point * UIManager.ModWidth, 0, 0);
        _fill.transform.localScale = new Vector3(_range * UIManager.ModWidth, _fill.transform.localScale.y, _fill.transform.localScale.z); 
    }
}
