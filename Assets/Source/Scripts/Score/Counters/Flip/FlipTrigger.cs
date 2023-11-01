using UnityEngine;

namespace BikeDefied.ScoreSystem
{
    public class FlipTrigger : MonoBehaviour
    {
        [SerializeField] private FlipTriggerDirection _direction;

        public FlipTriggerDirection Direction => _direction;
    }
}