using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    public bool canOpened = false;
    //private bool _isOpened;
    private int setopen = -1;
    [SerializeField] AudioSource _audio;
    [SerializeField] Animator _animator;
    [SerializeField] GlobalsoundScream audioGlobal;

    // Update is called once per frame
    public void Open()
    {
        if (canOpened && setopen == -1)
        {
            _audio.Play();
            setopen *= -1;
            _animator.Play("OpenbackDoor");
            audioGlobal.ScreamAudio();
            //_animator.SetFloat("New Float", setopen);
        }
    }
}
