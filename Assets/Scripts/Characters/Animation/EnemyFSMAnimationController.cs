using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyFSMAnimationController : MonoBehaviour
{
    [SerializeField] private Animator _animator;

    public void SetState(ANIMSTATE state)
    {
        if (_animator != null)
        {
            _animator.SetInteger("State", (int)state);
        }
    }
}