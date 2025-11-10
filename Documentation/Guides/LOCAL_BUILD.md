# Local Build Guide 🔨

Build your Unity project directly on your Mac without using GitHub Actions or any cloud services!

## Quick Start

### Prerequisites

- **macOS** (any recent version)
- **Unity installed** (preferably 2022.3.10f1 or newer)
- **Terminal** (built into macOS)

### Build in 3 Steps

1. **Open Terminal**
   - Applications → Utilities → Terminal
   - Or: Cmd+Space, type "Terminal", press Enter

2. **Navigate to your project**
   ```bash
   cd /path/to/unitity-test1
   ```

3. **Run the build script**
   ```bash
   ./build-local.sh
   ```

That's it! The script will:
- ✅ Find your Unity installation automatically
- ✅ Verify your project
- ✅ Build your game
- ✅ Create a `.app` file you can run

---

## Detailed Instructions

### First Time Setup

If this is your first time running the script, make sure it's executable:

```bash
chmod +x build-local.sh
```

You only need to do this once!

### Running the Build

From your project directory:

```bash
./build-local.sh
```

**What you'll see:**
1. Script searches for Unity installation
2. Verifies your project structure
3. Creates build directory
4. Runs Unity build (takes 5-15 minutes)
5. Verifies the build succeeded
6. Shows you where to find your game!

### Build Output

Your built game will be here:
```
Builds/macOS/ZeldaLikeStarter.app
```

**To run it:**
- Double-click `ZeldaLikeStarter.app` in Finder
- Or from terminal: `open Builds/macOS/ZeldaLikeStarter.app`

---

## Customization

### Change Build Settings

Edit the top of `build-local.sh`:

```bash
# Where to save builds
BUILD_PATH="$PROJECT_PATH/Builds/macOS"

# Name of the .app file
BUILD_NAME="ZeldaLikeStarter"

# Unity version to use
UNITY_VERSION="2022.3.10f1"
```

### Use Custom Unity Installation

If Unity is installed in a non-standard location:

```bash
UNITY_PATH="/path/to/Unity" ./build-local.sh
```

### Build to Different Location

Change the build path temporarily:

```bash
# Edit the script or modify BUILD_PATH variable
BUILD_PATH="/Users/yourname/Desktop/MyBuilds" ./build-local.sh
```

---

## Troubleshooting

### "Unity not found"

**Problem:** Script can't find your Unity installation

