using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SFX : MonoBehaviour {

    AudioSource _source; 
    public AudioSource Source { get { return _source; } set { _source = value; } }


    public static void Create(AudioClip _clip, Vector3 _pos)
    {
        GameObject _go = new GameObject();
        _go.transform.position = _pos;
        AudioSource _source = _go.AddComponent<AudioSource>();
        SFX _sfx = _go.AddComponent<SFX>();
        _source.clip = _clip; 
        _source.Play();
        _sfx.Source = _source; 
    }

    void Update()
    {
        if(_source == null || !_source.isPlaying)
        {
            Destroy(gameObject); 
        }
    }
}
