using UnityEngine;

public class PlayerMove : MonoBehaviour {

    [Header("Attributes")]
    public float moveSpeed = 200f;
    public float jumpHeight = 10f;
    public float jumpCutHeight = 5f;

    [Header("Grounding")]
    public float groundCheckRadius;
    public Transform groundCheck;
    public LayerMask groundCheckLayer;

    private bool jumpRequest;
    private bool jumpCutRequest;
    private bool facingRight = true;
    private bool isGrounded;
    private Vector3 moveInput;
    private Vector3 moveVelocity;
    private Rigidbody2D rb;

    void Start () {
        rb = GetComponent<Rigidbody2D>();
    }

    void OnDrawGizmosSelected () {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(groundCheck.position, groundCheckRadius);
    }


    void Update () {
        CheckInput();
    }

    void FixedUpdate () {
        // horizontal movement
        CheckMove();
        // jumprequests
        CheckJump();
    }

    private void CheckInput () {
        // horizontal input
        float horizontal = Input.GetAxisRaw("Horizontal");
        // sprite flipping
        if (horizontal > 0) {
            facingRight = true;
            Flip();
        }

        else if (horizontal < 0) {
            facingRight = false;
            Flip();
        }
        // apply input to the input and velocity vectors
        moveInput = new Vector3(horizontal, 0f);
        moveVelocity = moveInput.normalized * moveSpeed;
        // fix for one way platforms
        if (rb.velocity.y > 0) {
            isGrounded = false;
        }
        // grounding
        else {
            isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundCheckLayer);
        }
        // if pressing jump we jump
        if(Input.GetButton("Jump") && isGrounded) {
            jumpRequest = true;
        }
        // if releasing jump we jump cut
        if(Input.GetButtonUp("Jump")) {
            if (rb.velocity.y > jumpCutHeight) {
                jumpCutRequest = true;
            }
        }
    }

    private void CheckMove () {
        rb.velocity = new Vector3(moveVelocity.x * Time.deltaTime, rb.velocity.y);
    }

    // using velocity instead of addforce for more control
    private void CheckJump () {
        // jump
        if (jumpRequest) {
            rb.velocity = new Vector3(rb.velocity.x, jumpHeight);
            jumpRequest = false;
        }
        // jump cut
        else if (jumpCutRequest) {
            rb.velocity = new Vector3(rb.velocity.x, jumpCutHeight);
            jumpCutRequest = false;
        }
    }

    private void Flip () {
        // basic sprite flipping
        Vector3 scale = transform.localScale;
        if (facingRight) {
            scale.x = 1;
        }

        else {
            scale.x = -1;
        }

        transform.localScale = scale;
    }
}
