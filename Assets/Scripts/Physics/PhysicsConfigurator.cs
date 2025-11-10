using UnityEngine;

/// <summary>
/// PhysicsConfigurator allows you to easily modify Unity's physics settings at runtime
/// or in the editor. This is an educational tool to help you understand how different
/// physics parameters affect gameplay.
///
/// KEY CONCEPTS DEMONSTRATED:
/// - Unity's global physics settings
/// - How gravity affects objects
/// - Physics simulation parameters
/// - Layer-based collision detection
///
/// EDUCATIONAL NOTE: Physics in games is a simulation of real-world physics.
/// Unity uses PhysX, a professional physics engine, to handle collisions, gravity,
/// forces, and realistic object behavior.
/// </summary>
[ExecuteInEditMode] // This allows the script to run in the editor, not just during gameplay
public class PhysicsConfigurator : MonoBehaviour
{
    // ============================================================================
    // GRAVITY SETTINGS
    // ============================================================================
    [Header("Gravity Configuration")]
    [Tooltip("Gravity force applied to all objects (Earth gravity is -9.81 m/s²)")]
    [SerializeField] private Vector3 gravity = new Vector3(0, -9.81f, 0);

    [Space(10)]
    [Header("Preset Gravity Profiles")]
    [Tooltip("Apply a preset gravity configuration")]
    [SerializeField] private GravityPreset gravityPreset = GravityPreset.Earth;

    // ============================================================================
    // PHYSICS SIMULATION SETTINGS
    // ============================================================================
    [Header("Simulation Quality")]
    [Tooltip("How many times per second physics updates (higher = more accurate but slower)")]
    [SerializeField] private float fixedTimestep = 0.02f; // 50 times per second

    [Tooltip("Sleep threshold - objects moving slower than this will 'sleep' to save performance")]
    [SerializeField] private float sleepThreshold = 0.005f;

    [Tooltip("Default contact offset - how close objects can get before collision detection")]
    [SerializeField] private float defaultContactOffset = 0.01f;

    [Tooltip("How many iterations the physics solver uses (higher = more stable but slower)")]
    [SerializeField] private int defaultSolverIterations = 6;

    [Tooltip("Velocity iterations for the physics solver")]
    [SerializeField] private int defaultSolverVelocityIterations = 1;

    // ============================================================================
    // COLLISION & BOUNCE SETTINGS
    // ============================================================================
    [Header("Collision Settings")]
    [Tooltip("Minimum relative velocity for a collision to bounce (lower = bouncier)")]
    [SerializeField] private float bounceThreshold = 2.0f;

    [Tooltip("Maximum velocity an object can gain when depenetrating from another object")]
    [SerializeField] private float defaultMaxDepenetrationVelocity = 10.0f;

    [Tooltip("Queries (raycasts) hit backfaces of meshes?")]
    [SerializeField] private bool queriesHitBackfaces = false;

    [Tooltip("Queries hit trigger colliders?")]
    [SerializeField] private bool queriesHitTriggers = true;

    // ============================================================================
    // PERFORMANCE SETTINGS
    // ============================================================================
    [Header("Performance")]
    [Tooltip("Enable physics auto-simulation? (false = you control when physics updates)")]
    [SerializeField] private bool autoSimulation = true;

    [Tooltip("Automatically sync transforms? (May impact performance)")]
    [SerializeField] private bool autoSyncTransforms = false;

    // ============================================================================
    // DEBUG & TESTING
    // ============================================================================
    [Header("Debug Options")]
    [Tooltip("Show physics debug information in console?")]
    [SerializeField] private bool showDebugInfo = false;

    [Tooltip("Apply settings on Start? (useful for testing different configurations)")]
    [SerializeField] private bool applyOnStart = true;

    // Enum for gravity presets
    public enum GravityPreset
    {
        Custom,
        Earth,      // -9.81 m/s²
        Moon,       // -1.62 m/s² (about 1/6 of Earth)
        Mars,       // -3.71 m/s² (about 1/3 of Earth)
        Jupiter,    // -24.79 m/s² (about 2.5x Earth)
        Zero,       // 0 m/s² (space!)
        Low,        // -4.0 m/s² (floaty platformer feel)
        High        // -15.0 m/s² (heavy, responsive feel)
    }

    // ============================================================================
    // UNITY LIFECYCLE
    // ============================================================================

    private void Start()
    {
        if (applyOnStart)
        {
            ApplyPhysicsSettings();
        }
    }

    // Called when values change in the Inspector
    private void OnValidate()
    {
        // Apply gravity preset if changed
        if (gravityPreset != GravityPreset.Custom)
        {
            gravity = GetGravityFromPreset(gravityPreset);
        }

        // Apply settings immediately in editor
        if (Application.isPlaying && applyOnStart)
        {
            ApplyPhysicsSettings();
        }
    }