**Solutions:**
1. **Install Unity Hub**: Download from [unity.com](https://unity.com/download)
2. **Install Unity 2022.3.10f1** (or newer) via Unity Hub
3. **Specify path manually**:
   ```bash
   UNITY_PATH="/Applications/Unity/Hub/Editor/2022.3.10f1/Unity.app/Contents/MacOS/Unity" ./build-local.sh
   ```

### "License error"

**Problem:** Unity license not activated

**Solutions:**
1. Open Unity Editor
2. File → Open → Select your project
3. Unity will prompt you to activate license
4. Choose "Personal" (free) or enter your license
5. Try build script again

### "Assets not found"

**Problem:** Running script from wrong directory

**Solution:**
```bash
# Make sure you're in the project root
cd /path/to/unitity-test1

# Verify you're in the right place
ls -la  # Should see Assets/, ProjectSettings/, etc.

# Run script
./build-local.sh
```

### Build is Very Slow

**Normal:** First build takes 15-30 minutes

**Tips to speed up:**
- Close other applications
- Ensure Mac isn't running on battery saver mode
- Subsequent builds are faster (5-10 minutes)
- Consider building only when needed

### Build Fails with Errors

**Check the log output:** Script shows Unity's build log

**Common issues:**
- Missing scripts or assets
- Syntax errors in C# code
- Incorrect project settings

**Solution:**
1. Open project in Unity Editor
2. Fix any errors shown in Console
3. Try building in Unity Editor first (File → Build Settings → Build)
4. Once Editor build works, script will work too

### Permission Denied

**Problem:** Script not executable

**Solution:**
```bash
chmod +x build-local.sh
```

---

## Advanced Usage

### Build from Anywhere

Add this alias to your `~/.zshrc` or `~/.bash_profile`:

```bash
alias unity-build='cd /path/to/unitity-test1 && ./build-local.sh'
```

Now you can run `unity-build` from anywhere!

### Automated Builds

Create a cron job to build automatically:

```bash
# Edit crontab
crontab -e

# Add line to build every day at 2 AM
0 2 * * * cd /path/to/unitity-test1 && ./build-local.sh
```

### Build Multiple Projects

Copy `build-local.sh` to other Unity projects and customize the variables!

---

## Comparison: Local vs CI/CD

### Local Build (this script)

**Pros:**
- ✅ Free (no GitHub Actions minutes)
- ✅ Fast (no upload/download)
- ✅ Private (runs on your machine)
- ✅ Easy to debug

**Cons:**
- ❌ Requires your Mac
- ❌ Ties up your machine during build
- ❌ Manual process

### CI/CD Build (GitHub Actions)

**Pros:**
- ✅ Automatic (builds on push)
- ✅ Cloud-based (runs anywhere)
- ✅ Multiple platforms (Windows, Linux, etc.)
- ✅ Doesn't tie up your machine

**Cons:**
- ❌ Uses GitHub Actions minutes (10x for macOS)
- ❌ Slower (upload, build, download)
- ❌ Requires setup (secrets, license)

**Recommendation:**
- Use **local builds** for daily development
- Use **CI/CD** for releases and testing on other platforms

---

## Distribution

### Sharing Your Game

Once built, you can share your game:

1. **Zip the .app file:**
   ```bash
   cd Builds/macOS
   zip -r ZeldaLikeStarter.zip ZeldaLikeStarter.app
   ```

2. **Share the .zip file:**
   - Email it
   - Upload to cloud storage
   - Host on your website
   - Upload to itch.io or Steam

3. **Recipients can:**
   - Extract the .zip
   - Double-click the .app
   - Play!

### Code Signing (Optional)

For wider distribution, consider code signing:

```bash
# Sign with your Apple Developer certificate
codesign --force --deep --sign "Developer ID Application: Your Name" Builds/macOS/ZeldaLikeStarter.app

# Verify signature
codesign --verify --verbose Builds/macOS/ZeldaLikeStarter.app
```

This prevents macOS Gatekeeper warnings for users.

---

## Build Size Optimization

### Reduce Build Size

Edit Unity build settings:

1. Open Unity Editor
2. File → Build Settings
3. Player Settings → Other Settings
4. Set:
   - Strip Engine Code: Enabled
   - Managed Stripping Level: High
   - API Compatibility Level: .NET Standard 2.1

This can reduce build size by 30-50%!

### Check Build Size

```bash
du -sh Builds/macOS/ZeldaLikeStarter.app
```

Typical size: 50-150 MB for this starter project

---

## Tips & Best Practices

### Before Building

- ✅ Save all scenes
- ✅ Test in Unity Editor first
- ✅ Fix all Console errors
- ✅ Close Unity Editor (avoids conflicts)

### During Build

- ⏳ Be patient (5-15 minutes)
- 💻 Keep your Mac awake
- 🔌 Stay plugged in (don't let battery die)

### After Building

- ✅ Test the .app immediately
- ✅ Check all features work
- ✅ Compare with Editor version

### Regular Maintenance

- 🗑️ Delete old builds to save space
- 📝 Keep notes on build settings that work well
- 🔄 Update Unity version periodically

---

## Keyboard Shortcuts

Make your workflow faster:

**Terminal shortcuts:**
- `Ctrl+C` - Stop build (if needed)
- `Cmd+K` - Clear terminal
- `Cmd+T` - New tab

**Quick commands:**
```bash
# Navigate to project
cd ~/path/to/project

# Build
./build-local.sh

# Test immediately
open Builds/macOS/ZeldaLikeStarter.app

# Check logs if issues
cat ~/Library/Logs/Unity/Editor.log
```

---

## Getting Help

### Script Issues

The script has detailed error messages. If you see an error:
1. Read the error message carefully
2. Check the Troubleshooting section above
3. Look at the script's built-in help at the bottom

### Unity Issues

- Check Unity Console for errors
- Search Unity forums: [forum.unity.com](https://forum.unity.com/)
- Check Unity documentation: [docs.unity3d.com](https://docs.unity3d.com/)

### Still Stuck?

- Review this guide again
- Try building in Unity Editor first
- Check that your project works in Editor
- Verify Unity is properly installed

---

## Summary

**Quick command:**
```bash
./build-local.sh
```

**Output:**
```
Builds/macOS/ZeldaLikeStarter.app
```

**Time:**
- First build: 15-30 minutes
- Subsequent: 5-10 minutes

**Requirements:**
- macOS
- Unity installed
- Terminal

That's all you need! Happy building! 🎮

---

**Pro tip:** Run this command to build and immediately test:
```bash
./build-local.sh && open Builds/macOS/ZeldaLikeStarter.app
```
