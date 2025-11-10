# Unity Beginner's Guide 🎓

Welcome to Unity! This guide will teach you the fundamental concepts you need to know to start creating games.

## Table of Contents

1. [Unity Interface Overview](#unity-interface-overview)
2. [Core Concepts](#core-concepts)
3. [GameObjects and Components](#gameobjects-and-components)
4. [Transforms and 3D Space](#transforms-and-3d-space)
5. [Scripts and C#](#scripts-and-c)
6. [Unity Lifecycle Methods](#unity-lifecycle-methods)
7. [Input System](#input-system)
8. [Physics System](#physics-system)
9. [Common Patterns](#common-patterns)
10. [Best Practices](#best-practices)

---

## Unity Interface Overview

### The Main Windows

When you open Unity, you'll see several panels:

```
┌─────────────────────────────────────────────────────────┐
│  Menu Bar: File, Edit, Assets, GameObject, etc.        │
├────────────────┬────────────────────┬──────────────────┤
│   HIERARCHY    │    SCENE VIEW      │    INSPECTOR     │
│                │                    │                  │
│  Lists all     │  Visual editor     │  Properties of   │
│  GameObjects   │  where you place   │  selected object │
│  in your       │  and arrange       │                  │
│  scene         │  objects           │                  │
│                │                    │                  │
├────────────────┴────────────────────┴──────────────────┤
│               PROJECT (Assets)                          │
│  All files in your project                             │
│                                                         │
├────────────────────────────────────────────────────────┤
│               CONSOLE                                   │
│  Shows Debug.Log messages, warnings, and errors        │
└────────────────────────────────────────────────────────┘
```

### Key Panels Explained

**Hierarchy** (left):
- Lists every object in your current scene
- Organized in parent-child relationships
- Click objects to select them

**Scene View** (center):
- 3D visualization of your game world
- Use mouse to navigate:
  - Right-click + WASD to fly around
  - Alt + Left-click to orbit
  - Scroll wheel to zoom
  - Middle mouse button to pan

**Inspector** (right):
- Shows details of selected object
- Modify properties and add components here
- Most of your work happens here!

**Project** (bottom):
- Your file browser
- Contains all assets (scripts, models, textures, etc.)
- Organized into folders

**Console** (bottom, toggle with Ctrl+Shift+C):
- Shows messages from your code
- Errors appear in red (fix these!)
- Warnings in yellow (should fix)
- Messages in white (informational)

### Essential Toolbar Buttons

Located at top of Scene view:

- **Hand Tool** (Q): Move around the scene
- **Move Tool** (W): Move objects
- **Rotate Tool** (E): Rotate objects
- **Scale Tool** (R): Resize objects
- **Rect Tool** (T): For 2D UI elements
- **Transform Tool** (Y): All tools combined

### Play Controls

At the very top center:

- **▶ Play**: Test your game
- **⏸ Pause**: Freeze game
- **⏭ Step**: Advance one frame

**IMPORTANT**: Changes made in Play mode are NOT saved! Exit play mode before modifying.

---

## Core Concepts

### What is a Game Engine?

Unity is a **game engine** - a framework that handles:
- ✅ Rendering (displaying graphics)
- ✅ Physics (collisions, gravity)
- ✅ Audio
- ✅ Input (keyboard, mouse, gamepad)
- ✅ Asset management

You focus on **game logic** and **design**, Unity handles the technical stuff!

### The Game Loop

Every game runs in a loop:

```
START GAME
  ↓
Initialize everything
  ↓
┌─────────────────┐
│  Update Input   │ ← Check what player is doing
│       ↓         │
│  Update Logic   │ ← Calculate game state
│       ↓         │
│  Render Frame   │ ← Draw everything
│       ↓         │
│  Repeat 60x/sec │
└─────────────────┘
```

In Unity, you write code for the "Update Logic" part!

### Scene vs Game

- **Scene**: A container for your game world (like a level or menu)
- **Game**: Can have multiple scenes (Main Menu, Level 1, Level 2, etc.)
- You can load different scenes as the player progresses

---

## GameObjects and Components

### GameObjects

**Everything in Unity is a GameObject.**

A GameObject is just a container - it does nothing by itself!

```
GameObject: "Player"
  - Just a name and a position
  - No appearance
  - No behavior
  - No functionality

GameObject needs COMPONENTS to do things!
```

### Components

Components give GameObjects abilities:

```
GameObject: "Player"
  ├─ Transform        (Position, Rotation, Scale)
  ├─ Mesh Renderer    (Makes it visible)
  ├─ Collider         (Allows collisions)
  ├─ Rigidbody        (Adds physics)
  └─ PlayerController (Your custom behavior script)
```

**Think of it like building with LEGO blocks:**
- GameObject = the base plate
- Components = individual LEGO blocks you attach

### Common Built-in Components

| Component | Purpose |
|-----------|---------|
| **Transform** | Position, rotation, scale (every GameObject has this) |
| **Mesh Renderer** | Displays 3D model |
| **Mesh Filter** | Stores 3D model data |
| **Collider** | Defines collision shape (Box, Sphere, Capsule, Mesh) |
| **Rigidbody** | Adds physics (gravity, forces) |
| **Character Controller** | Specialized player movement |
| **Camera** | Defines what player sees |
| **Light** | Illuminates the scene |
| **Audio Source** | Plays sounds |
| **Animator** | Controls animations |

### Adding/Removing Components

**Add a component:**
1. Select GameObject
2. In Inspector, click "Add Component"
3. Search for component name
4. Click to add

**Remove a component:**
1. Select GameObject
2. Find component in Inspector
3. Click ⋮ (three dots) → Remove Component

---

## Transforms and 3D Space

### Understanding 3D Coordinates

Unity uses a 3D coordinate system:

```
      Y (Up)
      |
      |
      |_______ X (Right)
     /
    /
   Z (Forward)
```

- **X-axis**: Left (-) / Right (+)
- **Y-axis**: Down (-) / Up (+)
- **Z-axis**: Back (-) / Forward (+)

### Transform Properties

Every GameObject has a Transform with three properties:

**Position**: Where the object is
```csharp
transform.position = new Vector3(5, 0, 10);  // X=5, Y=0, Z=10
```

**Rotation**: Which way it faces
```csharp
transform.rotation = Quaternion.Euler(0, 90, 0);  // Rotated 90° on Y-axis
```

**Scale**: How big it is
```csharp
transform.localScale = new Vector3(2, 2, 2);  // Double the size
```

### Local vs World Space

- **World Space**: Position relative to origin (0,0,0)
- **Local Space**: Position relative to parent object

```
Parent at (10, 0, 0)
  └─ Child at local (5, 0, 0)
     World position: (15, 0, 0)
```

### Parent-Child Relationships

Objects can be parented:

```
Car (parent)
  ├─ Wheel1 (child)
  ├─ Wheel2 (child)
  ├─ Wheel3 (child)
  └─ Wheel4 (child)
```

When parent moves, all children move with it!

**How to parent:**
- Drag child object onto parent in Hierarchy

---

## Scripts and C#

### What is C#?

C# (C-Sharp) is the programming language Unity uses.

**Basic structure:**

```csharp
using UnityEngine;  // Import Unity's code

public class MyScript : MonoBehaviour  // Define a class
{
    // Variables (data)
    public int health = 100;
    private float speed = 5.0f;

    // Methods (functions)
    void Start()
    {
        // Runs once at beginning
        Debug.Log("Game started!");
    }

    void Update()
    {
        // Runs every frame
        transform.position += Vector3.forward * speed * Time.deltaTime;
    }
}
```

### Creating a Script

1. In Project window, right-click → Create → C# Script
2. Name it (use PascalCase: "MyPlayerController")
3. Double-click to open in your code editor
4. Write your code
5. Save (Ctrl+S)
6. Return to Unity (it will compile)

### Attaching Scripts to GameObjects

Scripts are components! To use them:

1. Select a GameObject
2. Drag script from Project onto Inspector
3. Or: Add Component → search for script name

### Variables in Inspector

Use `[SerializeField]` or `public` to expose variables:

```csharp
[SerializeField] private float speed = 5.0f;  // Visible in Inspector
public int health = 100;                       // Also visible
private float secretValue = 42;                // NOT visible
```

You can modify these values in Unity's Inspector!

### Accessing Other Components

Get a component on the same GameObject:

```csharp
Rigidbody rb = GetComponent<Rigidbody>();
rb.AddForce(Vector3.up * 10);
```

Get a component on another GameObject:

```csharp
GameObject player = GameObject.FindGameObjectWithTag("Player");
PlayerController controller = player.GetComponent<PlayerController>();
```

---

## Unity Lifecycle Methods

Unity calls specific methods at specific times:

### Execution Order

```
Game Start
  ↓
Awake()         // Object created, other objects may not exist yet
  ↓
Start()         // First frame, all objects exist, use for initialization
  ↓
┌──────────────┐
│ GAME LOOP    │
│   ↓          │
│ FixedUpdate()│ // Fixed time intervals, use for physics
│   ↓          │
│ Update()     │ // Every frame, use for game logic & input
│   ↓          │
│ LateUpdate() │ // After Update, use for cameras following players
│   ↓          │
│ (Render)     │ // Unity draws the frame
└──────────────┘
     ↓
OnDestroy()    // Object destroyed
```

### Common Methods

**Awake()** - First thing to run
```csharp
void Awake()
{
    // Initialize references
    rb = GetComponent<Rigidbody>();
}
```

**Start()** - After Awake, before first frame
```csharp
void Start()
{
    // Safe to access other objects
    player = GameObject.FindGameObjectWithTag("Player");
}
```

**Update()** - Every frame (60 FPS = 60 times per second)
```csharp
void Update()
{
    // Input handling
    if (Input.GetKeyDown(KeyCode.Space))
    {
        Jump();
    }
}
```

**FixedUpdate()** - Fixed time intervals (physics)
```csharp
void FixedUpdate()
{
    // Physics code here
    rb.AddForce(Vector3.forward * force);
}
```

**LateUpdate()** - After all Updates
```csharp
void LateUpdate()
{
    // Camera following player
    transform.position = player.position + offset;
}
```

### Time.deltaTime

**Always multiply movement by Time.deltaTime!**

```csharp
// WRONG - speed depends on framerate
transform.position += Vector3.forward * speed;

// CORRECT - consistent speed regardless of framerate
transform.position += Vector3.forward * speed * Time.deltaTime;
```

Why? Different computers run at different framerates. Time.deltaTime = time since last frame, so you move the same speed on any device!

---

## Input System

### Keyboard Input

```csharp
// Is key currently held down?
if (Input.GetKey(KeyCode.W))
{
    // Move forward while W is held
}

// Was key just pressed this frame?
if (Input.GetKeyDown(KeyCode.Space))
{
    // Jump once when Space is pressed
}

// Was key just released?
if (Input.GetKeyUp(KeyCode.Space))
{
    // Stop charging attack when Space released
}
```

### Mouse Input

```csharp
// Mouse buttons (0 = left, 1 = right, 2 = middle)
if (Input.GetMouseButton(0))
{
    // Left mouse held
}

// Mouse position on screen
Vector3 mousePos = Input.mousePosition;

// Mouse movement
float mouseX = Input.GetAxis("Mouse X");
float mouseY = Input.GetAxis("Mouse Y");
```

### Input Axes

Unity provides preconfigured axes (Edit → Project Settings → Input Manager):

```csharp
// Returns -1 to 1
float horizontal = Input.GetAxis("Horizontal");  // A/D or Left/Right
float vertical = Input.GetAxis("Vertical");      // W/S or Up/Down

// Instantaneous (no smoothing)
float horizontal = Input.GetAxisRaw("Horizontal");

// Jump button (space by default)
if (Input.GetButtonDown("Jump"))
{
    Jump();
}
```

---

## Physics System

### Rigidbody vs Character Controller

**Rigidbody** - Full physics simulation
- Affected by gravity and forces
- Use for: balls, projectiles, vehicles, ragdolls
- Can collide with everything

**Character Controller** - Simplified character movement
- Not affected by physics forces (except your own code)
- Use for: player characters, NPCs
- More control, less "realistic"

**This project uses Character Controller for the player!**

### Colliders

Colliders define the physical shape:

- **Box Collider** - Cube shape
- **Sphere Collider** - Ball shape
- **Capsule Collider** - Pill shape (good for characters!)
- **Mesh Collider** - Custom shape (expensive, avoid if possible)

### Collision vs Trigger

**Collision**: Physical impact (can't pass through)
```csharp
void OnCollisionEnter(Collision collision)
{
    Debug.Log("Hit: " + collision.gameObject.name);
}
```

**Trigger**: Detection zone (can pass through)
```csharp
void OnTriggerEnter(Collider other)
{
    if (other.CompareTag("Coin"))
    {
        CollectCoin();
    }
}
```

Set in Collider component: Check "Is Trigger" for trigger behavior.

### Applying Forces

```csharp
Rigidbody rb = GetComponent<Rigidbody>();

// Add continuous force
rb.AddForce(Vector3.up * 10);

// Add instant force (explosion, jump)
rb.AddForce(Vector3.up * 10, ForceMode.Impulse);

// Set velocity directly
rb.velocity = new Vector3(5, 0, 0);
```

### Raycasting

Shoot invisible rays to detect objects:

```csharp
// Shoot ray forward
Ray ray = new Ray(transform.position, transform.forward);
RaycastHit hit;

if (Physics.Raycast(ray, out hit, 100f))
{
    Debug.Log("Hit: " + hit.collider.gameObject.name);
    Debug.Log("Distance: " + hit.distance);
}
```

Use cases:
- Line of sight checks
- Shooting mechanics
- Ground detection

---

## Common Patterns

### Singleton Pattern

Only one instance of a class:

```csharp
public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
}

// Access from anywhere:
GameManager.Instance.DoSomething();
```

### Object Pooling

Reuse objects instead of creating/destroying:

```csharp
public class BulletPool : MonoBehaviour
{
    public GameObject bulletPrefab;
    private Queue<GameObject> pool = new Queue<GameObject>();

    public GameObject GetBullet()
    {
        if (pool.Count > 0)
        {
            GameObject bullet = pool.Dequeue();
            bullet.SetActive(true);
            return bullet;
        }
        return Instantiate(bulletPrefab);
    }

    public void ReturnBullet(GameObject bullet)
    {
        bullet.SetActive(false);
        pool.Enqueue(bullet);
    }
}
```

### Event System

Decouple code with events:

```csharp
// Define event
public delegate void OnHealthChanged(int newHealth);
public static event OnHealthChanged onHealthChanged;

// Trigger event
if (onHealthChanged != null)
    onHealthChanged(currentHealth);

// Subscribe to event
void Start()
{
    PlayerHealth.onHealthChanged += HandleHealthChanged;
}

void HandleHealthChanged(int newHealth)
{
    UpdateHealthUI(newHealth);
}
```

---

## Best Practices

### Code Organization

✅ **DO:**
- Use meaningful names: `playerSpeed` not `ps`
- Group related code in folders
- One class per file
- Use comments for complex logic

❌ **DON'T:**
- Put everything in one giant script
- Use vague names like `temp`, `thing`, `data`
- Have hundreds of public variables

### Performance

✅ **DO:**
- Cache component references in Start()
- Use object pooling for frequently created objects
- Disable scripts/objects when not needed

❌ **DON'T:**
- Call GetComponent() every frame
- Use Instantiate/Destroy rapidly
- Search for objects every frame (Find, FindObjectOfType)

### Inspector Organization

Use attributes to organize:

```csharp
[Header("Movement Settings")]
public float walkSpeed = 3.0f;
public float runSpeed = 6.0f;

[Header("Jump Settings")]
public float jumpForce = 5.0f;

[Space(10)]

[Tooltip("This explains what this variable does")]
public int health = 100;

[Range(0, 10)]  // Creates a slider
public float volume = 5.0f;
```

### Debugging

Use Debug methods:

```csharp
// Print to console
Debug.Log("Player health: " + health);
Debug.LogWarning("Low ammo!");
Debug.LogError("Something broke!");

// Draw visual debug lines in Scene view
Debug.DrawRay(transform.position, transform.forward * 10, Color.red);
Debug.DrawLine(startPos, endPos, Color.green, 2.0f);
```

---

## Quick Reference Card

### Essential Shortcuts

| Shortcut | Action |
|----------|--------|
| **Ctrl + N** | New scene |
| **Ctrl + S** | Save scene |
| **Ctrl + P** | Play/Stop |
| **Ctrl + Shift + F** | Align camera to selected object |
| **F** | Focus on selected object |
| **Q, W, E, R, T** | Tool selection |
| **Ctrl + D** | Duplicate selected object |
| **Ctrl + Shift + C** | Toggle console |

### Code Snippets

```csharp
// Move object
transform.position += Vector3.forward * speed * Time.deltaTime;

// Rotate object
transform.Rotate(0, 90 * Time.deltaTime, 0);

// Look at target
transform.LookAt(target);

// Instantiate object
GameObject newObj = Instantiate(prefab, position, rotation);

// Destroy object
Destroy(gameObject);

// Wait for seconds (coroutine)
StartCoroutine(WaitAndDo(2.0f));
IEnumerator WaitAndDo(float seconds)
{
    yield return new WaitForSeconds(seconds);
    Debug.Log("Waited!");
}

// Find object
GameObject player = GameObject.FindGameObjectWithTag("Player");

// Get component
Rigidbody rb = GetComponent<Rigidbody>();
```

---

## Learning Path

### Beginner (You are here!)

1. ✅ Understand Unity interface
2. ✅ Learn GameObjects and Components
3. ✅ Write simple scripts
4. ⬜ Create a simple game (Pong, Flappy Bird clone)

### Intermediate

1. ⬜ Learn coroutines and events
2. ⬜ Understand prefabs and scenes
3. ⬜ Build UI systems
4. ⬜ Add sound and particles

### Advanced

1. ⬜ Master animation systems
2. ⬜ Implement AI and pathfinding
3. ⬜ Create save/load systems
4. ⬜ Optimize for performance
5. ⬜ Build and publish a complete game!

---

## Resources

### Unity Official

- [Unity Learn](https://learn.unity.com/) - Free courses
- [Unity Manual](https://docs.unity3d.com/Manual/) - Complete documentation
- [Unity Scripting API](https://docs.unity3d.com/ScriptReference/) - Code reference

### YouTube Channels

- **Brackeys** - Best beginner tutorials
- **Sebastian Lague** - Advanced techniques
- **Code Monkey** - Practical examples
- **Dani** - Entertaining and educational

### Communities

- [Unity Forums](https://forum.unity.com/)
- [r/Unity3D](https://www.reddit.com/r/Unity3D/)
- [Unity Discord](https://discord.com/invite/unity)

---

## Next Steps

1. **Play with this project** - Change values, break things, fix them!
2. **Read the scripts** - Every line is commented for learning
3. **Try the experiments** - Each script has experimentation ideas
4. **Build something small** - Start with simple mechanics
5. **Have fun!** - Game development is creative and rewarding!

---

**Remember**: Every expert was once a beginner. Don't be afraid to experiment, make mistakes, and ask questions. The Unity community is incredibly helpful!

Happy game development! 🎮✨
