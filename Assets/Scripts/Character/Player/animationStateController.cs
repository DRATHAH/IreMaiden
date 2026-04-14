using UnityEngine;

public class animationStateController : MonoBehaviour
{
    Animator animator;
   /* Rigidbody rb;

    [SerializeField] private float groundCheckDistance = 0.2f;
    [SerializeField] private LayerMask groundLayer; */


    void Start()
    {
        animator = GetComponent<Animator>();
      /*  rb = GetComponent<Rigidbody>(); */
        Debug.Log(animator);
    }

    void Update()
    {
        bool isWalking = animator.GetBool("isRunning");
        bool forwardPressed = Input.GetKey("w");
        bool isCrouch = animator.GetBool("isCrouch");
        bool slidePressed = Input.GetKey("left shift");

       

        if (!isWalking && forwardPressed)
        {
            animator.SetBool("isRunning", true);
        }

        if (isWalking && !forwardPressed)
        {
            animator.SetBool("isRunning", false);
        }

        if (Input.GetKey(KeyCode.Space))
        {
            animator.SetTrigger("Jump");
        }

        if (!isCrouch && slidePressed)
        {
            animator.SetBool("isCrouch", true);
        }

        if (isCrouch && !slidePressed)
        {
            animator.SetBool("isCrouch", false);
        }    
       /* bool isGrounded = Physics.Raycast(
            transform.position,
            Vector3.down,
            groundCheckDistance,
            groundLayer
            );

        bool isFalling = !isGrounded && rb.velocity.y < -0.1f;

        animator.SetBool("isGrounded", isGrounded);
        animator.SetBool("isFalling", isFalling);
   */


        if (Input.GetMouseButtonDown(0))
        {
            animator.SetTrigger("basicAttack");
        }

        if (Input.GetMouseButtonDown(1))
        {
            animator.SetTrigger("fireBall");
        }

    }


}
// commented stuff is me miserably failing to set up falling animation :/