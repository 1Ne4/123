using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    private bool _isOpened;
    [SerializeField] Animator _animator;

    // Update is called once per frame
    public void Open()
    {
        _animator.SetBool("isOpened", _isOpened);
        _isOpened = !_isOpened;
    }
}
