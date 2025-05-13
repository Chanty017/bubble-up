using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;


public class Player : MonoBehaviour
{
    [Header("Movement Settings")]
    [SerializeField] private float moveSpeed = 7f;
    [SerializeField] private float acceleration = 10f;
    [SerializeField] private float deceleration = 10f;
    [SerializeField] private float jumpForce = 14f;
    [SerializeField] private float coyoteTime = 0.2f;
    [SerializeField] private float jumpBufferTime = 0.2f;
    [SerializeField] private int extraJumpsValue = 1; // Number of extra jumps

    [Header("Ground Detection")]
    [SerializeField] private Transform groundCheckPoint;
    [SerializeField] private float groundCheckRadius = 0.2f;
    [SerializeField] private LayerMask jumpableLayer;

    private Rigidbody2D myRB;
    private PlayerInputs playerInputs;
    private Vector2 moveInput;
    private float lastGroundedTime;
    private float lastJumpTime;
    private bool isFacingRight = true;
    private int extraJumps;

    [Header("Shooting")]
    PlayerShooting playerShooting;
    [SerializeField] Transform bubbleSpawnPoint;

    GameObject currentBubble;

    public Animator myAnim;

    bool isGrounded;

    int score;

    Vector2 startPoint;

    bool canControl;
    public GameObject PauseButton;

    void Awake()
    {
        playerInputs = new PlayerInputs();
        playerInputs.GamePlay.MoveLeft.performed += ctx => MoveLeft();
        playerInputs.GamePlay.MoveRight.performed += ctx => MoveRight();
        playerInputs.GamePlay.MoveLeft.canceled += ctx => MoveCancelled();
        playerInputs.GamePlay.MoveRight.canceled += ctx => MoveCancelled();
        playerInputs.GamePlay.Jump.performed += ctx => Jump();
        playerInputs.GamePlay.Shoot.started += ctx => ShootStarted();
        playerInputs.GamePlay.Shoot.canceled += ctx => ShootCanceled();
    }

    void Start()
    {
        startPoint = transform.position;
        myRB = GetComponent<Rigidbody2D>();
        playerShooting = GetComponent<PlayerShooting>();
        lastJumpTime = -jumpBufferTime - 1f;
        extraJumps = extraJumpsValue;
    }

    void OnEnable()
    {
        playerInputs.GamePlay.Enable();
    }

    void OnDisable()
    {
        playerInputs.GamePlay.Disable();
    }

    void Update()
    {

        Collider2D groundedCollider = Physics2D.OverlapCircle(groundCheckPoint.position, groundCheckRadius, jumpableLayer);
        isGrounded = groundedCollider;

        if (groundedCollider)
        {
            currentBubble = groundedCollider.gameObject;
            lastGroundedTime = Time.time;
            extraJumps = extraJumpsValue; // Reset extra jumps when grounded
        }
        else
        {
            currentBubble = null;
        }

        if (Time.time - lastJumpTime <= jumpBufferTime && (Time.time - lastGroundedTime <= coyoteTime || extraJumps > 0))
        {
            myRB.linearVelocity = new Vector2(myRB.linearVelocity.x, jumpForce);
            lastJumpTime = -1f;
            if (currentBubble != null && currentBubble.CompareTag("Bubble"))
            {
                DestroyBubble(currentBubble);
            }
            if (Time.time - lastGroundedTime > coyoteTime)
            {
                extraJumps--; // Consume an extra jump
            }

            SoundManager.Instance.PlaySFX("jump", .8f);
        }

        ApplyMovement();

        var currentPos = transform.position;
        int differenecY = Mathf.RoundToInt(currentPos.y - startPoint.y);
        if (differenecY > score)
        {
            score = differenecY;
            UIManager.Instance.UpdateScore(score);
            if (score > PlayerPrefs.GetInt("hs"))
            {
                PlayerPrefs.SetInt("hs", score);
            }
        }
    }

    void Jump()
    {
        lastJumpTime = Time.time;

    }

    void MoveLeft()
    {
        moveInput = new Vector2(-1, 0);
        if (isFacingRight)
            Flip();
    }

    void MoveCancelled()
    {
        moveInput = Vector2.zero;
    }

    void MoveRight()
    {
        moveInput = new Vector2(1, 0);
        if (!isFacingRight)
            Flip();
    }

    Coroutine shootingCor;
    void ShootStarted()
    {
        ShootCanceled();
        shootingCor = StartCoroutine(ShootingCor());
    }
    void ShootCanceled()
    {
        if (shootingCor != null)
        {
            StopCoroutine(shootingCor);
        }
    }

    IEnumerator ShootingCor()
    {
        while (true)
        {
            ShootBubble();
            yield return new WaitForSeconds(Random.Range(.15f, .2f));
        }
    }

    void ShootBubble()
    {
        SoundManager.Instance.PlaySFX("pop");
        Vector2 direction = isFacingRight ? Vector2.right : Vector2.left;
        playerShooting.ShootBubble(bubbleSpawnPoint.position, direction, myRB.linearVelocity);
    }

    private void ApplyMovement()
    {
        float targetVelocityX = moveInput.x * moveSpeed;
        float velocityChange = targetVelocityX - myRB.linearVelocity.x;
        float accelerationFactor = (Mathf.Abs(targetVelocityX) > 0.1f) ? acceleration : deceleration;
        float movement = velocityChange * accelerationFactor * Time.deltaTime;

        myRB.linearVelocity = new Vector2(myRB.linearVelocity.x + movement, myRB.linearVelocity.y);

        if (isGrounded)
        {
            if (myRB.linearVelocity.magnitude >= 0.1f)
            {
                PlayAnimation("run");
            }
            else
            {

                PlayAnimation("idle");
            }
        }
        else
        {
            PlayAnimation("jump");
        }

    }

    void PlayAnimation(string animationName)
    {
        if (!IsAnimationPlaying(animationName))
        {
            myAnim.Play(animationName, 0, 0);
        }
    }

    bool IsAnimationPlaying(string animationName)
    {
        AnimatorStateInfo currentState = myAnim.GetCurrentAnimatorStateInfo(0);
        AnimatorStateInfo nextState = myAnim.GetNextAnimatorStateInfo(0);

        return currentState.IsName(animationName) || nextState.IsName(animationName);
    }

    private void Flip()
    {
        isFacingRight = !isFacingRight;
        transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(groundCheckPoint.position, groundCheckRadius);
    }

    bool levelFinished;
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Bubble"))
        {
            DestroyBubble(other.gameObject);
        }
        else if (other.gameObject.CompareTag("Respawn") && !levelFinished)
        {
            levelFinished = true;
            GameOver();
        }
        if (other.gameObject.CompareTag("Spring"))
        {
            Rigidbody2D rb = GetComponent<Rigidbody2D>();


            float springForce = 20f;
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, springForce);

        }

    }

    void DestroyBubble(GameObject bubble)
    {
        Destroy(bubble);
    }


    public void GameOver()
    {
        Debug.Log("GAME OVER!");
        PauseButton.SetActive(false);
        UIManager.Instance.gameOverPanel.SetActive(true);
        UIManager.Instance.LoadScores(score, PlayerPrefs.GetInt("hs", 0));
        SoundManager.Instance.PlaySFX("punch");
        SoundManager.Instance.PlaySFX("gameOver");
        playerInputs.GamePlay.Disable();
        myRB.freezeRotation = false;
        myRB.gravityScale = 3f;
        playerInputs.GamePlay.Disable();
        GetComponent<CapsuleCollider2D>().enabled = false;
        myRB.linearVelocity = new Vector2(0, 10f);
        myRB.AddTorque(100f);

    }

}
