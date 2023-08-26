using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlipTrigger : MonoBehaviour
{
    [SerializeField] private FlipTriggerDirection _direction;

    public FlipTriggerDirection Direction => _direction;
}
