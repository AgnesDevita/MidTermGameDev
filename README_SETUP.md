# 🎮 MidTerm Game Dev - Complete Setup Guide

## ✅ SCRIPTS SUDAH SIAP SEMUA!

Semua script sudah dibuat dan siap digunakan. Berikut komponen-komponen yang sudah tersedia:

### 📂 Core Scripts
- ✅ **GameManager.cs** - Mengatur game flow, score, diamond collection
- ✅ **SceneLoader.cs** - Load scene dan transition
- ✅ **GameProgress.cs** - Simpan progress antar scene
- ✅ **LevelConfig.cs** - Konfigurasi level difficulty

### 🎯 Player Scripts
- ✅ **PlayerController.cs** - Movement dengan Input System (WASD + Sprint)
- ✅ **PlayerHealth.cs** - Health system dengan damage dan heal
- ✅ **ThirdPersonCamera.cs** - Third person camera controller

### 🤖 Enemy Scripts
- ✅ **GunBotAI.cs** - AI dengan patrol, chase, dan attack states
- Sudah support NavMesh
- Auto-detect player dan patrol points

### 💎 Diamond System
- ✅ **Diamond.cs** - Collectible item
- ✅ **DiamondSpawner.cs** - Auto spawn diamonds dengan NavMesh check
- ✅ **DiamondSystemSetup.cs** - Auto setup untuk diamond system

### 🎨 UI Scripts
- ✅ **GameUI.cs** - Score dan diamond counter
- ✅ **HealthUI.cs** - Health bar dengan color feedback
- ✅ **PauseMenuManager.cs** - Pause menu dengan ESC
- ✅ **WinLosePanel.cs** - Win/Lose screen
- ✅ **PlayButton.cs** - Main menu play button
- ✅ **ExitButton.cs** - Exit game button

### 🔧 Editor Tools
- ✅ **AutoSetupLevel1.cs** - Auto setup untuk Level 1
- ✅ **AutoSetupDiamondPrefab.cs** - Auto setup diamond prefab
- ✅ **AutoSetupDiamondSystem.cs** - Auto setup diamond spawner
- ✅ **SetupHelper.cs** - Setup guide

---

## 🚀 CARA SETUP GAME (STEP BY STEP)

### 1️⃣ BUILD SETTINGS
1. File > Build Settings
2. Add scenes:
   - MainMenu (sudah ada: MainMenuLama.unity)
   - Level1 (buat baru atau rename)
3. Close

### 2️⃣ TAGS & LAYERS
1. Tag 'Player' - untuk Zombie
2. Layer 'Level' - untuk ground/walls
3. Layer 'SpawnArea' - untuk spawn zones

### 3️⃣ SETUP LEVEL 1 SCENE

#### A. Create/Open Level1 Scene
- Bisa pakai scene yang ada atau buat baru
- Add Room.glb model dari `/Assets/UTS/MidTermGameDev/Assets/Asset/Room.glb`

#### B. NavMesh Setup
1. Select Room object
2. Mark as Navigation Static
3. Window > AI > Navigation
4. Tab "Bake"
5. Agent Radius: 0.5
6. Agent Height: 2
7. Click "Bake"

#### C. Auto Setup Components
1. Window > MidTerm Game > Auto Setup Level 1
2. Click "⭐ SETUP ALL ⭐"

Atau manual:

**ZOMBIE (Player):**
1. Drag Zombie.fbx ke scene
2. Tag: Player
3. Add Components (klik button di Auto Setup atau manual):
   - Rigidbody (Freeze Rotation X, Z)
   - CapsuleCollider
   - PlayerController
   - PlayerHealth
   - PlayerInputActions (optional)
4. Buat child Camera atau assign Main Camera

**GUNBOT (Enemy):**
1. Drag gun-bot_with_walk_and_idle_animation.glb
2. Scale: (0.01, 0.01, 0.01)
3. Add Components:
   - NavMeshAgent
   - CapsuleCollider
   - GunBotAI
   - Animator
4. GunBotAI Inspector:
   - Detection Radius: 500
   - Chase Speed: 300
   - Patrol Speed: 150
   - Attack Range: 80

**PATROL POINTS:**
1. Create Empty GameObject "Points"
2. Create child empties: "Point1", "Point2", "Point3", etc
3. Position them around the map
4. GunBot will auto-find them

**GAME MANAGER:**
1. Create Empty "GameManager"
2. Add GameManager script
3. Add LevelConfig script
4. Total Diamonds: 10

