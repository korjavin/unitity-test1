# Zelda-Like Unity Starter Project 🎮

Welcome to your first Unity project! This is an educational, Zelda-inspired 3D action-adventure starter project designed to teach you Unity development through hands-on experience.

## 🌟 What's Included

This project provides a solid foundation for creating 3D action-adventure games with:

- **3rd Person Character Controller** - Walk, run, jump, and duck mechanics
- **Smart Camera System** - Smooth follow camera with collision detection
- **Endless Field Terrain** - Infinite procedural terrain generation
- **Physics Playground** - Interactive physics configuration system
- **Educational Documentation** - Learn as you build!

## 📚 Perfect For Beginners

This is an **educational project** focused on:
- ✅ Clean, well-structured code
- ✅ Extensive comments explaining every concept
- ✅ Modular design for easy experimentation
- ✅ Best practices for Unity development
- ✅ Ready-to-customize systems

---

## 🚀 Quick Start Guide

### Prerequisites

You'll need:
1. **Unity Hub** - Download from [unity.com](https://unity.com/download)
2. **Unity Editor 2022.3 LTS or newer** - Install via Unity Hub
3. **A code editor** - Visual Studio, VS Code, or JetBrains Rider

### Installation Steps

1. **Clone this repository**
   ```bash
   git clone <your-repo-url>
   cd unitity-test1
   ```

2. **Open in Unity**
   - Open Unity Hub
   - Click "Add" → "Add project from disk"
   - Navigate to the cloned folder and select it
   - Unity will import the project (this may take a few minutes)

3. **Create Your First Scene**

   Since this is a starter project, you'll need to set up your first scene:

   **a) Create the Scene:**
   - In Unity, go to `File` → `New Scene`
   - Choose "3D" template
   - Save as `Assets/Scenes/MainGame.scene`

   **b) Add the Player:**
   - Right-click in Hierarchy → `3D Object` → `Capsule`
   - Rename it to "Player"
   - Add tag "Player" (Inspector → Tag → Add Tag → "Player")
   - **Add Character Controller:** Inspector → `Add Component` → `Character Controller`
   - **Add Player Script:** Inspector → `Add Component` → Search "PlayerController"

   **c) Add the Camera:**
   - Select Main Camera in Hierarchy
   - **Add Camera Script:** Inspector → `Add Component` → Search "ThirdPersonCamera"
   - In ThirdPersonCamera, drag the Player object to the "Target" field

   **d) Add the Terrain:**
   - Right-click in Hierarchy → `Create Empty`
   - Rename to "TerrainManager"
   - **Add Terrain Script:** Inspector → `Add Component` → Search "EndlessField"
   - The terrain will auto-detect the player

   **e) Add Physics Configuration (Optional):**
   - Right-click in Hierarchy → `Create Empty`
   - Rename to "PhysicsManager"
   - **Add Physics Script:** Inspector → `Add Component` → Search "PhysicsConfigurator"
   - Experiment with different gravity presets!

4. **Play!**
   - Press the Play button (▶️) at the top of Unity
   - Use **WASD** to move
   - **Space** to jump
   - **Shift** to run
   - **C** or **Ctrl** to duck
   - **Mouse** to look around
   - **ESC** to unlock cursor

---

## 🎯 Controls

| Input | Action |
|-------|--------|
| **W, A, S, D** | Move forward, left, back, right |
| **Space** | Jump |
| **Left Shift** | Run (hold while moving) |
| **C or Left Ctrl** | Duck/Crouch |
| **Mouse Movement** | Rotate camera |
| **Mouse Scroll** | Zoom in/out |
| **ESC** | Unlock cursor |

---

## 📁 Project Structure

```
unitity-test1/
├── Assets/
│   ├── Scenes/              # Your game scenes
│   ├── Scripts/
│   │   ├── Character/       # Player controller scripts
│   │   │   └── PlayerController.cs
│   │   ├── Camera/          # Camera system scripts
│   │   │   └── ThirdPersonCamera.cs
│   │   ├── Terrain/         # Terrain generation scripts
│   │   │   └── EndlessField.cs
│   │   └── Physics/         # Physics configuration
│   │       └── PhysicsConfigurator.cs
│   ├── Prefabs/            # Reusable game objects
│   ├── Materials/          # Visual materials
│   └── Textures/           # Images and textures
├── Documentation/
│   └── Guides/             # Educational guides
├── ProjectSettings/        # Unity project configuration
└── README.md              # This file!
```

---

## 🎓 Learning Resources

### Included Guides

Explore the `Documentation/Guides/` folder for detailed tutorials:

