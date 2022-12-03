using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationManager : MonoBehaviour
{
    // Start is called before the first frame update
    private Animator _Animator;
    private void Start()
    {
        _Animator = GetComponent<Animator>();
    }
    public void SetAnimationIdle() => _Animator.SetInteger("Animat", 0);
    public void SetAnimationRun() => _Animator.SetInteger("Animat", 1);
    public void SetAnimationJump() => _Animator.SetInteger("Animat", 2);
    public void SetAnimationAttack() => _Animator.SetInteger("Animat", 3);

}