**DIAMOND SPAWNER:**
1. Create Diamond Prefab:
   - Drag Diamond_ori.glb
   - Scale: (3, 3, 3)
   - Add BoxCollider (isTrigger = true)
   - Add Diamond script
   - Save as Prefab
2. Create Empty "DiamondSpawner"
3. Add DiamondSpawner script:
   - Assign Diamond Prefab
   - Total Diamonds: 10
4. Create Spawn Areas:
   - Add BoxColliders to rooms/areas
   - Drag to "Spawn Areas" list
   - Or use Editor tool

**UI SETUP:**
1. Canvas (Screen Space Overlay)
2. Add GameUI GameObject:
   - Add GameUI script
   - Create child TextMeshPro: "ScoreText", "DiamondCountText", "LevelText"
3. Add HealthUI GameObject:
   - Add HealthUI script
   - Create Slider "HealthBar"
   - Create Image "HealthBarFill" 
   - Create TextMeshPro "HealthText"
4. Create WinPanel:
   - Panel (inactive)
   - Add WinLosePanel script (isWinPanel = true)
   - Add buttons: Restart, Main Menu, Next Level
5. Create GameOverPanel:
   - Panel (inactive)
   - Add WinLosePanel script (isWinPanel = false)
   - Add buttons: Restart, Main Menu
6. Link panels to GameManager

**PAUSE MENU:**
1. Create Canvas "PauseCanvas"
2. Create Panel "PausePanel"
3. Add buttons: Resume, Restart, Main Menu
4. Create GameObject "PauseMenuManager"
5. Add PauseMenuManager script
6. Assign pauseRoot = PausePanel

### 4️⃣ MAIN MENU SCENE (Sudah Ada)
Scene MainMenuLama.unity sudah ada dengan:
- ✅ SceneLoader component
- ✅ PlayButton (akan load Level1)
- ✅ ExitButton
- ✅ UI sudah setup

Cek saja apakah buttons sudah connect ke:
- PlayButton.PlayGame()
- ExitButton.ExitGame()

---

## 🎮 INPUT SYSTEM

File `InputSystem_Actions.inputactions` sudah ada dengan:
- Move: WASD / Arrow Keys
- Look: Mouse
- Sprint: Left Shift
- Jump: Space
- Attack: Left Click

Sudah otomatis connect ke PlayerController!

---

## 🎯 TESTING

1. Play MainMenu scene
2. Click Play
3. Should load Level 1
4. Check:
   - ✅ Player can move (WASD)
   - ✅ Player can sprint (Shift)
   - ✅ Camera follows
   - ✅ Diamonds spawn
   - ✅ GunBot patrols
   - ✅ GunBot chases when near
   - ✅ Health decreases when hit
   - ✅ Collect diamonds = score up
   - ✅ Collect all = Win screen
   - ✅ Die = Game Over screen
   - ✅ ESC = Pause menu

---

## ⚡ QUICK FIXES

### Diamond tidak spawn?
- Check DiamondSpawner Inspector
- Check spawn areas ada BoxCollider
- Check NavMesh sudah di-bake
- Check layer mask settings

### GunBot tidak bergerak?
- Check NavMesh sudah di-bake
- Check GunBot punya NavMeshAgent
- Check patrol points ada

### Player tidak bisa gerak?
- Check PlayerController enabled
- Check Rigidbody tidak kinematic
- Check Input System installed

### Camera tidak follow?
- Check Main Camera punya ThirdPersonCamera script
- Check target assigned ke Player

---

## 📚 EDITOR TOOLS

### Window > MidTerm Game
- Auto Setup Level 1 - Auto setup semua components
- Diamond System Setup (sudah ada di Tools menu juga)

### Component Menu
- Right-click GameObject
- Add Component > cari nama script

---

## 🔥 PRO TIPS

1. **Testing Faster**: Langsung play Level1 scene untuk skip Main Menu
2. **Debug Mode**: Check Console untuk log messages
3. **Gizmos**: Select GunBot/DiamondSpawner di Scene view untuk lihat ranges
4. **Prefabs**: Buat prefab untuk reusable objects
5. **NavMesh**: Rebake jika ada perubahan geometry

---

## 📞 TROUBLESHOOTING

Jika ada error:
1. Check Console untuk error message
2. Check semua references assigned
3. Check Tags dan Layers sudah benar
4. Check NavMesh sudah di-bake
5. Check Input System Package installed

---

**Happy Coding! 🚀**
