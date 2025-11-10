# Character Appearance Customization Guide 🎨

Welcome! This guide will teach you how to customize your character's appearance, from simple color changes to importing full 3D models with animations.

## Table of Contents

1. [Quick Start - Change Colors](#quick-start---change-colors)
2. [Using Built-in Shapes](#using-built-in-shapes)
3. [Importing 3D Models](#importing-3d-models)
4. [Adding Animations](#adding-animations)
5. [Advanced Customization](#advanced-customization)

---

## Quick Start - Change Colors

The easiest way to customize your character is by changing its color!

### Method 1: Change Material Color

1. **Select your Player** in the Hierarchy
2. In the Inspector, find the **Mesh Renderer** component
3. Expand **Materials** → **Element 0**
4. Click the color picker next to **Albedo** (or Base Map)
5. Choose your favorite color!

### Method 2: Create a Custom Material

1. Right-click in `Assets/Materials/` → `Create` → `Material`
2. Name it "HeroMaterial"
3. Click on the new material to select it
4. In Inspector, change:
   - **Albedo/Base Color**: Your character's main color
   - **Metallic**: Makes it shiny (0 = matte, 1 = mirror)
   - **Smoothness**: Controls glossiness
5. Drag the material onto your character in the Scene view

### Pro Tip: Multiple Materials

Create different materials for different body parts:
- **"HeroArmor"** - Metallic and dark for armor
- **"HeroSkin"** - Non-metallic and flesh-toned
- **"HeroClothes"** - Colorful with low smoothness

---

## Using Built-in Shapes

Create a custom character using Unity's primitive shapes!

### Simple Character Design

1. **Create the body:**
   ```
   - Right-click in Hierarchy → 3D Object → Capsule
   - Name it "PlayerBody"
   - Scale: (1, 1.5, 1)
   ```

2. **Add a head:**
   ```
   - Right-click on PlayerBody → 3D Object → Sphere
   - Name it "Head"
   - Position: (0, 1.5, 0)
   - Scale: (0.8, 0.8, 0.8)
   ```

3. **Add arms:**
   ```
   - Right-click on PlayerBody → 3D Object → Capsule
   - Name it "ArmLeft"
   - Rotation: (0, 0, 90)  // Rotate to horizontal
   - Position: (-0.8, 0.5, 0)
   - Scale: (0.3, 0.6, 0.3)

   - Duplicate (Ctrl+D) and rename to "ArmRight"
   - Position: (0.8, 0.5, 0)
   ```

4. **Add legs:**
   ```
   - Right-click on PlayerBody → 3D Object → Capsule
   - Name it "LegLeft"
   - Position: (-0.3, -1.0, 0)
   - Scale: (0.3, 0.8, 0.3)

   - Duplicate and rename to "LegRight"
   - Position: (0.3, -1.0, 0)
   ```

5. **Parent everything:**
   - Drag Head, Arms, and Legs onto PlayerBody in Hierarchy
   - Now PlayerBody is the parent containing all parts

6. **Add Character Controller:**
   - Select PlayerBody
   - `Add Component` → `Character Controller`
   - Adjust size to fit your custom character

7. **Add the script:**
   - `Add Component` → `PlayerController`

### Customizing Your Shape Character

Apply different materials to each part:
```csharp
// Head: Skin color
// Body: Armor or shirt color
// Arms: Sleeve color
// Legs: Pants color
```

---

## Importing 3D Models

Ready to use professional-looking characters? Let's import a 3D model!

### Where to Get 3D Models

**Free Resources:**
- [Mixamo](https://www.mixamo.com/) - Free characters with animations!
- [Sketchfab](https://sketchfab.com/) - Many free models (check license)
- [Unity Asset Store](https://assetstore.unity.com/) - Some free packs
- [Kenney](https://kenney.nl/assets) - Simple, free game assets

**Formats Unity Supports:**
- `.fbx` (recommended)
- `.obj`
- `.blend` (Blender)
- `.dae` (Collada)

### Step-by-Step: Importing from Mixamo

1. **Download a character:**
   - Go to [Mixamo.com](https://www.mixamo.com/)
   - Sign in (free account)
   - Browse "Characters" tab
   - Click a character you like
   - Click "Download"
   - Format: FBX for Unity
   - Pose: T-Pose or A-Pose
   - Download!

2. **Import to Unity:**
   - Drag the downloaded `.fbx` file into `Assets/Prefabs/`
   - Unity will process it (may take a moment)

3. **Set up the model:**
   - Click the imported model in Assets
   - In Inspector, go to "Rig" tab
   - Animation Type: "Humanoid" (for character with standard body)
   - Click "Apply"

4. **Add to scene:**
   - Drag the model from Assets into Hierarchy
   - Rename it to "Player"
   - Add Tag "Player" (Inspector → Tag → Add Tag)

5. **Add components:**
   - `Add Component` → `Character Controller`
   - Adjust Character Controller size:
     - Center: Usually (0, 1, 0)
     - Radius: About 0.3-0.5
     - Height: Match your model (1.8-2.0 typically)
   - `Add Component` → `PlayerController`

6. **Update camera:**
   - Select Main Camera
   - In ThirdPersonCamera component
   - Drag your new Player to the "Target" field

### Adjusting Character Controller Size

The Character Controller needs to match your model:

```
Visual guide:
    ___    <- Top of capsule (center.y + height/2)
   /   \
  |     |  <- This is your character's body
  |     |
   \___/   <- Bottom of capsule (center.y - height/2)

   |-r-|   <- Radius
```

**Tips:**
- Height: Measure your model from feet to head
- Radius: Should encompass the widest part
- Center: Usually half the height (0, height/2, 0)
- Test by playing and watching the green capsule outline

---

## Adding Animations

Make your character come alive with animations!

### Simple Animation Setup (Mixamo)

1. **Download animations:**
   - Go back to Mixamo
   - Select your character
   - Go to "Animations" tab
   - Search for animations like:
     - "Idle"
     - "Walking"
     - "Running"
     - "Jumping"
   - Download each (FBX for Unity, Without Skin)

2. **Import animations:**
   - Drag all animation files into `Assets/Animations/` folder

3. **Create Animator Controller:**
   - Right-click in Assets → `Create` → `Animator Controller`
   - Name it "PlayerAnimator"

4. **Set up Animator:**
   - Double-click PlayerAnimator to open Animator window
   - Drag your animations into the Animator window
   - Right-click "Idle" → "Set as Layer Default State" (orange)

5. **Create transitions:**
   - Right-click "Idle" → "Make Transition" → Click "Walking"
   - Right-click "Walking" → "Make Transition" → Click "Idle"
   - Repeat for Running, Jumping, etc.

6. **Add parameters:**
   - Click "Parameters" tab in Animator
   - Click "+" → "Float" → Name it "Speed"
   - Click "+" → "Bool" → Name it "IsGrounded"

7. **Configure transitions:**
   - Click a transition arrow (e.g., Idle → Walking)
   - Uncheck "Has Exit Time" (for more responsive transitions)
   - Add Condition: "Speed" Greater than 0.1

8. **Apply to character:**
   - Select your Player in Hierarchy
   - `Add Component` → `Animator`
   - Drag PlayerAnimator into "Controller" field

### Connecting Animations to PlayerController

Add this to PlayerController.cs in the Update() method:

```csharp
// After movement code, add animation updates:
if (animator != null)
{
    // Update speed parameter for walk/run animations
    animator.SetFloat("Speed", CurrentSpeed);

    // Update grounded parameter for jump animations
    animator.SetBool("IsGrounded", isGrounded);
}
```

The animator variable already exists in PlayerController.cs!

### Animation Tips

- **Blend times**: Adjust transition duration for smooth transitions (0.1-0.3 is good)
- **Animation speed**: Select animation in Assets, adjust "Speed" in inspector
- **Root motion**: For advanced movement, enable "Apply Root Motion" in Animator component

---

## Advanced Customization

### Creating Outfits/Equipment

1. **Create child objects:**
   - Right-click on Player → `3D Object` → Cube
   - Name it "Sword", "Shield", etc.
   - Position it in the character's hand

2. **Attach to bones (if rigged):**
   - In Hierarchy, expand your character model
   - Find bone like "RightHand"
   - Drag sword onto RightHand bone
   - Sword now follows hand movement!

### Using Blend Shapes (Facial Expressions)

If your model has blend shapes:

```csharp
// Add this script to change expressions
SkinnedMeshRenderer smr = GetComponent<SkinnedMeshRenderer>();
smr.SetBlendShapeWeight(0, 100f);  // 0 = first blend shape, 100 = full effect
```

### Changing Character at Runtime

Create a character swapper script:

```csharp
public class CharacterSwapper : MonoBehaviour
{
    public GameObject[] characterPrefabs;
    private int currentCharacter = 0;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            SwapCharacter();
        }
    }

    void SwapCharacter()
    {
        // Destroy current
        Destroy(characterPrefabs[currentCharacter]);

        // Instantiate next
        currentCharacter = (currentCharacter + 1) % characterPrefabs.Length;
        GameObject newChar = Instantiate(characterPrefabs[currentCharacter]);

        // Transfer components...
    }
}
```

### Customization via UI

Create a character creator:

```csharp
// Slider for height
public Slider heightSlider;

void OnHeightChanged(float value)
{
    transform.localScale = new Vector3(1, value, 1);
}

// Color picker for skin
public void SetSkinColor(Color color)
{
    Renderer renderer = GetComponent<Renderer>();
    renderer.material.color = color;
}
```

---

## Troubleshooting

### Model is Too Small/Big

- Select model in Assets → Inspector → "Model" tab
- Adjust "Scale Factor" (1 = 1 meter in Unity)
- Click "Apply"

### Model is Facing Wrong Direction

- Model Import Settings → Rotation: Y = 180
- Or in PlayerController, adjust rotation

### Animations Don't Play

- Check Animator is attached to Player
- Verify Animator Controller is assigned
- Check parameters are set correctly
- Look in Animator window to see if states transition

### Character Falls Through Ground

- Adjust Character Controller height/center
- Make sure ground has Collider component

### Model Looks Pink

- Missing textures! Check if texture files were imported
- Drag texture files into same folder as model
- Unity should auto-assign them

---

## Summary Checklist

For a custom character:
- ✅ Import or create model
- ✅ Add Character Controller component
- ✅ Add PlayerController script
- ✅ Tag as "Player"
- ✅ Update camera target reference
- ✅ (Optional) Add Animator for animations
- ✅ (Optional) Customize materials and colors
- ✅ Test in Play mode!

---

## Resources

- [Mixamo](https://www.mixamo.com/) - Free characters & animations
- [Unity Character Controller Documentation](https://docs.unity3d.com/Manual/class-CharacterController.html)
- [Unity Animation Documentation](https://docs.unity3d.com/Manual/AnimationOverview.html)
- [Blender](https://www.blender.org/) - Free 3D modeling software

---

**Have fun creating your unique hero!** 🦸‍♂️🎨

Remember: Start simple (colors, shapes) and gradually move to complex models and animations. Every great game starts with a simple placeholder character!
