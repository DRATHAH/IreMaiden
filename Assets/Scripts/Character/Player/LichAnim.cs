using UnityEngine;

namespace IM
{
    [RequireComponent(typeof(Animator))]
    public class PlayerAnimationManager : MonoBehaviour
    {
        private Animator animator;
        private PlayerLocomotionManager locomotion;
        private Rigidbody rb;

        private bool wasGrounded;

        private void Awake()
        {
            animator = GetComponent<Animator>();
            locomotion = PlayerLocomotionManager.Instance;
            rb = locomotion.GetRb();
        }

        private void Update()
        {
            if (!locomotion.CanMove)
                return;

            UpdateLocomotion();
            UpdateAirStates();
            UpdateCrouch();
        }

        private void UpdateLocomotion()
        {
            Vector3 flatVelocity = new Vector3(rb.linearVelocity.x, 0f, rb.linearVelocity.z);
            animator.SetFloat("Speed", flatVelocity.magnitude);
        }

        private void UpdateAirStates()
        {
            animator.SetBool("IsGrounded", locomotion.grounded);
            animator.SetFloat("VerticalVelocity", rb.linearVelocity.y);

            // Jump trigger (leaving ground)
            if (wasGrounded && !locomotion.grounded && rb.linearVelocity.y > 0.1f)
            {
                animator.SetTrigger("Jump");
            }

            // Land trigger
            if (!wasGrounded && locomotion.grounded)
            {
                animator.SetTrigger("Land");
            }

            wasGrounded = locomotion.grounded;
        }

        private void UpdateCrouch()
        {
            animator.SetBool("IsCrouching", locomotion.IsCrouching());
        }

        // -------- ACTION TRIGGERS (CALL FROM OTHER SCRIPTS) --------

        public void PlayBasicAttack()
        {
            animator.SetTrigger("BasicAttack");
        }

        public void PlayFireball()
        {
            animator.SetTrigger("Fireball");
        }

        public void PlayHandSpell()
        {
            animator.SetTrigger("HandSpell");
        }

        public void PlayDash()
        {
            animator.SetBool("IsDashing", true);
        }

        public void StopDash()
        {
            animator.SetBool("IsDashing", false);
        }
    }
}