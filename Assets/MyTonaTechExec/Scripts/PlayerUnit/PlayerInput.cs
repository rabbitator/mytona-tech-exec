using MyTonaTechExec.EventBus;
using MyTonaTechExec.EventBus.Messages;
using UnityEngine;
using UnityEngine.Serialization;

namespace MyTonaTechExec.PlayerUnit
{
    public class PlayerInput : MonoBehaviour
    {
        [FormerlySerializedAs("Camera")]
        [SerializeField]
        public Camera _camera;
        [FormerlySerializedAs("Player")]
        [SerializeField]
        public Player _player;

        private void Awake()
        {
            EventBus.EventBus.Sub(PlayerDeadHandler, EventBus.EventBus.PLAYER_DEATH);
        }

        private void OnDestroy()
        {
            EventBus.EventBus.Unsub(PlayerDeadHandler, EventBus.EventBus.PLAYER_DEATH);
        }

        private void PlayerDeadHandler()
        {
            enabled = false;
        }

        void Update()
        {
            var moveInput = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));

            var ray = _camera.ScreenPointToRay(Input.mousePosition);

            var plane = new Plane(Vector3.up, Vector3.up * _player.transform.position.y);
            plane.Raycast(ray, out var enter);
            var aimPos = ray.GetPoint(enter);
            var aimInput = aimPos - _player.transform.position;


            var fire = Input.GetKey(KeyCode.Mouse0);
            EventBus<PlayerInputMessage>.Pub(new PlayerInputMessage()
            {
                MovementDirection = moveInput.normalized,
                AimDirection = new Vector2(aimInput.x, aimInput.z).normalized,
                Fire = fire
            });
        }
    }
}