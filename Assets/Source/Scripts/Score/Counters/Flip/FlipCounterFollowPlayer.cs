using BikeDefied.BikeSystem;
using Reflex.Attributes;
using UnityEngine;

namespace BikeDefied.ScoreSystem
{
    public class FlipCounterFollowPlayer : MonoBehaviour
    {
        private Transform _bike;

        [Inject]
        private void Inject(Bike bike) =>
            _bike = bike.transform;

        private void Update() =>
            transform.position = _bike.position;
    }
}