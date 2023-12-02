using UnityEngine;

namespace Module.Player.State
{
    public class PauseState : IPlayerState
    {
        private readonly Rigidbody rigidbody;
        private readonly MovementSetting movementSetting;
        private float currentSpeed;

        public PauseState(Rigidbody rigidbody, MovementSetting movementSetting)
        {
            this.rigidbody = rigidbody;
            this.movementSetting = movementSetting;
        }

        public PlayerState GetState()
        {
            return PlayerState.Pause;
        }

        public void Start()
        {
            currentSpeed = rigidbody.velocity.magnitude;
        }

        public void Stop() { }

        public void FixedUpdate()
        {
            currentSpeed -= movementSetting.Resistance * Time.fixedDeltaTime;
            currentSpeed = Mathf.Clamp(currentSpeed, movementSetting.MinSpeed, movementSetting.MaxSpeed);

            rigidbody.velocity = rigidbody.transform.forward * currentSpeed;
        }
    }
}