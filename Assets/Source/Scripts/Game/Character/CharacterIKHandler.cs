using UnityEngine;

namespace BikeDefied.Game.Character
{
    public class CharacterIKHandler : MonoBehaviour
    {
        [SerializeField] private Animator _animator;

        [SerializeField] private bool _ikActive;
        [SerializeField] private Transform _hipsPoint;
        [SerializeField] private Transform _rightHandPoint;
        [SerializeField] private Transform _leftHandPoint;
        [SerializeField] private Transform _rightLegPoint;
        [SerializeField] private Transform _leftLegPoint;
        [SerializeField] private Transform _lookPoint;

        private void OnAnimatorIK()
        {
            if (_animator)
            {
                if (_ikActive)
                {
                    _animator.SetLookAtWeight(1);
                    _animator.SetLookAtPosition(_lookPoint.position);

                    _animator.bodyPosition = _hipsPoint.position;
                    _animator.bodyRotation = _hipsPoint.rotation;

                    SetIk(AvatarIKGoal.RightHand, _rightHandPoint);
                    SetIk(AvatarIKGoal.LeftHand, _leftHandPoint);
                    SetIk(AvatarIKGoal.RightFoot, _rightLegPoint);
                    SetIk(AvatarIKGoal.LeftFoot, _leftLegPoint);
                }
                else
                    ResetAllIk();
            }
        }

        private void SetIk(AvatarIKGoal goal, Transform point)
        {
            _animator.SetIKPositionWeight(goal, 1);
            _animator.SetIKRotationWeight(goal, 1);
            _animator.SetIKPosition(goal, point.position);
            _animator.SetIKRotation(goal, point.rotation);
        }

        private void ResetAllIk()
        {
            _animator.SetIKPositionWeight(AvatarIKGoal.RightHand, 0);
            _animator.SetIKRotationWeight(AvatarIKGoal.RightHand, 0);
            _animator.SetLookAtWeight(0);
        }
    }
}