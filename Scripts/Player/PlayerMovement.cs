using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [Header("Movement")]
    public float moveSpeed = 5f;

    [Header("Touch Joystick (Mobile)")]
    public TouchJoystick touchJoystick;

    [Header("Sprite")]
    public SpriteRenderer spriteRenderer;
    private Rigidbody2D rb;
    private Vector2 moveInput;
    private PlayerControls controls;

    void Awake()
    {
        controls = new PlayerControls();
    }

    void OnEnable()
    {
        controls.Player.Enable();
        controls.Player.Move.performed += OnMove;
        controls.Player.Move.canceled += OnMove;
    }

    void OnDisable()
    {
        controls.Player.Move.performed -= OnMove;
        controls.Player.Move.canceled -= OnMove;
        controls.Player.Disable();
    }

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        // Se o spriteRenderer não for setado no Inspector, tenta pegar automaticamente
        if (spriteRenderer == null)
        {
            spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        }
    }

    void FixedUpdate()
    {
        Vector2 finalInput = moveInput;

#if UNITY_ANDROID || UNITY_IOS
        if (touchJoystick != null && touchJoystick.GetInput().sqrMagnitude > 0.01f)
        {
            finalInput = touchJoystick.GetInput();
        }
#endif
        rb.velocity = finalInput * moveSpeed;

        if (finalInput.x > 0.01f)
        {
            spriteRenderer.flipX = true;  
        }
        else if (finalInput.x < -0.01f)
        {
            spriteRenderer.flipX = false; 
        }
    }

    private void OnMove(InputAction.CallbackContext context)
    {
        moveInput = context.ReadValue<Vector2>();
    }
}
