# Physics and Motion Guide ⚛️

Learn how physics works in Unity and how to create realistic (or unrealistic!) movement and interactions.

## Table of Contents

1. [Introduction to Game Physics](#introduction-to-game-physics)
2. [Understanding Gravity](#understanding-gravity)
3. [Forces and Movement](#forces-and-movement)
4. [Collision Detection](#collision-detection)
5. [Character Movement Physics](#character-movement-physics)
6. [Tweaking Physics for Feel](#tweaking-physics-for-feel)
7. [Advanced Physics Concepts](#advanced-physics-concepts)
8. [Physics Performance](#physics-performance)

---

## Introduction to Game Physics

### What is Game Physics?

Game physics is a **simulation** of real-world physics. It's not 100% accurate (that would be too slow!), but it's good enough to feel realistic or fun.

Unity uses **PhysX**, a professional physics engine that powers many AAA games.

### Physics vs. Kinematic Movement

**Physics-based** (uses Rigidbody):
- Objects affected by gravity and forces
- Collisions automatically resolved
- Examples: balls, vehicles, ragdolls
- More realistic but less predictable

**Kinematic** (uses Character Controller or manual Transform):
- You control movement directly
- No automatic physics forces
- Examples: player characters, moving platforms
- More control but need to handle collisions yourself

**This project uses Character Controller (kinematic) for precise control!**

---

## Understanding Gravity

### What is Gravity?

Gravity is a constant downward **acceleration** (not force!).

In real life:
- Earth: **-9.81 m/s²** (meters per second squared)
- Means every second, objects fall 9.81 m/s faster

In Unity:
- Default: **-9.81** on Y-axis
- Configurable in `ProjectSettings/DynamicsManager.asset`
- Or use PhysicsConfigurator script in this project!

### How Gravity Affects Objects

```csharp
// Every physics update:
velocity.y += gravity.y * Time.deltaTime;

// Over time:
// Frame 1: velocity.y = 0
// Frame 2: velocity.y = -9.81 * 0.02 = -0.196
// Frame 3: velocity.y = -0.392
// Frame 4: velocity.y = -0.588
// ... object accelerates downward!
```

### Gravity Experiments

Try these in PhysicsConfigurator or DynamicsManager:

**Moon Gravity** (Y: -1.62):
- Slow, floaty jumps
- Great for platformers!
- Good for: exploring, puzzle games

**Low Gravity** (Y: -4.0):
- Moderate floating
- Player has more air control
- Good for: aerial combat, parkour

**Earth Gravity** (Y: -9.81):
- Realistic feel
- Standard for most games
- Good for: simulation, realistic games

**High Gravity** (Y: -15.0):
- Fast, snappy jumps
- Very responsive
- Good for: action games, speedrunning

**Zero Gravity** (Y: 0):
- No falling!
- Space environment
- Good for: space games, creative modes

### Gravity Multipliers

Instead of changing global gravity, you can multiply it per-object:

```csharp
// In PlayerController.cs, ApplyGravity():
velocity.y += Physics.gravity.y * gravityMultiplier * Time.deltaTime;
```

- `gravityMultiplier = 1.0` → Normal gravity
- `gravityMultiplier = 2.0` → Falls twice as fast
- `gravityMultiplier = 0.5` → Falls half as fast

**Why use multipliers?**
- Gives tighter control over jump arcs
- Different characters can have different gravity
- Can change during gameplay (power-ups!)

---

## Forces and Movement

### Types of Forces

**Constant Force** (AddForce):
```csharp
rb.AddForce(Vector3.forward * 10);
// Applies force every frame, accelerates gradually
```

**Impulse** (AddForce with ForceMode.Impulse):
```csharp
rb.AddForce(Vector3.up * 10, ForceMode.Impulse);
// Instant burst of force (good for jumps, explosions)
```

**Velocity Change** (direct):
```csharp
rb.velocity = new Vector3(5, 0, 0);
// Sets velocity directly (instant speed change)
```

### Understanding Velocity vs Force

**Velocity** = Current speed and direction
```csharp
Vector3 velocity = new Vector3(5, 0, 0);  // Moving right at 5 m/s
```

**Force** = Push that changes velocity
```csharp
rb.AddForce(Vector3.right * 10);  // Push right, will accelerate
```

**Analogy**:
- Velocity = how fast your car is going
- Force = pressing the gas pedal
- Gravity = hill pulling you down

### Acceleration

Acceleration = Change in velocity over time

```csharp
// Manual acceleration example:
Vector3 targetVelocity = moveDirection * speed;
velocity = Vector3.Lerp(velocity, targetVelocity, acceleration * Time.deltaTime);
```

- High acceleration = instant speed changes (arcade feel)
- Low acceleration = gradual speed changes (realistic feel)

Try in PlayerController.cs:
```csharp
[SerializeField] private float accelerationRate = 10.0f;  // Change this!
```

---

## Collision Detection

### Collider Types

Unity provides several collider shapes:

**Primitive Colliders** (Fast):
- Box Collider - Cube shape
- Sphere Collider - Ball shape
- Capsule Collider - Pill shape (best for characters!)

**Complex Colliders** (Slower):
- Mesh Collider - Matches exact 3D model shape
- Terrain Collider - For terrain objects

**Rule of thumb**: Use primitive colliders whenever possible!

### Collision Layers

Layers control what collides with what:

1. Go to `Edit` → `Project Settings` → `Tags and Layers`
2. Add layers: "Player", "Enemy", "Ground", etc.
3. Assign layers to GameObjects
4. Configure collision matrix: `Edit` → `Project Settings` → `Physics`

Example:
- "Player" layer collides with "Ground" and "Enemy"
- "Player" layer doesn't collide with "PlayerProjectile"

### Collision Methods

**Physical Collision** (solid objects):

```csharp
void OnCollisionEnter(Collision collision)
{
    // Called when collision starts
    Debug.Log("Hit: " + collision.gameObject.name);

    // Get collision details
    ContactPoint contact = collision.contacts[0];
    Vector3 hitPoint = contact.point;
    Vector3 hitNormal = contact.normal;
}

void OnCollisionStay(Collision collision)
{
    // Called every frame while colliding
}

void OnCollisionExit(Collision collision)
{
    // Called when collision ends
}
```

**Trigger Collision** (pass-through zones):

```csharp
void OnTriggerEnter(Collider other)
{
    // Called when entering trigger
    if (other.CompareTag("Coin"))
    {
        CollectCoin();
    }
}

void OnTriggerStay(Collider other)
{
    // Called every frame while in trigger
}

void OnTriggerExit(Collider other)
{
    // Called when exiting trigger
}
```

### Collision Settings

**Bounciness** (Physics Material):
1. Create: `Assets` → `Create` → `Physics Material`
2. Set properties:
   - Bounciness: 0 = no bounce, 1 = perfect bounce
   - Friction: 0 = ice, 1 = sticky
3. Apply to Collider's "Material" field

**Collision Detection Modes** (Rigidbody):
- Discrete: Fast, may miss fast-moving collisions
- Continuous: Slower, catches all collisions (use for bullets)
- Continuous Dynamic: Best quality, most expensive

---

## Character Movement Physics

### Character Controller vs Rigidbody

**Character Controller** (used in this project):
- No physics forces affect it (except your code)
- Has built-in ground detection
- Collisions handled automatically
- Best for: player characters, NPCs

**Rigidbody Character**:
- Affected by physics
- More realistic but harder to control
- Can be pushed by other objects
- Best for: vehicles, objects player controls

### Ground Detection

Character Controller provides `isGrounded`:

```csharp
bool isGrounded = characterController.isGrounded;
```

For Rigidbody, use raycasting:

```csharp
bool IsGrounded()
{
    float rayLength = 0.1f;
    Ray ray = new Ray(transform.position, Vector3.down);
    return Physics.Raycast(ray, rayLength);
}
```

### Jump Physics

**Simple Jump** (instant upward velocity):
```csharp
if (Input.GetButtonDown("Jump") && isGrounded)
{
    velocity.y = jumpForce;  // e.g., 8.0 m/s upward
}
```

**Variable Jump Height** (hold for higher jumps):
```csharp
if (Input.GetButton("Jump") && velocity.y > 0)
{
    velocity.y += extraJumpForce * Time.deltaTime;
}
```

**Double Jump**:
```csharp
private int airJumpCount = 0;
private int maxAirJumps = 1;  // 1 = double jump, 2 = triple jump

if (Input.GetButtonDown("Jump"))
{
    if (isGrounded || airJumpCount < maxAirJumps)
    {
        velocity.y = jumpForce;
        if (!isGrounded) airJumpCount++;
    }
}

if (isGrounded) airJumpCount = 0;  // Reset on landing
```

### Air Control

How much can player change direction mid-air?

```csharp
// Full air control (arcade feel)
velocity.x = input.x * speed;
velocity.z = input.z * speed;

// Limited air control (realistic)
Vector3 airMove = new Vector3(input.x, 0, input.z) * airControlMultiplier;
velocity += airMove * Time.deltaTime;

// No air control (momentum-based)
// Don't modify velocity.x or velocity.z in air!
```

### Slope Handling

Character Controller can climb slopes:

```csharp
characterController.slopeLimit = 45f;  // Max slope angle (degrees)
characterController.stepOffset = 0.3f;  // Max step height (meters)
```

For smooth slope movement:

```csharp
if (isGrounded && velocity.y < 0)
{
    velocity.y = -2f;  // Small downward force to stay grounded on slopes
}
```

---

## Tweaking Physics for Feel

### The "Game Feel" Concept

Physics in games should feel **fun**, not necessarily realistic!

**Responsive Feel** (tight, snappy):
- High gravity
- High acceleration
- Instant direction changes
- Examples: Super Meat Boy, Celeste

**Floaty Feel** (loose, airy):
- Low gravity
- Low acceleration
- Gradual direction changes
- Examples: Super Mario 64, Journey

**Realistic Feel** (heavy, momentum-based):
- Earth gravity
- Medium acceleration
- Momentum preserved
- Examples: GTA, Red Dead Redemption

### Jump Feel Tuning

**For tight platforming:**
```csharp
jumpForce = 8.0f;
gravityMultiplier = 2.5f;  // Falls fast = responsive
```

**For exploration:**
```csharp
jumpForce = 6.0f;
gravityMultiplier = 1.5f;  // Gentle falling = relaxed
```

**For floaty platforming:**
```csharp
jumpForce = 10.0f;
gravityMultiplier = 1.0f;  // Lots of air time
```

### Movement Feel Tuning

**For precise control:**
```csharp
accelerationRate = 15.0f;  // Instant speed changes
rotationSpeed = 20.0f;     // Quick turning
```

**For momentum-based:**
```csharp
accelerationRate = 3.0f;   // Gradual acceleration
rotationSpeed = 5.0f;      // Slow turning (like a car)
```

### "Coyote Time" (forgiveness mechanics)

Let player jump shortly after leaving a platform:

```csharp
private float coyoteTime = 0.1f;  // 0.1 seconds of grace period
private float lastGroundedTime;

void Update()
{
    if (isGrounded)
    {
        lastGroundedTime = Time.time;
    }

    if (Input.GetButtonDown("Jump"))
    {
        if (Time.time - lastGroundedTime < coyoteTime)
        {
            Jump();  // Allow jump!
        }
    }
}
```

### "Jump Buffering" (input forgiveness)

Remember jump input if pressed slightly before landing:

```csharp
private float jumpBufferTime = 0.1f;
private float jumpPressedTime = -1f;

void Update()
{
    if (Input.GetButtonDown("Jump"))
    {
        jumpPressedTime = Time.time;
    }

    if (isGrounded && Time.time - jumpPressedTime < jumpBufferTime)
    {
        Jump();
        jumpPressedTime = -1f;  // Consume the buffered input
    }
}
```

---

## Advanced Physics Concepts

### Velocity Clamping

Prevent objects from moving too fast:

```csharp
// Clamp horizontal speed
Vector2 horizontalVelocity = new Vector2(velocity.x, velocity.z);
if (horizontalVelocity.magnitude > maxSpeed)
{
    horizontalVelocity = horizontalVelocity.normalized * maxSpeed;
    velocity.x = horizontalVelocity.x;
    velocity.z = horizontalVelocity.y;
}

// Clamp fall speed (terminal velocity)
if (velocity.y < maxFallSpeed)
{
    velocity.y = maxFallSpeed;
}
```

### Drag and Friction

**Air Drag** (slows horizontal movement):
```csharp
velocity.x *= (1 - airDrag * Time.deltaTime);
velocity.z *= (1 - airDrag * Time.deltaTime);
```

**Ground Friction** (stops sliding):
```csharp
if (isGrounded && input.magnitude < 0.1f)
{
    velocity.x *= (1 - groundFriction * Time.deltaTime);
    velocity.z *= (1 - groundFriction * Time.deltaTime);
}
```

### Physics Materials

Create different surface feels:

**Ice Surface**:
- Friction: 0.0
- Bounciness: 0.1

**Rubber Surface**:
- Friction: 1.0
- Bounciness: 0.8

**Wood Surface**:
- Friction: 0.6
- Bounciness: 0.2

### Springs and Oscillation

Create bouncy, spring-like movement:

```csharp
// Hooke's Law: F = -kx
Vector3 displacement = transform.position - targetPosition;
Vector3 springForce = -springConstant * displacement;
Vector3 dampingForce = -dampingConstant * velocity;

velocity += (springForce + dampingForce) * Time.deltaTime;
transform.position += velocity * Time.deltaTime;
```

---

## Physics Performance

### Optimization Tips

**1. Use Layers**
- Only check collisions between relevant layers
- `Edit` → `Project Settings` → `Physics` → Layer Collision Matrix

**2. Use Simple Colliders**
- Primitives (box, sphere, capsule) are fastest
- Avoid mesh colliders when possible

**3. Adjust Physics Timestep**
```csharp
Time.fixedDeltaTime = 0.02f;  // 50 updates/sec (default)
// Lower = more accurate, but slower
// Higher = faster, but less accurate
```

**4. Use Sleep**
- Idle Rigidbodies "sleep" to save performance
- Automatically wake when interacted with

**5. Reduce Solver Iterations**
```csharp
Physics.defaultSolverIterations = 6;  // Default
// Lower = faster but less stable
// Higher = slower but more stable
```

### Profiling Physics

Check physics performance:

1. Window → Analysis → Profiler
2. Look at "Physics" section
3. Check "Physics.Simulate" time

If physics is slow:
- Reduce number of physics objects
- Simplify colliders
- Adjust settings above

---

## Experimentation Lab

### Experiment 1: Gravity Presets

Use PhysicsConfigurator script:

1. Add PhysicsConfigurator to scene
2. Try different gravity presets:
   - Moon: Floaty jumps
   - Earth: Normal
   - Jupiter: Super heavy
   - Zero: Space mode!

### Experiment 2: Jump Force vs Gravity

In PlayerController.cs, try combinations:

| Jump Force | Gravity Multiplier | Feel |
|------------|-------------------|------|
| 8.0 | 2.5 | Responsive, snappy |
| 12.0 | 1.0 | Floaty, high jumps |
| 5.0 | 3.0 | Low, fast jumps |
| 15.0 | 2.0 | Huge air time |

### Experiment 3: Movement Acceleration

Change `accelerationRate` in PlayerController.cs:

- 1.0 = Slippery, ice skating
- 5.0 = Gradual acceleration
- 10.0 = Default, responsive
- 20.0 = Instant, arcade feel

### Experiment 4: Air Control

Add this to PlayerController.cs:

```csharp
[SerializeField] private float airControlFactor = 0.5f;  // 0 = no control, 1 = full control

// In HandleInput(), when applying movement:
float controlFactor = isGrounded ? 1.0f : airControlFactor;
velocity.x = Mathf.Lerp(velocity.x, targetVelocity.x * controlFactor, accelerationRate * Time.deltaTime);
velocity.z = Mathf.Lerp(velocity.z, targetVelocity.z * controlFactor, accelerationRate * Time.deltaTime);
```

Try values: 0.0, 0.3, 0.5, 1.0

---

## Summary

### Key Takeaways

1. **Gravity** = constant downward acceleration
2. **Forces** change velocity, **velocity** changes position
3. **Character Controller** = kinematic (more control)
4. **Rigidbody** = dynamic (more realistic)
5. **Game feel** matters more than realism!
6. **Iterate and test** - feel is subjective

### Physics Checklist for Your Game

- [ ] Choose movement system (Character Controller vs Rigidbody)
- [ ] Set gravity appropriate for your game's feel
- [ ] Tune jump force and gravity multiplier
- [ ] Adjust acceleration and rotation speeds
- [ ] Implement collision detection
- [ ] Add forgiveness mechanics (coyote time, jump buffering)
- [ ] Test on different hardware
- [ ] Iterate based on playtest feedback

---

## Additional Resources

- [Unity Physics Documentation](https://docs.unity3d.com/Manual/PhysicsSection.html)
- [Game Feel: A Designer's Guide](http://www.game-feel.com/)
- [GMTK: What Makes Celeste's Movement Feel So Good](https://www.youtube.com/watch?v=yorTG9at90g)
- [Physics for Game Developers](http://shop.oreilly.com/product/9780596000066.do)

---

**Remember**: Physics in games is about creating a fun experience, not simulating reality perfectly. Experiment, iterate, and trust your instincts about what feels good to play!

Happy physics hacking! ⚛️🎮
