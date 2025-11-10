# Scene Setup Guide 🎬

This guide will walk you through creating your first playable scene in Unity!

## Quick Start - Create Your First Scene

### Step 1: Create a New Scene

1. In Unity, go to `File` → `New Scene`
2. Choose "3D (Built-in Render Pipeline)" template
3. Click "Create"
4. Save immediately: `File` → `Save As` → `Assets/Scenes/MainGame.scene`

You now have an empty scene with a Main Camera and Directional Light!

### Step 2: Add the Ground (Endless Terrain)

1. **Create Terrain Manager:**
   - Right-click in Hierarchy → `Create Empty`
   - Rename to "TerrainManager"
   - Position: (0, 0, 0)

2. **Add Endless Terrain Script:**
   - Select TerrainManager
   - In Inspector, click `Add Component`
   - Search for "EndlessField"
   - Click to add

3. **Configure Terrain (optional):**
   - Tile Size: 20 (default) - size of each terrain tile
   - View Distance In Tiles: 5 - how many tiles around player
   - Use Height Variation: Check for bumpy terrain
   - Material: Drag a material from Assets/Materials if you created one

The terrain will automatically find and follow your player!

### Step 3: Add the Player Character

1. **Create Player GameObject:**
   - Right-click in Hierarchy → `3D Object` → `Capsule`
   - Rename to "Player"
   - Position: (0, 1, 0) - start above ground

2. **Add Player Tag:**
   - Select Player
   - In Inspector, click Tag dropdown (top)
   - Select "Add Tag..."
   - Click + and type "Player"
   - Select Player again
   - Set Tag to "Player"

3. **Add Character Controller:**
   - With Player selected
   - Inspector → `Add Component` → "Character Controller"
   - Set these properties:
     - Center: (0, 0, 0)
     - Radius: 0.5
     - Height: 2
     - Slope Limit: 45
     - Step Offset: 0.3

4. **Add Movement Script:**
   - Still on Player
   - Inspector → `Add Component` → "PlayerController"
   - Adjust settings as desired:
     - Walk Speed: 3 (default)
     - Run Speed: 6 (default)
     - Jump Force: 8 (default)

5. **Optional - Add Color:**
   - Select Player
   - Find Mesh Renderer → Materials → Element 0
   - Click color next to Albedo
   - Choose your hero's color!

### Step 4: Set Up the Camera

1. **Select Main Camera** in Hierarchy

2. **Add Camera Script:**
   - Inspector → `Add Component` → "ThirdPersonCamera"

3. **Assign Target:**
   - Find "Target" field in ThirdPersonCamera component
   - Drag "Player" from Hierarchy into this field
   - Or click the circle icon and select Player

4. **Optional - Adjust Camera:**
   - Distance: 5 (how far back)
   - Height: 2 (how high up)
   - Mouse Sensitivity: 3 (look speed)
   - Allow Zoom: Checked

### Step 5: Add Lighting

Your scene already has a Directional Light, but let's configure it:

1. **Select Directional Light** in Hierarchy

2. **Adjust Light:**
   - Rotation: (50, -30, 0) for nice angle
   - Color: Slightly yellow (255, 244, 214) for warm sunlight
   - Intensity: 1

3. **Add Ambient Light:**
   - Go to `Window` → `Rendering` → `Lighting`
   - Environment tab
   - Skybox Material: Default-Skybox (should already be set)
   - Sun Source: Drag Directional Light here
   - Ambient Color: Light blue (137, 181, 255)

### Step 6: Add Physics Configuration (Optional)

1. **Create Physics Manager:**
   - Right-click in Hierarchy → `Create Empty`
   - Rename to "PhysicsManager"

2. **Add Physics Script:**
   - Select PhysicsManager
   - Inspector → `Add Component` → "PhysicsConfigurator"

3. **Try Different Gravity:**
   - Gravity Preset: Try "Moon" or "Low"
   - Apply On Start: Checked

### Step 7: Test Your Scene!

1. Click the **Play** button (▶️) at the top
2. Use controls:
   - **WASD** to move
   - **Shift** to run
   - **Space** to jump
   - **C** to duck
   - **Mouse** to look around

3. If it works, congratulations! 🎉

## Common Issues and Fixes

### Player Falls Through Ground

**Problem**: Character falls endlessly

**Solutions**:
- Make sure TerrainManager has EndlessField script
- Check that Player has Character Controller component
- Verify Player position Y is above 0
- Terrain tiles need colliders (auto-added by EndlessField)

### Camera Doesn't Follow Player

**Problem**: Camera stays in one place

**Solutions**:
- Check ThirdPersonCamera has "Target" assigned to Player
- Verify Player is tagged as "Player"
- Make sure ThirdPersonCamera script is on Main Camera

### Can't Move Character

**Problem**: Pressing WASD does nothing

**Solutions**:
- Verify PlayerController script is on Player
- Check Character Controller component exists
- Make sure you're in Play mode (▶️)
- Check Console (Ctrl+Shift+C) for errors

### Character Moves Too Fast/Slow

**Problem**: Movement speed feels wrong

**Solutions**:
- Select Player
- Adjust PlayerController settings:
  - Walk Speed (lower = slower)
  - Run Speed (higher = faster)
- Try: Walk=3, Run=6 for default feel

### Camera is Too Close/Far

**Problem**: Camera distance uncomfortable