    // ============================================================================
    // PHYSICS CONFIGURATION
    // ============================================================================

    /// <summary>
    /// Applies all configured physics settings to Unity's physics system.
    /// EDUCATIONAL NOTE: These settings affect ALL physics objects in your game!
    /// </summary>
    public void ApplyPhysicsSettings()
    {
        // GRAVITY
        // EDUCATIONAL NOTE: Gravity is what makes objects fall. In real life,
        // Earth's gravity accelerates objects at 9.81 meters per second squared.
        Physics.gravity = gravity;

        // SIMULATION QUALITY
        // EDUCATIONAL NOTE: FixedTimestep controls how often physics updates.
        // 0.02 = 50 times per second (common for games)
        // Lower values = more accurate but more CPU intensive
        Time.fixedDeltaTime = fixedTimestep;

        // SOLVER ITERATIONS
        // EDUCATIONAL NOTE: The physics solver calculates collisions and forces.
        // More iterations = more accurate but slower. Good balance is 6-8.
        Physics.defaultSolverIterations = defaultSolverIterations;
        Physics.defaultSolverVelocityIterations = defaultSolverVelocityIterations;

        // COLLISION SETTINGS
        // EDUCATIONAL NOTE: These control how objects interact when they touch
        Physics.bounceThreshold = bounceThreshold;
        Physics.defaultMaxDepenetrationVelocity = defaultMaxDepenetrationVelocity;
        Physics.defaultContactOffset = defaultContactOffset;

        // PERFORMANCE SETTINGS
        Physics.autoSimulation = autoSimulation;
        Physics.autoSyncTransforms = autoSyncTransforms;

        // QUERY SETTINGS
        // EDUCATIONAL NOTE: Queries include Raycasts, SphereCasts, etc.
        Physics.queriesHitBackfaces = queriesHitBackfaces;
        Physics.queriesHitTriggers = queriesHitTriggers;

        // SLEEP SETTINGS
        // EDUCATIONAL NOTE: "Sleeping" objects don't update physics to save performance
        // Objects wake up when something interacts with them
        Physics.sleepThreshold = sleepThreshold;

        if (showDebugInfo)
        {
            LogCurrentSettings();
        }
    }

    /// <summary>
    /// Gets gravity value from preset.
    /// </summary>
    private Vector3 GetGravityFromPreset(GravityPreset preset)
    {
        switch (preset)
        {
            case GravityPreset.Earth:
                return new Vector3(0, -9.81f, 0);
            case GravityPreset.Moon:
                return new Vector3(0, -1.62f, 0);
            case GravityPreset.Mars:
                return new Vector3(0, -3.71f, 0);
            case GravityPreset.Jupiter:
                return new Vector3(0, -24.79f, 0);
            case GravityPreset.Zero:
                return Vector3.zero;
            case GravityPreset.Low:
                return new Vector3(0, -4.0f, 0);
            case GravityPreset.High:
                return new Vector3(0, -15.0f, 0);
            default:
                return gravity; // Custom
        }
    }

    /// <summary>
    /// Resets all physics settings to Unity defaults.
    /// </summary>
    public void ResetToDefaults()
    {
        gravity = new Vector3(0, -9.81f, 0);
        fixedTimestep = 0.02f;
        sleepThreshold = 0.005f;
        defaultContactOffset = 0.01f;
        defaultSolverIterations = 6;
        defaultSolverVelocityIterations = 1;
        bounceThreshold = 2.0f;
        defaultMaxDepenetrationVelocity = 10.0f;
        queriesHitBackfaces = false;
        queriesHitTriggers = true;
        autoSimulation = true;
        autoSyncTransforms = false;

        ApplyPhysicsSettings();
        Debug.Log("PhysicsConfigurator: Reset to Unity defaults");
    }

    // ============================================================================
    // DEBUG & INFO
    // ============================================================================

    /// <summary>
    /// Logs current physics settings to console.
    /// </summary>
    private void LogCurrentSettings()
    {
        Debug.Log("=== PHYSICS SETTINGS ===");
        Debug.Log($"Gravity: {Physics.gravity}");
        Debug.Log($"Fixed Timestep: {Time.fixedDeltaTime}s ({1f / Time.fixedDeltaTime} updates/second)");
        Debug.Log($"Solver Iterations: {Physics.defaultSolverIterations}");
        Debug.Log($"Bounce Threshold: {Physics.bounceThreshold}");
        Debug.Log($"Sleep Threshold: {Physics.sleepThreshold}");
        Debug.Log($"Auto Simulation: {Physics.autoSimulation}");
        Debug.Log("========================");
    }