1. **[Beginner's Guide](Documentation/Guides/BEGINNERS_GUIDE.md)** - Unity basics and core concepts
2. **[Character Customization](Documentation/Guides/CHARACTER_CUSTOMIZATION.md)** - How to change your hero's appearance
3. **[Physics Guide](Documentation/Guides/PHYSICS_GUIDE.md)** - Understanding gravity, forces, and motion

### Understanding the Code

Every script in this project is **heavily commented** with:
- 📝 Explanations of what each section does
- 🎯 "EDUCATIONAL NOTE" markers highlighting key concepts
- 🧪 "EXPERIMENTATION IDEAS" suggesting things to try
- 💡 Best practices and common patterns

**Start here:** Open `Assets/Scripts/Character/PlayerController.cs` and read through it!

---

## 🛠️ Customization & Experimentation

### Modify Character Movement

Open `Assets/Scripts/Character/PlayerController.cs` and try changing:

```csharp
[SerializeField] private float walkSpeed = 3.0f;  // Change to 5.0 for faster walking
[SerializeField] private float runSpeed = 6.0f;   // Change to 10.0 for super speed!
[SerializeField] private float jumpForce = 8.0f;  // Change to 15.0 for huge jumps
```

### Change Camera Behavior

Open `Assets/Scripts/Camera/ThirdPersonCamera.cs`:

```csharp
[SerializeField] private float distance = 5.0f;     // How far the camera sits back
[SerializeField] private float mouseSensitivity = 3.0f;  // Mouse look speed
[SerializeField] private bool invertY = false;      // Invert vertical look?
```

### Adjust Physics

Use the PhysicsConfigurator component or open `ProjectSettings/DynamicsManager.asset`:

- **Moon gravity**: Set gravity Y to -1.62 for floaty jumps
- **High gravity**: Set gravity Y to -15.0 for snappy, responsive movement
- **Zero gravity**: Set gravity to (0, 0, 0) for space exploration!

---

## 🏗️ Building Your Game

### 🚀 Quick Local Build (Recommended for Mac Users)

**Build on your Mac in 1 command:**

```bash
./build-local.sh
```

That's it! The script will:
- ✅ Find Unity automatically
- ✅ Build your game
- ✅ Create a `.app` file ready to run

**Output:** `Builds/macOS/ZeldaLikeStarter.app`

📖 **Detailed guide:** See [LOCAL_BUILD.md](Documentation/Guides/LOCAL_BUILD.md)

**First time?** Make the script executable:
```bash
chmod +x build-local.sh
```

---

### Building in Unity Editor

**For any platform (Windows, Mac, Linux, WebGL):**

1. Go to `File` → `Build Settings`
2. Click `Add Open Scenes` to add your current scene
3. Select target platform:
   - `PC, Mac & Linux Standalone` → Choose Windows/Mac/Linux
   - `WebGL` for web browsers
4. Click `Switch Platform` if needed (may take a while)
5. Click `Build` and choose an output folder
6. Unity will create your game files

**Build Tips:**
- **Optimize**: `Edit` → `Project Settings` → `Quality`
- **Test often**: Build frequently to catch issues early
- **Scene management**: Ensure all scenes are added to Build Settings

---

## 🔄 CI/CD - Automated Cloud Builds

This project includes GitHub Actions for cloud building!

### What is CI/CD?

**Continuous Integration/Continuous Deployment** builds your game in the cloud without tying up your computer.

### Current Configuration

- **Platform:** macOS only
- **Trigger:** Manual only (no automatic builds on push)
- **Cost:** Uses GitHub Actions minutes (10x rate for macOS)

### How to Use

1. **Set up Unity License** (one-time):
   - Go to: Repository → Settings → Secrets and variables → Actions
   - Add secrets:
     - `UNITY_LICENSE` - Your Unity license file (.ulf contents)
     - `UNITY_EMAIL` - Your Unity account email
     - `UNITY_PASSWORD` - Your Unity account password
   - Guide: [Game CI activation](https://game.ci/docs/github/activation)

2. **Trigger Build Manually:**
   - Go to "Actions" tab in GitHub
   - Click "Unity Build Pipeline" on the left
   - Click "Run workflow" button (top right)
   - Select branch and click "Run workflow"

3. **Download Build:**
   - Wait for build to complete (~10-15 minutes)
   - Download "Build-StandaloneOSX" artifact
   - Extract and run!

### Recommendation

💡 **Use local builds** (`./build-local.sh`) for daily development - it's faster, free, and easier!

💡 **Use CI/CD** only when you need to build while doing other work, or for official releases

---

## 🎨 Customizing Character Appearance

See [CHARACTER_CUSTOMIZATION.md](Documentation/Guides/CHARACTER_CUSTOMIZATION.md) for detailed instructions, but here's a quick overview:

### Using 3D Models

1. **Import a model** (FBX, OBJ, etc.):
   - Drag your model file into `Assets/Prefabs/`

2. **Replace the capsule**:
   - Drag your model into the Hierarchy
   - Copy PlayerController component settings
   - Delete old capsule, rename new model to "Player"
   - Adjust Character Controller size to match model

3. **Add animations** (optional):
   - Import animation files
   - Add Animator component
   - Create Animation Controller
   - Set up animation transitions

### Quick Placeholder Character

Use Unity's primitive shapes:
- Capsule for body
- Sphere for head
- Cubes for arms/legs
- Apply different materials for colors

---

## 🧪 Experimentation Ideas

### Beginner Experiments

1. **Change movement speed** - Make your character super fast or super slow
2. **Adjust jump height** - Create moon-like jumps or tiny hops
3. **Modify gravity** - Experience different planets' gravity
4. **Change terrain size** - Make bigger or smaller terrain tiles
5. **Camera distance** - Get up close or far away

### Intermediate Experiments

1. **Double Jump** - Set `maxAirJumps = 1` in PlayerController
2. **Wall Detection** - Add raycasts to detect nearby walls
3. **Stamina System** - Limit how long the player can run
4. **Different Terrains** - Create multiple terrain types (grass, sand, snow)
5. **Collectibles** - Add objects the player can pick up

### Advanced Experiments

1. **Combat System** - Add weapon handling and enemy AI
2. **Inventory System** - Create item management
3. **Save/Load System** - Persist player progress
4. **Dialogue System** - Add NPCs with conversation trees
5. **Quest System** - Implement missions and objectives

---

## 🐛 Troubleshooting

### Character Falls Through Ground

- Make sure terrain has a Collider component
- Check that player has CharacterController (not Rigidbody)
- Verify terrain is on Default layer

### Camera Clips Through Walls

- Ensure collision layers are set correctly in ThirdPersonCamera
- Increase `collisionRadius` value
- Check that walls have Collider components

### Character Doesn't Move

- Verify PlayerController script is attached
- Check that CharacterController component exists
- Make sure Input settings are configured (Edit → Project Settings → Input Manager)

### Scripts Won't Compile

- Check for typos in script names
- Ensure all scripts are in the Assets folder
- Look at Console window (Window → General → Console) for error messages
- Unity might need to reimport scripts (Assets → Reimport All)

### Build Fails

- Check all scenes are added to Build Settings
- Verify target platform is correctly selected
- Look for errors in Console before building
- Make sure Unity version matches project version

---

## 📖 Additional Learning Resources

### Official Unity Resources

- [Unity Learn](https://learn.unity.com/) - Official tutorials and courses
- [Unity Manual](https://docs.unity3d.com/Manual/index.html) - Complete documentation
- [Unity Scripting API](https://docs.unity3d.com/ScriptReference/) - Code reference

### Community Resources

- [Unity Forums](https://forum.unity.com/) - Ask questions and get help
- [r/Unity3D](https://www.reddit.com/r/Unity3D/) - Reddit community
- [Brackeys YouTube](https://www.youtube.com/user/Brackeys) - Excellent tutorials
- [Sebastian Lague](https://www.youtube.com/c/SebastianLague) - Advanced concepts

### Game Design

- [Game Maker's Toolkit](https://www.youtube.com/c/MarkBrownGMT) - Game design analysis
- [Extra Credits](https://www.youtube.com/extracredits) - Game design concepts

---

## 🤝 Contributing

This is an educational project! Contributions that improve learning are welcome:

1. Fork the repository
2. Create a feature branch (`git checkout -b feature/amazing-tutorial`)
3. Commit your changes with clear messages
4. Push to your branch (`git push origin feature/amazing-tutorial`)
5. Open a Pull Request

### Contribution Guidelines

- Keep code educational and well-commented
- Add documentation for new features
- Follow existing code style
- Test thoroughly before submitting

---

## 📝 License

This project is licensed under the terms in the LICENSE file.

Feel free to use this project for learning, teaching, or as a starting point for your own games!

---

## 🎮 What's Next?

### Immediate Next Steps

1. ✅ Complete the Quick Start setup
2. ✅ Play around with the character movement
3. ✅ Read through the PlayerController script
4. ✅ Try the experimentation ideas at the end of each script
5. ✅ Check out the Documentation guides

### Building Your Own Game

Once you're comfortable with the basics:

1. **Plan your game** - What type of game do you want to make?
2. **Add assets** - Import models, textures, sounds from Unity Asset Store or create your own
3. **Expand mechanics** - Add new gameplay systems
4. **Create levels** - Design interesting environments
5. **Polish** - Add juice, effects, sound, and UI
6. **Test and iterate** - Playtest frequently and improve

### Need Help?

- 📖 Check the Documentation/Guides folder
- 💬 Read the comments in the scripts
- 🔍 Search Unity forums and documentation
- 🎓 Take Unity Learn courses
- 👥 Join Unity communities

---

## 🌟 Credits

This project was created as an educational resource for Unity beginners.

**Key Technologies:**
- Unity Engine
- C# Programming Language
- PhysX Physics Engine

**Made with ❤️ for learning and experimentation!**

---

Happy Game Development! 🎮✨

*Remember: Every expert was once a beginner. Keep experimenting, keep learning, and most importantly, have fun!*
