using MyTonaTechExec.EventBus;
using MyTonaTechExec.EventBus.Messages;
using UnityEngine;

namespace MyTonaTechExec.PlayerUnit
{
    public class PlayerMover : MonoBehaviour
    {
        private void Awake()
        {
            EventBus<PlayerInputMessage>.Sub(InputHandler);
        }

        private void OnDestroy()
        {
            EventBus<PlayerInputMessage>.Unsub(InputHandler);
        }

        private void InputHandler(PlayerInputMessage message)
        {
            var speed = GetComponent<Player>().MoveSpeed;
            var delta = new Vector3(speed * message.MovementDirection.x, 0, speed * message.MovementDirection.y) * Time.deltaTime;
            transform.position += delta;
            transform.forward = new Vector3(message.AimDirection.x, 0, message.AimDirection.y);
        }
    }
}