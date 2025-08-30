using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimManager : MonoBehaviour
{
    private Animator anim;

    private bool isRunning = false;
    private bool isSneaking = false;

    private void Start()
    {
        anim = GetComponentInChildren<Animator>();

    }


}
