using MyTonaTechExec.PlayerUnit;
using MyTonaTechExec.Utils;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

namespace MyTonaTechExec.MobUnit
{
    public class MobMover : MonoBehaviour, IMobComponent
    {
        [FormerlySerializedAs("SightDistance")]
        [SerializeField]
        private float _sightDistance = 5f;
        [FormerlySerializedAs("MoveSpeed")]
        [SerializeField]
        private float _moveSpeed;
        [FormerlySerializedAs("Active")]
        [SerializeField]
        private bool _active = true;

        private Vector3 _targetPosition = Vector3.zero;
        private MobAnimator _mobAnimator;

        public bool Active
        {
            get => _active;
            set => _active = value;
        }

        private void Awake()
        {
            _mobAnimator = GetComponent<MobAnimator>();
            PickRandomPosition();
            EventBus.EventBus.Sub(OnDeath, EventBus.EventBus.PLAYER_DEATH);
        }

        private void OnDestroy()
        {
            EventBus.EventBus.Unsub(OnDeath, EventBus.EventBus.PLAYER_DEATH);
        }

        private void Update()
        {
            if (_active)
            {
                var playerDistance = (transform.position - Player.Instance.transform.position).Flat().magnitude;
                var targetDistance = (transform.position - _targetPosition).Flat().magnitude;
                if (_sightDistance >= playerDistance)
                {
                    _targetPosition = Player.Instance.transform.position;
                }
                else if (targetDistance < 0.2f)
                {
                    PickRandomPosition();
                }

                var direction = (_targetPosition - transform.position).Flat().normalized;

                transform.SetPositionAndRotation(transform.position + direction * (Time.deltaTime * _moveSpeed), Quaternion.LookRotation(direction, Vector3.up));
            }

            _mobAnimator.SetIsRun(_active);
        }

        private void PickRandomPosition()
        {
            _targetPosition.x = Random.value * 11 - 6;
            _targetPosition.z = Random.value * 11 - 6;
        }

        public void OnDeath()
        {
            enabled = false;
        }
    }
}