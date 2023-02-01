using MyTonaTechExec.PlayerUnit;
using MyTonaTechExec.Utils;
using UnityEngine;
using Random = UnityEngine.Random;

namespace MyTonaTechExec.MobUnit
{
    public class MobMover : MonoBehaviour, IMobComponent
    {
        public float SightDistance = 5f;
        public float MoveSpeed;
        public bool Active = true;
    
        private Vector3 targetPosition = Vector3.zero;
        private MobAnimator mobAnimator;

        private void Awake()
        {
            mobAnimator = GetComponent<MobAnimator>();
            PickRandomPosition();
            EventBus.EventBus.Sub(OnDeath, EventBus.EventBus.PLAYER_DEATH);
        }

        private void OnDestroy()
        {
            EventBus.EventBus.Unsub(OnDeath, EventBus.EventBus.PLAYER_DEATH);
        }

        private void Update()
        {
            if (Active)
            {
                var playerDistance = (transform.position - Player.Instance.transform.position).Flat().magnitude;
                var targetDistance = (transform.position - targetPosition).Flat().magnitude;
                if (SightDistance >= playerDistance)
                {
                    targetPosition = Player.Instance.transform.position;
                }
                else if (targetDistance < 0.2f)
                {
                    PickRandomPosition();
                }

                var direction = (targetPosition - transform.position).Flat().normalized;

                transform.SetPositionAndRotation(transform.position + direction * (Time.deltaTime * MoveSpeed), Quaternion.LookRotation(direction, Vector3.up));
            }

            mobAnimator.SetIsRun(Active);
        }

        private void PickRandomPosition()
        {
            targetPosition.x = Random.value * 11 - 6;
            targetPosition.z = Random.value * 11 - 6;
        }

        public void OnDeath()
        {
            enabled = false;
        }
    }
}