**Solutions**:
- Select Main Camera
- In ThirdPersonCamera component:
  - Distance: Increase to move back, decrease to move closer
  - Height: Adjust vertical offset

## Experimentation Ideas

### Experiment 1: Create Obstacles

Add objects to jump over:

1. Right-click in Hierarchy → `3D Object` → `Cube`
2. Position it in front of player: (0, 0.5, 10)
3. Scale it: (5, 1, 1) - long wall
4. Test: Can you jump over it?

Try different sizes and positions!

### Experiment 2: Create a Platform

Make a raised platform:

1. Create Cube: Position (10, 2, 10)
2. Scale: (5, 0.5, 5)
3. Jump onto it from the ground!

### Experiment 3: Add More Lights

Create dramatic lighting:

1. Right-click → `Light` → `Point Light`
2. Position: (5, 2, 5)
3. Color: Choose any color
4. Range: 10
5. Intensity: 2

Create colored zones in your world!

### Experiment 4: Moon Gravity

Feel what it's like on the moon:

1. Select PhysicsManager (or create one)
2. PhysicsConfigurator → Gravity Preset: "Moon"
3. Play and feel the floaty jumps!

### Experiment 5: Multiple Materials

Make a colorful character:

1. Create materials: Assets → Create → Material
2. Name them: "HeadMaterial", "BodyMaterial"
3. Assign different colors to each
4. Drag onto Player's different parts

## Advanced Scene Setup

### Adding a Skybox

1. Download a skybox from Unity Asset Store (search "skybox")
2. Import into project
3. `Window` → `Rendering` → `Lighting`
4. Drag skybox material to "Skybox Material" field

### Adding Fog

Create atmospheric depth:

1. `Window` → `Rendering` → `Lighting`
2. Environment → Fog
3. Check "Fog" checkbox
4. Fog Color: Light blue
5. Fog Mode: Exponential Squared
6. Fog Density: 0.01

### Adding Post-Processing

Make your game look more polished:

1. `Window` → `Package Manager`
2. Search "Post Processing"
3. Install
4. Create Post-Processing Volume (Google for tutorials)

### Multiple Scenes

Create a main menu:

1. `File` → `New Scene` → "MainMenu"
2. Add UI: Right-click → UI → Button
3. Script to load game scene:

```csharp
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public void PlayGame()
    {
        SceneManager.LoadScene("MainGame");
    }
}
```

4. Attach to button's OnClick event

## Scene Organization Tips

### Hierarchy Organization

Organize your scene like this:

```
Scene: MainGame
├── === MANAGERS ===
│   ├── GameManager
│   ├── TerrainManager
│   └── PhysicsManager
├── === PLAYER ===
│   └── Player
├── === CAMERA ===
│   └── Main Camera
├── === LIGHTING ===
│   ├── Directional Light
│   └── Point Lights
├── === ENVIRONMENT ===
│   ├── Obstacles
│   ├── Platforms
│   └── Decorations
└── === UI ===
    └── Canvas
```

Use empty GameObjects as folders!

### Naming Conventions

- **PascalCase** for GameObjects: "PlayerCharacter", "MainCamera"
- **Descriptive names**: "JumpingPlatform" not "Cube (3)"
- **Prefixes for groups**: "Obstacle_Wall1", "Obstacle_Wall2"

### Using Prefabs

Turn repeated objects into prefabs:

1. Set up object in scene (e.g., obstacle)
2. Drag from Hierarchy to Assets/Prefabs/
3. Now you have a reusable template!
4. Drag from Prefabs into scene to create instances
5. Changes to prefab affect all instances

## Performance Tips

### Keep Scene Simple

- Start simple, add complexity gradually
- Profile performance: `Window` → `Analysis` → `Profiler`
- Target 60 FPS minimum

### Optimize Terrain

If FPS is low:
- Reduce EndlessField view distance
- Use simpler terrain tiles
- Reduce tile count

### Lighting Optimization

- Use Baked lighting for static objects
- Limit real-time shadows
- Combine light sources

## Saving and Loading

### Scene Management

**Save Scene:**
- `File` → `Save` (Ctrl+S)
- Save often!

**Save As:**
- `File` → `Save As` to create variations
- Example: "MainGame_Desert", "MainGame_Forest"

**Open Scene:**
- Double-click scene in Project window
- Or `File` → `Open Scene`

### Build Settings

Add scene to build:
1. `File` → `Build Settings`
2. Click `Add Open Scenes`
3. Scene appears in list
4. Check the checkbox to include in build

## Summary Checklist

For a complete playable scene:

- [ ] Scene created and saved
- [ ] TerrainManager with EndlessField
- [ ] Player with Character Controller and PlayerController
- [ ] Player tagged as "Player"
- [ ] Main Camera with ThirdPersonCamera
- [ ] Camera target set to Player
- [ ] Directional Light configured
- [ ] (Optional) PhysicsManager with PhysicsConfigurator
- [ ] Scene tested in Play mode
- [ ] Scene added to Build Settings

## Next Steps

Once your scene works:

1. **Experiment** - Try all the experiments above
2. **Customize** - Make it your own with colors, shapes, lighting
3. **Add gameplay** - Collectibles, enemies, puzzles
4. **Create more scenes** - Menu, multiple levels
5. **Build and share** - `File` → `Build Settings` → `Build`

Have fun creating your world! 🌍✨
