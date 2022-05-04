using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DeathStateNameSpace
{
    public class ShootingState : DeathState
    {
        public int attackDamage = 1;
        [Header("Death刀气预制件")]
        public Bullet bullet;
        // public AudioSource attackAudio;
        private int shootDir;
        public float shootCd;

        private float lastShootTime;

        public override bool CanEnter(Death death)
        {
            if (Time.time < lastShootTime + shootCd)
            {
                return false;
            }
            return true;
        }

        public override void Enter(Death death_)
        {
            base.Enter(death_);
            GetEndTime("Death_Shoot");
            death_.Anim_.Animator_.SetTrigger("Shoot");
        }

        public override DeathState HandleCommand(TranslationCommand translationCommand, Command actionCommand)
        {
            shootDir = (int)translationCommand.Horizontal;
            return null;
        }

        public override void Exit()
        {
            lastShootTime = Time.time;
            Quaternion bulletRotation = death_.transform.rotation;
            bulletRotation.y = (death_.transform.rotation.y == 0) ? 180 : 0;
            Instantiate(bullet, death_.transform.position + death_.transform.right * (-1), bulletRotation);
        }

        protected override void Start()
        {
            
        }

        public override void StateUpdate()
        {
            death_.Physics_.Rb.velocity = Vector2.zero;
            if (Time.time > stateTime)
            {
                Exit();
                death_.TranslationState(teleporting);
            }
        }

        public override void StateFixedUpdate()
        {
            death_.Physics_.HorizontalMove(0, 0);
        }
    }
}