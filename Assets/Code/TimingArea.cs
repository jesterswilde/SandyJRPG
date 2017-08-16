using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimingArea : MonoBehaviour {

    [SerializeField]
    TimingStrip _up;
    [SerializeField]
    TimingStrip _right;
    [SerializeField]
    TimingStrip _down;
    [SerializeField]
    TimingStrip _left;
    [SerializeField]
    Transform _slider;

    Vector3 _basePos; 

    public void SetTimings(float[] ups, float[] rights, float[] downs, float[] lefts)
    {
        _up.gameObject.SetActive(ups[0] > 0);
        _up.SetFill(ups[0], ups[1]); 
        _right.gameObject.SetActive(rights[0] > 0);
        _right.SetFill(rights[0], rights[1]);
        _down.gameObject.SetActive(downs[0] > 0);
        _down.SetFill(downs[0], downs[1]);
        _left.gameObject.SetActive(lefts[0] > 0);
        _left.SetFill(lefts[0], lefts[1]);
    }

    public void UpdateSlider(float _currentTime)
    {
        float _x = _basePos.x + UIManager.ModWidth * _currentTime;
        _slider.position = new Vector3(_x, _slider.position.y, _slider.position.z); 
    }
    void Awake()
    {
        _basePos = _slider.position; 
    }

    void Start()
    {
        UIManager.RegisterTimingArea(this); 
    }
}
