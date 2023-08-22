using System;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private Animator _ragdollAnimator;

    public event Action Dead;


}