    /// <summary>
    /// Public method to log settings (can be called from other scripts or Unity Events).
    /// </summary>
    public void PrintPhysicsInfo()
    {
        LogCurrentSettings();
    }

    // ============================================================================
    // PUBLIC HELPER METHODS
    // ============================================================================

    /// <summary>
    /// Quickly set gravity to a specific preset.
    /// </summary>
    public void SetGravityPreset(GravityPreset preset)
    {
        gravityPreset = preset;
        gravity = GetGravityFromPreset(preset);
        Physics.gravity = gravity;

        Debug.Log($"Gravity set to {preset}: {gravity}");
    }

    /// <summary>
    /// Set custom gravity value.
    /// </summary>
    public void SetCustomGravity(Vector3 newGravity)
    {
        gravityPreset = GravityPreset.Custom;
        gravity = newGravity;
        Physics.gravity = gravity;
    }

    /// <summary>
    /// Set physics update rate (updates per second).
    /// </summary>
    public void SetPhysicsUpdateRate(int updatesPerSecond)
    {
        fixedTimestep = 1f / updatesPerSecond;
        Time.fixedDeltaTime = fixedTimestep;

        Debug.Log($"Physics update rate set to {updatesPerSecond} updates/second");
    }
}

// ================================================================================
// EDUCATIONAL GUIDE: UNDERSTANDING PHYSICS SETTINGS
// ================================================================================
//
// GRAVITY:
// --------
// Gravity is a constant downward acceleration applied to all Rigidbodies.
// - Earth: -9.81 m/s² (realistic)
// - Moon: -1.62 m/s² (slow, floaty jumps - great for platformers!)
// - High: -15.0 m/s² (fast, responsive jumps - great for action games)
//
// WHY IT MATTERS: Gravity heavily affects how your game "feels". Lower gravity
// makes jumping feel floaty and gives players more air control. Higher gravity
// makes jumping snappy and precise.
//
// FIXED TIMESTEP:
// ---------------
// How often physics calculations happen. Default is 0.02s (50 times/second).
// - Lower (0.01 = 100/sec): More accurate, smoother, but uses more CPU
// - Higher (0.04 = 25/sec): Less accurate, jittery, but better performance
//
// WHY IT MATTERS: Affects both physics accuracy and performance. If physics
// objects jitter or pass through each other, try lowering this value.
//
// SOLVER ITERATIONS:
// ------------------
// How many times Unity calculates physics per update. Default is 6.
// - Higher (10+): More stable collisions, heavy objects don't sink into ground
// - Lower (3-4): Faster but less stable, objects may jitter or penetrate
//
// WHY IT MATTERS: Important for stacked objects, ragdolls, and precise collisions.
//
// BOUNCE THRESHOLD:
// -----------------
// Minimum velocity needed for objects to bounce. Default is 2.0 m/s.
// - Lower (0.5): Objects bounce more easily (bouncy world!)
// - Higher (5.0): Objects only bounce when moving fast (more realistic)
//
// WHY IT MATTERS: Controls how "bouncy" your game world feels.
//
// SLEEP THRESHOLD:
// ----------------
// Objects moving slower than this "sleep" to save performance. Default is 0.005.
// - Lower: Objects sleep more easily (better performance, but may seem "sticky")
// - Higher: Objects stay awake longer (more realistic but worse performance)
//
// WHY IT MATTERS: Important for performance in games with many physics objects.
//
// QUERIES HIT TRIGGERS:
// ---------------------
// Should raycasts detect trigger colliders? Default is true.
// - True: Raycasts detect triggers (useful for checking if player is in a zone)
// - False: Raycasts ignore triggers (useful for line-of-sight checks)
//
// WHY IT MATTERS: Depends on how you use raycasts in your game.
//
// ================================================================================
// EXPERIMENTATION IDEAS:
// ================================================================================
// 1. Moon Gravity: Set to Moon preset and feel the floaty jumps!
// 2. Slow Motion: Set fixedTimestep to 0.04 and gravity to -5.0
// 3. Zero Gravity: Set to Zero preset - now you're in space!
// 4. Super Bouncy: Set bounceThreshold to 0.1 and watch everything bounce
// 5. High Precision: Set solverIterations to 15 for super stable physics
//
// Try creating different "physics profiles" for different gameplay scenarios:
// - Combat: High gravity, high solver iterations for precise hits
// - Platforming: Low gravity, normal iterations for floaty jumps
// - Puzzle: Normal gravity, high iterations for stable stacking
// ================================================================================