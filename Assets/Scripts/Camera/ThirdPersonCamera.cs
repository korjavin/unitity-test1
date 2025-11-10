using UnityEngine;

/// <summary>
/// ThirdPersonCamera provides a smooth, orbiting camera that follows the player character.
/// This is essential for Zelda-like games where you need to see your character and surroundings.
///
/// KEY CONCEPTS DEMONSTRATED:
/// - Camera follow mechanics
/// - Input handling for camera control
/// - Smooth interpolation (Lerp/Slerp)
/// - Collision detection (preventing camera from going through walls)
/// - Transform manipulation
///
/// EDUCATIONAL NOTE: In Unity, the Camera is what the player sees through.
/// This script controls the Camera's position and rotation to create a 3rd person view.
/// </summary>
public class ThirdPersonCamera : MonoBehaviour
{
    // ============================================================================
    // TARGET & FOLLOW SETTINGS
    // ============================================================================
    [Header("Target Settings")]
    [Tooltip("The Transform to follow (usually your player character)")]
    [SerializeField] private Transform target;

    [Tooltip("Offset from the target's position (higher Y = camera looks down more)")]
    [SerializeField] private Vector3 targetOffset = new Vector3(0, 1.5f, 0);

    // ============================================================================
    // CAMERA POSITIONING
    // ============================================================================
    [Header("Camera Position")]
    [Tooltip("How far back from the target the camera sits")]
    [SerializeField] private float distance = 5.0f;

    [Tooltip("Minimum distance the camera can be from target")]
    [SerializeField] private float minDistance = 2.0f;

    [Tooltip("Maximum distance the camera can be from target")]
    [SerializeField] private float maxDistance = 10.0f;

    [Tooltip("Camera height offset from target")]
    [SerializeField] private float height = 2.0f;

    // ============================================================================
    // CAMERA ROTATION
    // ============================================================================
    [Header("Camera Rotation")]
    [Tooltip("How fast the camera rotates horizontally (left/right)")]
    [SerializeField] private float rotationSpeed = 5.0f;

    [Tooltip("How fast the camera rotates vertically (up/down)")]
    [SerializeField] private float verticalRotationSpeed = 3.0f;

    [Tooltip("Minimum vertical angle (looking up)")]
    [SerializeField] private float minVerticalAngle = -20f;

    [Tooltip("Maximum vertical angle (looking down)")]
    [SerializeField] private float maxVerticalAngle = 60f;

    // ============================================================================
    // SMOOTHING & FEEL
    // ============================================================================
    [Header("Smoothing")]
    [Tooltip("How smoothly the camera follows the target (higher = smoother but more lag)")]
    [SerializeField] private float followSmoothness = 10.0f;

    [Tooltip("How smoothly the camera rotates (higher = snappier)")]
    [SerializeField] private float rotationSmoothness = 8.0f;

    // ============================================================================
    // COLLISION DETECTION
    // ============================================================================
    [Header("Collision")]
    [Tooltip("Detect and avoid walls? (prevents camera from going through objects)")]
    [SerializeField] private bool detectCollisions = true;

    [Tooltip("Layers the camera should detect as obstacles")]
    [SerializeField] private LayerMask collisionLayers = ~0; // Default: all layers

    [Tooltip("Radius for collision detection sphere")]
    [SerializeField] private float collisionRadius = 0.3f;

    // ============================================================================
    // INPUT CONFIGURATION
    // ============================================================================
    [Header("Input")]
    [Tooltip("Mouse sensitivity for camera rotation")]
    [SerializeField] private float mouseSensitivity = 3.0f;

    [Tooltip("Invert Y-axis? (some players prefer inverted controls)")]
    [SerializeField] private bool invertY = false;

    [Tooltip("Allow camera zoom with mouse scroll wheel?")]
    [SerializeField] private bool allowZoom = true;

    [Tooltip("How fast to zoom in/out with scroll wheel")]
    [SerializeField] private float zoomSpeed = 2.0f;

    // ============================================================================
    // PRIVATE STATE
    // ============================================================================
    // These track the current camera state
    private float currentDistance; // Current distance from target
    private float currentHorizontalAngle; // Current horizontal rotation (Y-axis)
    private float currentVerticalAngle; // Current vertical rotation (X-axis)
    private Vector3 currentVelocity; // Used for smooth damping

    // ============================================================================
    // UNITY LIFECYCLE METHODS
    // ============================================================================

