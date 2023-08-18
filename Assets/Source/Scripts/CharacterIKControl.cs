using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterIKControl : MonoBehaviour
{
    [SerializeField] private bool _ikActive;
    [SerializeField] private Transform _rightHandTarget;
    [SerializeField] private Transform _leftHandTarget;
    [SerializeField] private Transform _rightFootTarget;
    [SerializeField] private Transform _leftFootTarget;
    [SerializeField] private Transform _lookAtTarget;

    private Animator animator;

    void Start()
    {
        animator = GetComponent<Animator>();
    }

    void OnAnimatorIK()
    {
        if (animator)
        {
            if (_ikActive)
            {
                SetIk(AvatarIKGoal.RightHand, _rightHandTarget);
                SetIk(AvatarIKGoal.LeftHand, _leftHandTarget);
                SetIk(AvatarIKGoal.RightFoot, _rightFootTarget);
                SetIk(AvatarIKGoal.LeftFoot, _leftFootTarget);

                animator.SetLookAtWeight(1f, 1f, 0f);
                animator.SetLookAtPosition(_lookAtTarget.position);
            }
            else
            {
                ResetIk(AvatarIKGoal.RightHand);
                ResetIk(AvatarIKGoal.LeftHand);
            }
        }
    }

    private void SetIk(AvatarIKGoal goal, Transform target)
    {
        if (target != null)
        {
            animator.SetIKPositionWeight(goal, 1);
            animator.SetIKRotationWeight(goal, 1);
            animator.SetIKPosition(goal, target.position);
            animator.SetIKRotation(goal, target.rotation);
        }
    }

    private void ResetIk(AvatarIKGoal goal)
    {
        animator.SetIKPositionWeight(goal, 0);
        animator.SetIKRotationWeight(goal, 0);
    }
}
