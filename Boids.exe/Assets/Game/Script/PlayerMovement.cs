using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("Movement")]
    public float moveSpeed;

    public float groundDrag;

    public float jumpForce;
    public float jumpCooldown;
    public float airMultiplier;
    bool readyToJump;

    public float upForce;
    public float downForce;
    public float speedInWater;

    [Header("KeyBinds")]
    public KeyCode jumpKey = KeyCode.Space;

    public KeyCode up = KeyCode.Q;
    public KeyCode down = KeyCode.E;

    [Header("Water/Ground Check")]
    public CapsuleCollider player;
    bool onGround = false;
    bool inWater = true;

    public Transform orientation;

    float horizontalInput;
    float verticalInput;

    Vector3 moveDirection;

    Rigidbody rb;

    private void Start()
    {
        ResetJump();
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;

        rb.drag = 5;

        //récupère le composant capsule collider
        player = GetComponent<CapsuleCollider>();
        RenderSettings.fog = false;
    }

    private void Update()
    {

        MyInput();
        SpeedControl();

        //force vers le sol
        if (inWater)
        {
            rb.useGravity = false;
            moveSpeed = speedInWater;
        }
        else
        {
            rb.useGravity = true;
            moveSpeed = 7;
        }
    }

    private void FixedUpdate()
    {
        MovePlayer();
    }

    private void MyInput()
    {
        horizontalInput = Input.GetAxisRaw("Horizontal");
        verticalInput = Input.GetAxisRaw("Vertical");

        //quand le joueur veut sauter
        if (Input.GetKey(jumpKey) && readyToJump && !inWater)
        {
            rb.drag = 0;
            readyToJump = false;

            Jump();
            rb.drag = 5;
            Invoke(nameof(ResetJump), jumpCooldown);

        }

        if (Input.GetKey(up) && inWater)
        {
            rb.AddForce(0,upForce,0,ForceMode.Force);
        }

        if (Input.GetKey(down) && inWater)
        {
            rb.AddForce(0,-upForce,0,ForceMode.Force);
        }
    }

    private void MovePlayer()
    {
        //calcule le mouvement et la force a donné dans la direction visé
        moveDirection = orientation.forward * verticalInput + orientation.right * horizontalInput;

        //sur terre
        if (onGround)
            rb.AddForce(moveDirection.normalized * moveSpeed * 10f, ForceMode.Force);

        //en l'air
        else
            rb.AddForce(moveDirection.normalized * moveSpeed * 10f * airMultiplier, ForceMode.Force);
    }

    private void SpeedControl()
    {
        Vector3 flatVel = new Vector3(rb.velocity.x, 0f, rb.velocity.z);

        //limite la velocity si nécessaire
        if (flatVel.magnitude > moveSpeed)
        {
            Vector3 limitedVel = flatVel.normalized * moveSpeed;
            rb.velocity = new Vector3(limitedVel.x, rb.velocity.y, limitedVel.z);
        }
    }

    private void OnTriggerStay(Collider other)
    {

        //Detecte si le joueur est a l'interieur de l'eau ou non
        if (player.bounds.min.x > other.bounds.min.x && player.bounds.max.x < other.bounds.max.x &&
           player.bounds.min.y > other.bounds.min.y && player.bounds.max.y < other.bounds.max.y &&
           player.bounds.min.z > other.bounds.min.z && player.bounds.max.z < other.bounds.max.z)
        {
            inWater = true;
            onGround = false;
            RenderSettings.fog = true;
        }
        else
        {
            onGround = true;
            inWater = false;
            RenderSettings.fog = false;
        }

    }

    private void Jump()
    {
        //remmet à zéro la velociter à l'axe y
        rb.velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);

        rb.AddForce(transform.up * jumpForce, ForceMode.Impulse);
    }

    private void ResetJump()
    {
        readyToJump = true;
    }
}
