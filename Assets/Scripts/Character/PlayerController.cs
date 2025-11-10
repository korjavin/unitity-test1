using UnityEngine;

/// <summary>
/// PlayerController handles all character movement including walking, running, jumping, and ducking.
/// This script demonstrates core Unity concepts like:
/// - Input handling
/// - Physics-based movement
/// - State management
/// - Component interaction
///
/// EDUCATIONAL NOTE: This is a MonoBehaviour, which is Unity's base class for scripts
/// that can be attached to GameObjects in your scene.
/// </summary>
[RequireComponent(typeof(CharacterController))] // This ensures a CharacterController is always present
public class PlayerController : MonoBehaviour
{
    // ============================================================================
    // MOVEMENT CONFIGURATION
    // ============================================================================
    // [Header] creates a nice section in the Unity Inspector for organization
    // [Tooltip] shows helpful hints when you hover over these fields in Unity

    [Header("Movement Speeds")]
    [Tooltip("How fast the character walks (meters per second)")]
    [SerializeField] private float walkSpeed = 3.0f;

    [Tooltip("How fast the character runs (meters per second)")]
    [SerializeField] private float runSpeed = 6.0f;

    [Tooltip("How fast the character moves while ducking")]
    [SerializeField] private float duckSpeed = 1.5f;

    [Tooltip("How quickly the character accelerates to target speed")]
    [SerializeField] private float accelerationRate = 10.0f;

    // ============================================================================
    // JUMPING CONFIGURATION
    // ============================================================================
    [Header("Jump Settings")]
    [Tooltip("The upward velocity applied when jumping")]
    [SerializeField] private float jumpForce = 8.0f;

    [Tooltip("Additional downward force applied when falling (makes jumps feel more responsive)")]
    [SerializeField] private float gravityMultiplier = 2.5f;

    [Tooltip("How many times can the player jump in the air? (0 = only ground jumps, 1 = double jump)")]
    [SerializeField] private int maxAirJumps = 0;

    // ============================================================================
    // DUCKING/CROUCHING CONFIGURATION
    // ============================================================================
    [Header("Duck Settings")]
    [Tooltip("The height of the character when ducking")]
    [SerializeField] private float duckHeight = 1.0f;

    [Tooltip("The normal standing height of the character")]
    [SerializeField] private float normalHeight = 2.0f;

    [Tooltip("How quickly the character transitions between standing and ducking")]
    [SerializeField] private float duckTransitionSpeed = 8.0f;

    // ============================================================================
    // ROTATION CONFIGURATION
    // ============================================================================
    [Header("Rotation Settings")]
    [Tooltip("How quickly the character rotates to face movement direction")]
    [SerializeField] private float rotationSpeed = 10.0f;

    // ============================================================================
    // COMPONENT REFERENCES
    // ============================================================================
    // These are references to other Unity components we need to interact with
    private CharacterController characterController; // Handles collision and movement
    private Animator animator; // Controls character animations (optional)

    // ============================================================================
    // MOVEMENT STATE
    // ============================================================================
    // These variables track the current state of our character
    private Vector3 velocity; // Current movement velocity (speed + direction)
    private bool isGrounded; // Is the character touching the ground?
    private bool isDucking; // Is the character currently ducking?
    private int airJumpCount; // How many air jumps have been used?
    private float currentHeight; // Current height of the character (for smooth duck transitions)

    // ============================================================================
    // UNITY LIFECYCLE METHODS
    // ============================================================================
    // These methods are called automatically by Unity at specific times

    /// <summary>
    /// Start is called once when the script is first enabled, before the first frame.
    /// Use this for initialization.
    /// </summary>
    private void Start()
    {
        // Get the CharacterController component attached to this GameObject
        // CharacterController is Unity's built-in component for character movement
        characterController = GetComponent<CharacterController>();

        // Try to get an Animator component (it's optional, so we don't require it)
        animator = GetComponent<Animator>();

        // Initialize the current height to normal standing height
        currentHeight = normalHeight;

        // EDUCATIONAL NOTE: GetComponent<T>() is how you access other components
        // on the same GameObject. It's a fundamental Unity pattern.
    }