    /// <summary>
    /// Called when the script is first initialized.
    /// </summary>
    private void Start()
    {
        // Initialize distance to starting value
        currentDistance = distance;

        // If no target is set, try to find an object tagged "Player"
        // EDUCATIONAL NOTE: Tags are a way to identify GameObjects in Unity
        if (target == null)
        {
            GameObject playerObject = GameObject.FindGameObjectWithTag("Player");
            if (playerObject != null)
            {
                target = playerObject.transform;
            }
            else
            {
                Debug.LogWarning("ThirdPersonCamera: No target assigned and no object with 'Player' tag found!");
            }
        }

        // Initialize angles based on current camera rotation
        // EDUCATIONAL NOTE: eulerAngles converts a Quaternion rotation to degrees
        Vector3 angles = transform.eulerAngles;
        currentHorizontalAngle = angles.y;
        currentVerticalAngle = angles.x;

        // Hide and lock the mouse cursor for better camera control
        // EDUCATIONAL NOTE: This makes it feel like a proper 3D game
        // Press Escape to unlock the cursor (handled by Unity by default)
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    /// <summary>
    /// LateUpdate is called after all Update methods.
    /// This ensures the camera moves AFTER the player has moved,
    /// preventing jittery camera movement.
    ///
    /// EDUCATIONAL NOTE: Unity's execution order matters!
    /// - Update() = handle input, move characters
    /// - LateUpdate() = update camera (after everything else moved)
    /// </summary>
    private void LateUpdate()
    {
        // Don't do anything if we have no target
        if (target == null) return;

        // Handle camera rotation input
        HandleCameraInput();

        // Handle zoom input
        if (allowZoom)
        {
            HandleZoomInput();
        }

        // Calculate and apply camera position
        UpdateCameraPosition();
    }

    // ============================================================================
    // INPUT HANDLING
    // ============================================================================

    /// <summary>
    /// Handles mouse input for rotating the camera around the target.
    /// </summary>
    private void HandleCameraInput()
    {
        // Get mouse movement
        // EDUCATIONAL NOTE: "Mouse X" and "Mouse Y" are Unity's built-in input axes
        // that return how much the mouse moved this frame
        float mouseX = Input.GetAxis("Mouse X");
        float mouseY = Input.GetAxis("Mouse Y");

        // Only rotate if we actually have input
        if (Mathf.Abs(mouseX) > 0.01f || Mathf.Abs(mouseY) > 0.01f)
        {
            // Horizontal rotation (looking left/right)
            currentHorizontalAngle += mouseX * mouseSensitivity * rotationSpeed;

            // Vertical rotation (looking up/down)
            float verticalChange = mouseY * mouseSensitivity * verticalRotationSpeed;
            if (invertY)
            {
                verticalChange = -verticalChange;
            }
            currentVerticalAngle -= verticalChange;

            // Clamp vertical angle to prevent camera from flipping upside down
            // EDUCATIONAL NOTE: Mathf.Clamp restricts a value between min and max
            currentVerticalAngle = Mathf.Clamp(currentVerticalAngle, minVerticalAngle, maxVerticalAngle);
        }

        // Allow player to unlock cursor with Escape key
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }

        // Relock cursor when clicking (for gameplay convenience)
        if (Input.GetMouseButtonDown(0) || Input.GetMouseButtonDown(1))
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
    }

    /// <summary>
    /// Handles mouse scroll wheel input for zooming the camera in/out.
    /// </summary>
    private void HandleZoomInput()
    {
        // Get scroll wheel input
        // EDUCATIONAL NOTE: Mouse ScrollWheel returns positive when scrolling up,
        // negative when scrolling down
        float scroll = Input.GetAxis("Mouse ScrollWheel");

        if (Mathf.Abs(scroll) > 0.01f)
        {
            // Adjust distance based on scroll
            currentDistance -= scroll * zoomSpeed;

            // Clamp to min/max distance
            currentDistance = Mathf.Clamp(currentDistance, minDistance, maxDistance);
        }
    }

    // ============================================================================
    // CAMERA POSITIONING
    // ============================================================================

