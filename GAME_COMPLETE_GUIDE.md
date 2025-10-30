# 🎮 COMPLETE GAME SETUP & DOCUMENTATION

## 📋 TABLE OF CONTENTS
1. [Game Overview](#game-overview)
2. [Quick Start](#quick-start)
3. [Complete Setup Guide](#complete-setup-guide)
4. [Scripts Documentation](#scripts-documentation)
5. [Troubleshooting](#troubleshooting)

---

## 🎯 GAME OVERVIEW

**Game Type:** 3D Third-Person Survival/Collection Game

**Objective:** 
- Control a Zombie character
- Collect all diamonds in the level
- Avoid or survive GunBot enemies
- Complete levels to progress

**Core Mechanics:**
- WASD movement with Sprint (Shift)
- Third-person camera with mouse look
- Health system with damage feedback
- AI enemies with patrol, chase, and attack behaviors
- Diamond collection system
- Score and progress tracking
- Pause menu and game over/win screens

---

## ⚡ QUICK START

### Fastest Way to Get Running:

1. **Open Scene**: `Assets/UTS/MidTermGameDev/Assets/Scenes/MainMenuLama.unity`

2. **Open Quick Setup Menu**: 
   - Top Menu: `MidTerm Game > Quick Setup Menu`

3. **For Level 1**:
   - Create new scene or open existing Level1
   - Add Room model
   - Bake NavMesh (Window > AI > Navigation)
   - Use Quick Setup Menu to add all components

4. **Play!**

---

## 🚀 COMPLETE SETUP GUIDE

### STEP 1: PROJECT SETUP

#### A. Build Settings
```
File > Build Settings
Add Scenes:
  0. MainMenu (or MainMenuLama)
  1. Level1
```

#### B. Tags Setup
```
Tags & Layers > Tags:
  - Player
  - Finish
  - GameController
  - hero
```

#### C. Layers Setup
```
Tags & Layers > Layers:
  - Level (untuk ground/walls)
  - SpawnArea (untuk spawn zones)
```

#### D. Input System
Project sudah include Input System Package.
File: `InputSystem_Actions.inputactions`

Controls:
- Move: WASD / Arrows
- Look: Mouse
- Sprint: Left Shift
- Jump: Space (optional)
- Attack: Left Mouse (optional)

---

### STEP 2: LEVEL SETUP

#### A. Create Level Scene
1. Create new scene: `Level1.unity`
2. Save to: `Assets/UTS/MidTermGameDev/Assets/Scenes/`

#### B. Add Environment
1. Add Room model: `Assets/Asset/Room.glb`
2. Position at (0, 0, 0)
3. Add Directional Light
4. Setup Ground Collision

#### C. NavMesh Baking
```
1. Select Room/Ground objects
2. Mark as "Navigation Static"
3. Window > AI > Navigation
4. Bake tab:
   - Agent Radius: 0.5
   - Agent Height: 2
   - Max Slope: 45
5. Click "Bake"
```

---

### STEP 3: PLAYER SETUP

#### Method 1: Quick Setup (RECOMMENDED)
```
1. Add Zombie.fbx to scene
2. MidTerm Game > Quick Setup Menu
3. Select Zombie in hierarchy
4. Click "Setup Selected as Player"
```

#### Method 2: Manual Setup
```
1. Add Zombie.fbx model to scene
2. Tag: "Player"
3. Components:
   - Rigidbody
     * Use Gravity: ✓
     * Is Kinematic: ✗
     * Constraints: Freeze Rotation X, Z
   - CapsuleCollider
     * Height: 2
     * Radius: 0.5
   - PlayerController (script)
     * Move Speed: 5
     * Run Speed: 9
     * Rotation Speed: 10
   - PlayerHealth (script)
     * Max Health: 100
   - PlayerInputActions (optional)
```

#### Player Camera Setup
Option 1: Child Camera
```
1. Create Camera as child of Zombie
2. Position: (0, 1.5, -3)
3. Rotation: (15, 0, 0)
4. PlayerController will handle rotation
```

Option 2: Third Person Camera
```
1. MidTerm Game > Quick Setup Menu
2. Click "Create Third Person Camera"
3. Adjust settings in Inspector
```

---

### STEP 4: ENEMY SETUP (GunBot)

#### Quick Setup
```
1. Add gun-bot_with_walk_and_idle_animation.glb to scene
2. Scale: (0.01, 0.01, 0.01) ⚠️ IMPORTANT!
3. MidTerm Game > Quick Setup Menu
4. Select GunBot
5. Click "Setup Selected as GunBot"
```

#### Manual Setup
```
1. Add model, scale to 0.01
2. Components:
   - NavMeshAgent
     * Speed: 150 (will be set by script)
     * Angular Speed: 120
     * Stopping Distance: 0.5
   - CapsuleCollider
   - GunBotAI (script)
     * Detection Radius: 500
     * Lose Player Radius: 600
     * Attack Range: 80
     * Patrol Speed: 150
     * Chase Speed: 300
     * Attack Damage: 10
     * Attack Cooldown: 1.5
   - Animator (optional)
```

#### Patrol Points
```
1. Create empty GameObject "Points"
2. Create children: Point1, Point2, Point3, Point4
3. Position around the map
4. GunBot will auto-detect them
```

OR use Quick Setup:
```
MidTerm Game > Quick Setup Menu
Click "Create Patrol Points System"
```

---

### STEP 5: DIAMOND SYSTEM

#### A. Create Diamond Prefab
```
Manual:
1. Assets/Asset/Diamond_ori.glb
2. Scale: (3, 3, 3)
3. Add Component: Diamond (script)
4. Add Component: BoxCollider
   - Is Trigger: ✓
5. Create Prefab in Assets/Prefabs/

OR Quick:
MidTerm Game > Quick Setup Menu
Click "Fix Diamond Prefab"
```

#### B. Create Diamond Spawner
```
Quick Setup:
1. MidTerm Game > Quick Setup Menu
2. Click "Create Diamond Spawner"
3. Assign Diamond Prefab
4. Create Spawn Areas (see below)

Manual:
1. Create empty "DiamondSpawner"
2. Add Component: DiamondSpawner
   - Diamond Prefab: [assign]
   - Total Diamonds: 10
   - Auto Spawn On Start: ✓
   - Enable Respawn: ✗
   - Min Spacing: 2
   - Ground Mask: Level
   - Use NavMesh Check: ✓
```

#### C. Create Spawn Areas
```
Method 1: Add to rooms
1. Select room/area GameObject
2. MidTerm Game > Quick Setup Menu
3. Click "Setup Selected as Spawn Area"
4. Drag to DiamondSpawner's "Spawn Areas" list

Method 2: Create boxes
1. Create empty GameObject "SpawnArea1"
2. Add BoxCollider
   - Is Trigger: ✓
   - Size: cover desired spawn area
3. Layer: "SpawnArea"
4. Add to DiamondSpawner list
```

---

### STEP 6: GAME MANAGER SETUP

#### Quick Setup
```
MidTerm Game > Quick Setup Menu
Click "Create Game Manager GameObject"
Click "Create Level Config GameObject"
```

#### Manual Setup
```
1. Create empty "GameManager"
2. Add GameManager script:
   - Total Diamonds: 10
   - Auto Win On Complete: ✓
3. Add LevelConfig script:
   - Level Number: 1
   - Diamond Count: 10
```

---

### STEP 7: UI SETUP

#### A. Canvas
```
1. Create UI > Canvas
2. Canvas Scaler:
   - UI Scale Mode: Scale With Screen Size
   - Reference Resolution: 1920x1080
```

#### B. Game HUD
```
1. Create empty "GameUI" under Canvas
2. Add GameUI script
3. Create children:
   - LevelText (TextMeshPro)
   - ScoreText (TextMeshPro)
   - DiamondCountText (TextMeshPro)
4. Link to GameUI script
```

#### C. Health UI
```
1. Create empty "HealthUI" under Canvas
2. Add HealthUI script
3. Create Slider "HealthBar":
   - Add child Image "Fill"
4. Create TextMeshPro "HealthText"
5. Link to HealthUI script
```

#### D. Win Panel
```
1. Create UI > Panel "WinPanel"
2. Set inactive
3. Add WinLosePanel script:
   - Is Win Panel: ✓
4. Add children:
   - Title (TextMeshPro): "VICTORY!"
   - Score (TextMeshPro)
   - Message (TextMeshPro)
   - Button "Restart" → WinLosePanel.OnRestart()
   - Button "Main Menu" → WinLosePanel.OnMainMenu()
   - Button "Next Level" → WinLosePanel.OnNextLevel()
5. Link to GameManager.winPanel
```

#### E. Game Over Panel
```
Same as Win Panel but:
- Name: "GameOverPanel"
- Is Win Panel: ✗
- Title: "GAME OVER"
- No "Next Level" button
- Link to GameManager.gameOverPanel
```

#### F. Pause Menu
```
1. Create Canvas "PauseCanvas"
2. Create Panel "PausePanel" (inactive)
3. Add buttons:
   - Resume
   - Restart
   - Main Menu
4. Create empty "PauseMenuManager"
5. Add PauseMenuManager script:
   - Pause Root: PausePanel
   - Lock Cursor In Gameplay: ✓
6. Connect buttons:
   - Resume → PauseMenuManager.OnClick_Resume()
   - Restart → PauseMenuManager.OnClick_RestartLevel()
   - Main Menu → PauseMenuManager.OnClick_MainMenu()
```

---

### STEP 8: MAIN MENU

Scene `MainMenuLama.unity` already exists with:
- ✅ Canvas with UI
- ✅ PlayButton
- ✅ ExitButton
- ✅ SceneLoader

Check button connections:
```
PlayButt > Button > OnClick():
  - PlayButton.PlayGame()

ExitButt > Button > OnClick():
  - ExitButton.ExitGame()
```

---

## 📚 SCRIPTS DOCUMENTATION

### Core Scripts

#### GameManager.cs
Main game controller.

**Public Properties:**
- `totalDiamonds` - Win condition
- `autoWinOnComplete` - Auto show win screen
- `scoreText` - UI reference
- `diamondCountText` - UI reference
- `winPanel` - Win screen
- `gameOverPanel` - Game over screen

**Public Methods:**
```csharp
CollectDiamond(int points)  // Call when diamond collected
WinGame()                   // Trigger win state
GameOver()                  // Trigger game over
RestartGame()               // Restart from Level 1
RestartLevel()              // Restart current level
LoadMainMenu()              // Return to main menu
LoadNextLevel()             // Progress to next level
```

#### PlayerController.cs
Player movement and rotation.

**Settings:**
- `moveSpeed` - Walk speed (default: 5)
- `runSpeed` - Sprint speed (default: 9)
- `rotationSpeed` - Turn speed (default: 10)
- `cameraTransform` - Camera reference

**Input:**
- Move: WASD/Arrows (auto from Input System)
- Sprint: Hold Left Shift

#### PlayerHealth.cs
Health and damage system.

**Public Properties:**
- `maxHealth` - Starting health
- `currentHealth` - Current HP
- `invincibilityDuration` - I-frames after hit

**Public Methods:**
```csharp
TakeDamage(int damage)     // Deal damage
Heal(int amount)           // Restore health
GetHealthPercentage()      // 0-1 for UI
IsDead()                   // Check death
IsInvincible()             // Check I-frames
```

#### GunBotAI.cs
Enemy AI with state machine.

**States:**
- Patrol - Walk between waypoints
- Chase - Follow player when detected
- Attack - Damage player in range

**Settings:**
- `detectionRadius` - Start chasing (500)
- `losePlayerRadius` - Stop chasing (600)
- `attackRange` - Attack distance (80)
- `patrolSpeed` - Walk speed (150)
- `chaseSpeed` - Run speed (300)
- `attackDamage` - Damage per hit (10)
- `attackCooldown` - Seconds between attacks (1.5)

#### Diamond.cs
Collectible item.

**Settings:**
- `pointValue` - Score value (10)
- `enableRotation` - Spin animation
- `enableFloating` - Bob animation
- `collectSound` - Audio on collect

#### DiamondSpawner.cs
Spawns diamonds on NavMesh.

**Settings:**
- `diamondPrefab` - What to spawn
- `totalDiamonds` - How many
- `spawnAreas` - List of BoxColliders
- `minSpacing` - Minimum distance apart
- `useNavMeshCheck` - Ensure on walkable area

**Methods:**
```csharp
[ContextMenu("Spawn Diamonds Now")]
SpawnNow()  // Clear and respawn all
```

### UI Scripts

#### GameUI.cs
Shows score, diamonds, level.

Auto-links child TextMeshPro components by name.

#### HealthUI.cs
Shows health bar with color coding.

Auto-links Slider and Image components.

#### PauseMenuManager.cs
Handles pause menu with ESC key.

**Settings:**
- `pauseRoot` - Panel to show/hide
- `lockCursorInGameplay` - Cursor mode

#### WinLosePanel.cs
Win/Lose screen controller.

**Settings:**
- `isWinPanel` - Win or lose mode
- Buttons auto-connect to game flow

### Utility Scripts

#### LevelConfig.cs
Auto-configures level difficulty.

Sets diamond count and enemy stats per level.

#### GameProgress.cs
Static class for cross-scene data.

Saves score and diamonds between scenes.

#### SceneLoader.cs
Scene management utilities.

All scene transitions go through here.

---

## 🎮 GAMEPLAY FLOW

### Main Menu
```
Start Game → MainMenuLama.unity
Player clicks "Play"
  ↓
SceneLoader.LoadLevel1()
  ↓
Level1.unity loads
```

### Level Start
```
Level1.unity loads
  ↓
LevelConfig.Awake()
  - Detect level number
  - Configure difficulty
  ↓
GameManager.Start()
  - Set diamond target
  - Load saved progress
  - Update UI
  ↓
DiamondSpawner.Start()
  - Spawn diamonds on NavMesh
  ↓
GunBotAI.Start()
  - Find player
  - Find patrol points
  - Start patrolling
  ↓
Game begins!
```

### During Gameplay
```
Player moves around (WASD + Shift)
  ↓
Collects diamonds
  ↓
Diamond.OnTriggerEnter()
  ↓
GameManager.CollectDiamond()
  - Increase score
  - Save progress
  - Update UI
  - Check win condition

GunBot AI Loop:
  ↓
Patrol → Detect Player → Chase → Attack
         ↓              ↓         ↓
    Out of range  Escape    PlayerHealth.TakeDamage()
         ↓              ↓         ↓
    Continue patrol  Chase    Update HealthUI
                              ↓
                         HP <= 0?
                              ↓
                      GameManager.GameOver()
```

### Win Condition
```
All diamonds collected
  ↓
GameManager.CollectDiamond()
  - diamondsCollected >= totalDiamonds
  ↓
Level 1?
  ↓ Yes          ↓ No
LoadNextLevel()  WinGame()
  ↓                ↓
Level2.unity    Show WinPanel
                Time.timeScale = 0
```

### Pause
```
Press ESC
  ↓
PauseMenuManager.Pause()
  - Time.timeScale = 0
  - Show pause panel
  - Unlock cursor
  ↓
Resume / Restart / Main Menu
```

---

## 🔧 EDITOR TOOLS

### Quick Setup Menu
```
MidTerm Game > Quick Setup Menu
```

**Features:**
- One-click component setup
- Auto-configuration
- Link references
- Create game objects
- Fix common issues

### Context Menu Actions
```
Right-click DiamondSpawner:
  - "Spawn Diamonds Now"

Tools > Diamond System:
  - "Setup Diamond Prefab"
```

---

## 🐛 TROUBLESHOOTING

### Player tidak bisa gerak
**Check:**
- [ ] PlayerController script attached
- [ ] Rigidbody not kinematic
- [ ] Input System package installed
- [ ] InputSystem_Actions assigned
- [ ] Ground has collision

**Fix:**
```csharp
// Check Console for errors
// Verify Rigidbody constraints
// Test with arrow keys AND WASD
```

### GunBot tidak bergerak
**Check:**
- [ ] NavMesh baked
- [ ] NavMeshAgent component
- [ ] GunBotAI script attached
- [ ] Scale is 0.01 (important!)
- [ ] Patrol points exist

**Fix:**
```
Window > AI > Navigation > Bake
Check GunBot radius matches NavMesh
Verify patrol points under "Points" object
```

### Diamonds tidak spawn
**Check:**
- [ ] DiamondSpawner has prefab assigned
- [ ] Spawn areas added to list
- [ ] BoxColliders on spawn areas
- [ ] NavMesh baked
- [ ] Ground layer in groundMask

**Debug:**
```
Select DiamondSpawner
Right-click > Spawn Diamonds Now
Check Console for spawn messages
View Scene Gizmos (spawn areas = blue)
```

### Camera tidak follow
**Check:**
- [ ] ThirdPersonCamera script on camera
- [ ] Target assigned to player
- [ ] Camera not child of player (if using ThirdPersonCamera)
- [ ] PlayerController.cameraTransform assigned

**Fix:**
```
Use Quick Setup Menu > Create Third Person Camera
OR
Make camera child of player (simpler)
```

### UI tidak update
**Check:**
- [ ] GameManager has UI references
- [ ] TextMeshPro components exist
- [ ] GameUI script attached
- [ ] HealthUI script attached

**Fix:**
```
UI scripts have Auto-Link feature
Just name children correctly:
  - ScoreText
  - DiamondCountText
  - HealthBar
  - HealthBarFill
```

### Win/Lose panel tidak muncul
**Check:**
- [ ] Panels assigned to GameManager
- [ ] Panels start inactive
- [ ] WinLosePanel script attached
- [ ] Buttons connected to methods

**Fix:**
```
Inspector > GameManager:
  - Win Panel: [assign WinPanel]
  - Game Over Panel: [assign GameOverPanel]

Each panel:
  - Active: ✗ (unchecked)
  - Has WinLosePanel script
```

### Build errors
**Check:**
- [ ] All scenes in Build Settings
- [ ] Scene names match strings in scripts
- [ ] Input System in Player Settings
- [ ] No missing references

**Fix:**
```
File > Build Settings
Add scenes in order
Player Settings > Active Input Handling:
  - Both (or New)
```

---

## 💡 PRO TIPS

### Development
1. **Test Level 1 directly** - Skip main menu during development
2. **Use Scene view Gizmos** - Visualize ranges and areas
3. **Console is your friend** - Scripts log useful debug info
4. **Prefabs for reusability** - Make prefabs for repeated objects

### Performance
1. **Bake NavMesh once** - Don't bake during play mode
2. **Pool diamonds** - For respawning (advanced)
3. **Optimize AI update** - GunBot already optimized
4. **Use occlusion culling** - For large levels

### Level Design
1. **Test spawn areas** - Use Gizmos to verify
2. **Balance difficulty** - Adjust in LevelConfig
3. **Clear paths** - Ensure NavMesh connectivity
4. **Strategic diamonds** - Place in interesting locations

### Polish
1. **Add sounds** - Assign audio clips in Inspector
2. **Particle effects** - On diamond collect
3. **Animation** - Player and enemy animations
4. **Post-processing** - Use URP volume

---

## 🎯 CHECKLIST SEBELUM BUILD

### Scene Setup
- [ ] MainMenu scene exists
- [ ] Level1 scene complete
- [ ] Both in Build Settings

### Player
- [ ] Zombie tagged "Player"
- [ ] PlayerController working
- [ ] PlayerHealth working
- [ ] Camera follows

### Enemy
- [ ] GunBot scaled correctly (0.01)
- [ ] NavMeshAgent configured
- [ ] GunBotAI working
- [ ] Patrol points positioned

### Diamond System
- [ ] Diamond prefab setup
- [ ] DiamondSpawner configured
- [ ] Spawn areas created
- [ ] Diamonds spawn correctly

### Game Flow
- [ ] GameManager configured
- [ ] LevelConfig exists
- [ ] Win condition works
- [ ] Game Over works
- [ ] Restart works
- [ ] Main Menu works

### UI
- [ ] Game HUD visible
- [ ] Health bar updates
- [ ] Score updates
- [ ] Win panel complete
- [ ] Game Over panel complete
- [ ] Pause menu works

### Polish
- [ ] No Console errors
- [ ] All references assigned
- [ ] Tags and Layers correct
- [ ] Input System working
- [ ] Build Settings correct

---

## 📞 SUPPORT

Jika masih ada masalah:

1. Check Console untuk error messages
2. Review checklist di atas
3. Compare dengan working scene
4. Test individual components
5. Rebuild dari scratch jika perlu

**Remember:** All scripts are ready to use! Just add components and configure settings.

---

**Happy Game Development! 🚀🎮**

Made with ❤️ for MidTerm Game Dev Project