    /// <summary>
    /// Update is called once per frame.
    /// Use this for input handling and non-physics calculations.
    /// </summary>
    private void Update()
    {
        // Check if we're on the ground using Unity's CharacterController
        // EDUCATIONAL NOTE: CharacterController.isGrounded checks if the character
        // is standing on something. It's not 100% reliable, so we add a small buffer.
        isGrounded = characterController.isGrounded;

        // Reset air jump count when we land
        if (isGrounded)
        {
            airJumpCount = 0;
        }

        // Handle all input (WASD, Space, Shift, Ctrl, etc.)
        HandleInput();

        // Apply gravity and update vertical velocity
        ApplyGravity();

        // Smoothly transition duck height
        UpdateDuckHeight();

        // Move the character based on calculated velocity
        // EDUCATIONAL NOTE: Time.deltaTime is the time since last frame
        // We multiply by this to make movement frame-rate independent
        characterController.Move(velocity * Time.deltaTime);
    }

    // ============================================================================
    // INPUT HANDLING
    // ============================================================================

    /// <summary>
    /// Handles all player input (keyboard, gamepad, etc.)
    /// </summary>
    private void HandleInput()
    {
        // -----------------------------------------------------------------------
        // MOVEMENT INPUT
        // -----------------------------------------------------------------------
        // Get horizontal input (A/D keys or Left/Right arrows)
        // Returns a value between -1 (left) and 1 (right)
        float horizontal = Input.GetAxisRaw("Horizontal");

        // Get vertical input (W/S keys or Up/Down arrows)
        // Returns a value between -1 (back) and 1 (forward)
        float vertical = Input.GetAxisRaw("Vertical");

        // Create a movement direction vector
        // EDUCATIONAL NOTE: Vector3 represents a point or direction in 3D space (x, y, z)
        Vector3 moveDirection = new Vector3(horizontal, 0, vertical);

        // Normalize to prevent faster diagonal movement
        // EDUCATIONAL NOTE: Normalizing makes the vector length = 1, so diagonal
        // movement (0.7, 0, 0.7) becomes (0.5, 0, 0.5) and has the same speed as
        // forward movement (0, 0, 1)
        if (moveDirection.magnitude > 1)
        {
            moveDirection.Normalize();
        }

        // -----------------------------------------------------------------------
        // DUCK INPUT
        // -----------------------------------------------------------------------
        // Check if duck button is held (Left Ctrl or C key)
        if (Input.GetKey(KeyCode.LeftControl) || Input.GetKey(KeyCode.C))
        {
            isDucking = true;
        }
        else
        {
            // Only stand up if there's room above the character
            if (CanStandUp())
            {
                isDucking = false;
            }
        }

        // -----------------------------------------------------------------------
        // DETERMINE MOVEMENT SPEED
        // -----------------------------------------------------------------------
        float targetSpeed;

        if (isDucking)
        {
            // Move slowly when ducking
            targetSpeed = duckSpeed;
        }
        else if (Input.GetKey(KeyCode.LeftShift))
        {
            // Run when holding Shift
            targetSpeed = runSpeed;
        }
        else
        {
            // Normal walking speed
            targetSpeed = walkSpeed;
        }

        // Calculate target velocity
        Vector3 targetVelocity = moveDirection * targetSpeed;

        // Smoothly accelerate towards target velocity (feels more natural)
        // EDUCATIONAL NOTE: Vector3.Lerp smoothly interpolates between two vectors
        // The third parameter (0 to 1) controls how much to move towards the target
        velocity.x = Mathf.Lerp(velocity.x, targetVelocity.x, accelerationRate * Time.deltaTime);
        velocity.z = Mathf.Lerp(velocity.z, targetVelocity.z, accelerationRate * Time.deltaTime);

        // -----------------------------------------------------------------------
        // ROTATION (face movement direction)
        // -----------------------------------------------------------------------
        if (moveDirection.magnitude > 0.1f)
        {
            // Calculate the rotation needed to face the movement direction
            // EDUCATIONAL NOTE: Quaternions represent rotations in 3D space
            // LookRotation creates a rotation that faces a specific direction
            Quaternion targetRotation = Quaternion.LookRotation(moveDirection);

            // Smoothly rotate towards target rotation
            transform.rotation = Quaternion.Slerp(
                transform.rotation,
                targetRotation,
                rotationSpeed * Time.deltaTime
            );
        }

        // -----------------------------------------------------------------------
        // JUMP INPUT
        // -----------------------------------------------------------------------
        // GetButtonDown returns true only on the frame the button is pressed
        // This prevents holding Space to jump continuously
        if (Input.GetButtonDown("Jump"))
        {
            // Can we jump?
            if (isGrounded || airJumpCount < maxAirJumps)
            {
                // Apply upward velocity for jump
                velocity.y = jumpForce;

                // Track air jumps
                if (!isGrounded)
                {
                    airJumpCount++;
                }
            }
        }
    }

    // ============================================================================
    // PHYSICS & GRAVITY
    // ============================================================================