    /// <summary>
    /// Calculates and applies the camera's position and rotation based on current settings.
    /// </summary>
    private void UpdateCameraPosition()
    {
        // Calculate the target position we're following
        // EDUCATIONAL NOTE: We add an offset so the camera focuses on the character's
        // upper body/head, not their feet
        Vector3 targetPosition = target.position + targetOffset;

        // Calculate desired rotation based on input angles
        // EDUCATIONAL NOTE: Quaternion.Euler creates a rotation from angles in degrees
        Quaternion rotation = Quaternion.Euler(currentVerticalAngle, currentHorizontalAngle, 0);

        // Calculate desired camera position behind and above the target
        // EDUCATIONAL NOTE: Quaternion * Vector3 rotates the vector
        // rotation * -Vector3.forward gives us the "back" direction
        Vector3 desiredPosition = targetPosition + rotation * new Vector3(0, height, -currentDistance);

        // Check for collisions between target and desired camera position
        float finalDistance = currentDistance;
        if (detectCollisions)
        {
            finalDistance = CheckCameraCollision(targetPosition, desiredPosition);
        }

        // Recalculate position with collision-adjusted distance
        Vector3 finalPosition = targetPosition + rotation * new Vector3(0, height, -finalDistance);

        // Smoothly move camera to final position
        // EDUCATIONAL NOTE: Vector3.Lerp smoothly interpolates between two positions
        // This creates smooth camera movement instead of instant "snapping"
        transform.position = Vector3.Lerp(
            transform.position,
            finalPosition,
            followSmoothness * Time.deltaTime
        );

        // Smoothly rotate camera to look at target
        // EDUCATIONAL NOTE: Quaternion.LookRotation creates a rotation that faces a direction
        Vector3 lookDirection = targetPosition - transform.position;
        Quaternion targetRotation = Quaternion.LookRotation(lookDirection);

        transform.rotation = Quaternion.Slerp(
            transform.rotation,
            targetRotation,
            rotationSmoothness * Time.deltaTime
        );
    }

    /// <summary>
    /// Checks for obstacles between the target and camera, and adjusts distance accordingly.
    /// This prevents the camera from clipping through walls.
    /// </summary>
    /// <param name="from">Start position (target)</param>
    /// <param name="to">End position (desired camera position)</param>
    /// <returns>The safe distance that avoids collisions</returns>
    private float CheckCameraCollision(Vector3 from, Vector3 to)
    {
        // Calculate direction and distance
        Vector3 direction = to - from;
        float desiredDistance = direction.magnitude;

        // Perform a spherecast from target towards camera
        // EDUCATIONAL NOTE: SphereCast is like Raycast but uses a sphere instead of a line
        // This prevents the camera from getting too close to walls
        RaycastHit hit;
        if (Physics.SphereCast(
            from,
            collisionRadius,
            direction.normalized,
            out hit,
            desiredDistance,
            collisionLayers))
        {
            // Hit something! Move camera closer to avoid it
            // Subtract collision radius to keep some space from the wall
            return hit.distance - collisionRadius;
        }

        // No collision, use desired distance
        return currentDistance;
    }

    // ============================================================================
    // PUBLIC METHODS
    // ============================================================================

    /// <summary>
    /// Sets a new target for the camera to follow.
    /// Useful if the player character changes or for cutscenes.
    /// </summary>
    public void SetTarget(Transform newTarget)
    {
        target = newTarget;
    }

    /// <summary>
    /// Instantly snaps the camera to a specific angle (no smoothing).
    /// Useful for cutscenes or teleporting the player.
    /// </summary>
    public void SetCameraAngle(float horizontal, float vertical)
    {
        currentHorizontalAngle = horizontal;
        currentVerticalAngle = Mathf.Clamp(vertical, minVerticalAngle, maxVerticalAngle);
    }

    /// <summary>
    /// Resets the camera to default position behind the target.
    /// </summary>
    public void ResetCamera()
    {
        currentDistance = distance;
        currentVerticalAngle = 0;
        // Keep horizontal angle so we don't disorient the player
    }

    // ============================================================================
    // DEBUG & VISUALIZATION
    // ============================================================================

    /// <summary>
    /// Draws debug visualization in the Scene view.
    /// </summary>
    private void OnDrawGizmosSelected()
    {
        if (target == null) return;

        // Draw target position
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(target.position + targetOffset, 0.3f);

        // Draw camera collision sphere
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, collisionRadius);

        // Draw line from camera to target
        Gizmos.color = Color.cyan;
        Gizmos.DrawLine(transform.position, target.position + targetOffset);
    }
}

// ================================================================================
// EXPERIMENTATION IDEAS FOR LEARNING
// ================================================================================
// Try modifying these values and see what happens:
//
// 1. DISTANCE: Set distance to 15.0 - see how a far camera changes gameplay feel
// 2. HEIGHT: Set height to 5.0 - creates a more "bird's eye" view
// 3. SMOOTHNESS: Set followSmoothness to 1.0 - camera becomes very "laggy"
// 4. ROTATION: Set rotationSpeed to 20.0 - super sensitive camera controls
// 5. COLLISION: Disable detectCollisions - camera will clip through walls!
//
// ADVANCED EXPERIMENTS:
// - Add camera shake when the player lands from a jump
// - Implement different camera modes (first person, top-down, etc.)
// - Add "look at" functionality to focus on specific objects or enemies
// - Create a shoulder-swap feature (camera switches left/right shoulder)
// - Implement auto-rotation to face the direction the player is moving
// ================================================================================