    /// <summary>
    /// Applies gravity to the character's vertical velocity.
    /// This is what makes the character fall down and controls jump arc.
    /// </summary>
    private void ApplyGravity()
    {
        // EDUCATIONAL NOTE: Physics.gravity is Unity's global gravity setting
        // (default is -9.81 on Y axis, simulating Earth's gravity)

        if (isGrounded && velocity.y < 0)
        {
            // When on ground, keep a small downward force to stay grounded
            // This helps the CharacterController detect ground properly
            velocity.y = -2f;
        }
        else
        {
            // Apply gravity when in the air
            // EDUCATIONAL NOTE: We multiply by gravityMultiplier to make jumping
            // feel more responsive (heavier falling = tighter jump control)
            velocity.y += Physics.gravity.y * gravityMultiplier * Time.deltaTime;
        }
    }

    // ============================================================================
    // DUCKING SYSTEM
    // ============================================================================

    /// <summary>
    /// Smoothly transitions the character height when ducking/standing.
    /// </summary>
    private void UpdateDuckHeight()
    {
        // Determine target height based on ducking state
        float targetHeight = isDucking ? duckHeight : normalHeight;

        // Smoothly interpolate to target height
        currentHeight = Mathf.Lerp(currentHeight, targetHeight, duckTransitionSpeed * Time.deltaTime);

        // Apply height to CharacterController
        // EDUCATIONAL NOTE: CharacterController.height controls the collision capsule size
        characterController.height = currentHeight;

        // Adjust the center of the capsule so it stays grounded
        // When we duck, we want the bottom to stay in place, not the center
        characterController.center = new Vector3(0, currentHeight / 2, 0);
    }

    /// <summary>
    /// Checks if there's enough room above the character to stand up.
    /// This prevents standing up when under low ceilings.
    /// </summary>
    private bool CanStandUp()
    {
        // If already standing, we can "stand up" (no-op)
        if (!isDucking)
        {
            return true;
        }

        // Calculate how much height we'd gain by standing
        float heightDifference = normalHeight - currentHeight;

        // Raycast upward to check for obstacles
        // EDUCATIONAL NOTE: Physics.Raycast shoots an invisible ray and checks what it hits
        // This is perfect for detecting obstacles, line of sight, etc.
        Vector3 rayStart = transform.position + Vector3.up * currentHeight;
        bool hitSomething = Physics.Raycast(
            rayStart,
            Vector3.up,
            heightDifference + 0.1f, // Add small buffer
            LayerMask.GetMask("Default") // Only check default collision layer
        );

        // Can stand if nothing is blocking above
        return !hitSomething;
    }

    // ============================================================================
    // PUBLIC INTERFACE & DEBUG
    // ============================================================================

    /// <summary>
    /// Public property to check if character is currently ducking.
    /// Other scripts can read this to react to ducking state.
    /// </summary>
    public bool IsDucking => isDucking;

    /// <summary>
    /// Public property to get current movement speed.
    /// </summary>
    public float CurrentSpeed => new Vector2(velocity.x, velocity.z).magnitude;

    /// <summary>
    /// Draws debug information in the Scene view (not visible in Game view).
    /// EDUCATIONAL NOTE: This helps visualize what's happening during development.
    /// </summary>
    private void OnDrawGizmosSelected()
    {
        // Only draw gizmos in the editor when this object is selected
        if (!Application.isPlaying) return;

        // Draw velocity vector
        Gizmos.color = Color.green;
        Gizmos.DrawRay(transform.position, velocity);

        // Draw grounding check
        Gizmos.color = isGrounded ? Color.green : Color.red;
        Gizmos.DrawWireSphere(transform.position, 0.3f);
    }
}

// ================================================================================
// EXPERIMENTATION IDEAS FOR LEARNING
// ================================================================================
// Try modifying these values and see what happens:
//
// 1. GRAVITY: Increase gravityMultiplier to 5.0 - how does jumping feel?
// 2. JUMP HEIGHT: Double the jumpForce - can you reach new places?
// 3. SPEED: Set runSpeed to 20.0 - how does fast movement feel?
// 4. AIR CONTROL: Set maxAirJumps to 2 - now you can triple jump!
// 5. ACCELERATION: Set accelerationRate to 1.0 - notice the "slippery" feel?
//
// ADVANCED EXPERIMENTS:
// - Add a wall-run mechanic by detecting walls with Raycasts
// - Implement a stamina system that limits running and jumping
// - Add footstep sounds that change based on movement speed
// - Create different movement profiles (ice physics, low gravity, etc.)
// ================================================================================