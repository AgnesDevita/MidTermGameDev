# 🎮 LEVEL PROGRESSION SYSTEM - 100% AUTOMATIC!

## ✅ ZERO SETUP - SEMUA OTOMATIS!

---

## 🎯 Level Configuration (Auto-Detect)

### **LEVEL 1**
```
Scene Name: Level1
Target: 10 diamonds
GunBot: 1x difficulty (normal)
- Patrol Speed: 150
- Chase Speed: 300
- Detection: 500
- Damage: 10
- Attack Speed: 1.5s
```

### **LEVEL 2**
```
Scene Name: Level2
Target: 20 diamonds (+10 dari Level 1)
GunBot: 2x AGGRESSIVE!
- Patrol Speed: 300 (2x) ✅
- Chase Speed: 600 (2x) ✅
- Detection: 1000 (2x) ✅
- Damage: 20 (2x) ✅
- Attack Speed: 0.75s (2x faster) ✅
```

---

## 🚀 How It Works (100% AUTOMATIC!)

### **Auto-Detection System:**

```csharp
LevelConfig.Awake()
↓
1. Detect scene name
   - Contains "Level1" → Level 1 config
   - Contains "Level2" → Level 2 config
↓
2. Set diamond count
   - Level 1: 10 diamonds
   - Level 2: 20 diamonds
↓
3. Set GunBot multipliers
   - Level 1: 1x (normal)
   - Level 2: 2x (aggressive)
↓
4. Apply to all systems
   - DiamondSpawner.totalDiamonds
   - GameManager.totalDiamonds
   - GunBotAI[] settings
↓
5. Done! Play!
```

---

## 📊 Technical Implementation

### **File: LevelConfig.cs (NEW!)**

#### **Auto-Detection:**
```csharp
void Awake()
{
    AutoDetectLevel();      // Detect Level 1 or 2
    ApplyLevelConfiguration();  // Apply settings
}

void AutoDetectLevel()
{
    string sceneName = SceneManager.GetActiveScene().name;
    
    if (sceneName.Contains("Level1"))
    {
        levelNumber = 1;
        diamondCount = 10;
        gunBotSpeedMultiplier = 1f;
        gunBotDetectionMultiplier = 1f;
        gunBotDamageMultiplier = 1f;
        gunBotAttackSpeedMultiplier = 1f;
    }
    else if (sceneName.Contains("Level2"))
    {
        levelNumber = 2;
        diamondCount = 20;
        gunBotSpeedMultiplier = 2f;         // 2x speed!
        gunBotDetectionMultiplier = 2f;     // 2x detection!
        gunBotDamageMultiplier = 2f;        // 2x damage!
        gunBotAttackSpeedMultiplier = 2f;   // 2x attack speed!
    }
}
```

#### **Auto-Configuration:**
```csharp
void ApplyLevelConfiguration()
{
    // Configure DiamondSpawner
    DiamondSpawner spawner = FindFirstObjectByType<DiamondSpawner>();
    if (spawner != null)
    {
        spawner.totalDiamonds = diamondCount;
    }
    
    // Configure GameManager
    GameManager gameManager = FindFirstObjectByType<GameManager>();
    if (gameManager != null)
    {
        gameManager.totalDiamonds = diamondCount;
    }
    
    // Configure ALL GunBots in scene
    GunBotAI[] gunBots = FindObjectsByType<GunBotAI>();
    foreach (GunBotAI bot in gunBots)
    {
        bot.patrolSpeed *= gunBotSpeedMultiplier;      // Multiply!
        bot.chaseSpeed *= gunBotSpeedMultiplier;       // Multiply!
        bot.detectionRadius *= gunBotDetectionMultiplier;
        bot.losePlayerRadius *= gunBotDetectionMultiplier;
        bot.attackDamage = (int)(bot.attackDamage * gunBotDamageMultiplier);
        bot.attackCooldown /= gunBotAttackSpeedMultiplier;  // Divide = faster!
    }
}
```

---

## 🎮 Gameplay Comparison

### **LEVEL 1 (Easy):**
```
Diamonds: 10
GunBot Behavior:
  - Slow patrol (150 speed)
  - Slow chase (300 speed)
  - Normal detection (500 range)
  - Low damage (10 per hit)
  - Slow attacks (1.5s cooldown)
  
Result: MANAGEABLE ✅
```

### **LEVEL 2 (Hard):**
```
Diamonds: 20 (DOUBLE!)
GunBot Behavior:
  - FAST patrol (300 speed) = 2x!
  - FAST chase (600 speed) = 2x!
  - LONG detection (1000 range) = 2x!
  - HIGH damage (20 per hit) = 2x!
  - RAPID attacks (0.75s cooldown) = 2x faster!
  
Result: CHALLENGING! 🔥
```

---

## 🔧 Auto-Setup v6.0

### **New Step 3: Create LevelConfig**

```csharp
Step 3: Creating LevelConfig...
↓
Detect scene name:
  - "Level1" found → Level 1 config
  - "Level2" found → Level 2 config
↓
Create _LevelConfig GameObject
Add LevelConfig component
Set properties based on level
↓
✅ LevelConfig created for LEVEL X: Y diamonds, GunBot Zx
```

### **Auto-Setup Sequence (v6.0):**
```
Step 1: Add Diamond scripts (static items)
Step 2: Setup DiamondSpawner (random spawn)
Step 3: Create LevelConfig (auto-detect level) ← NEW!
Step 4: Create GameManager
Step 5: Create Game UI
Step 6: Create Health UI
Step 7: Add PlayerHealth
Step 8: Link references
Step 9: Verify Zombie tag

✅ DONE! LEVEL AUTO-CONFIGURED!
```

---

## 📋 Level Setup Checklist

### **LEVEL 1 (Current):**
- [x] Scene: Level1.unity exists
- [x] Auto-setup v6.0 runs
- [x] LevelConfig detects "Level1"
- [x] 10 diamonds configured
- [x] GunBot 1x difficulty
- [x] READY TO PLAY!

### **LEVEL 2 (To Create):**

**Option 1: Duplicate Level1 (Recommended)**
```
1. In Project window
2. Right-click Level1.unity
3. Duplicate
4. Rename to "Level2.unity"
5. Double-click to open
6. Auto-setup v6.0 runs automatically!
7. LevelConfig detects "Level2"
8. 20 diamonds + 2x GunBot configured!
9. Save scene
10. DONE! ✅
```

**Option 2: From Scratch**
```
1. Create new scene
2. Save as "Level2.unity"
3. Add Plane, Zombie, GunBot, DiamondSpawner, etc.
4. Auto-setup v6.0 runs
5. Done!
```

---

## 🎯 Testing

### **Test Level 1:**
```
1. Open Level1.unity
2. Press Play
3. Console: "[LevelConfig] Level 1 detected: 10 diamonds, GunBot 1x aggressive"
4. Check:
   - Diamond counter: 0/10
   - GunBot speed: Normal
   - Collect all 10 diamonds
   - WIN!
```

### **Test Level 2:**
```
1. Open Level2.unity
2. Press Play
3. Console: "[LevelConfig] Level 2 detected: 20 diamonds, GunBot 2x aggressive"
4. Check:
   - Diamond counter: 0/20
   - GunBot speed: FAST! (2x)
   - GunBot detection: FAR! (2x)
   - GunBot damage: HIGH! (2x)
   - Attack speed: RAPID! (2x)
   - Collect all 20 diamonds
   - WIN!
```

---

## 🔍 Verification

### **After Auto-Setup v6.0:**

**Check Hierarchy:**
- [x] _LevelConfig GameObject exists
- [x] LevelConfig component attached

**Check Console:**
```
Level 1:
"[LevelConfig] Level 1 detected: 10 diamonds, GunBot 1x aggressive"
"[LevelConfig] DiamondSpawner configured: 10 diamonds"
"[LevelConfig] GameManager configured: 10 diamonds target"
"[LevelConfig] GunBot configured: Speed 1x, Detection 1x, Damage 1x"

Level 2:
"[LevelConfig] Level 2 detected: 20 diamonds, GunBot 2x aggressive"
"[LevelConfig] DiamondSpawner configured: 20 diamonds"
"[LevelConfig] GameManager configured: 20 diamonds target"
"[LevelConfig] GunBot configured: Speed 2x, Detection 2x, Damage 2x"
```

**Check Inspector (_LevelConfig):**
```
Level 1:
  Level Number: 1
  Diamond Count: 10
  GunBot Speed Multiplier: 1
  GunBot Detection Multiplier: 1
  GunBot Damage Multiplier: 1
  GunBot Attack Speed Multiplier: 1

Level 2:
  Level Number: 2
  Diamond Count: 20
  GunBot Speed Multiplier: 2
  GunBot Detection Multiplier: 2
  GunBot Damage Multiplier: 2
  GunBot Attack Speed Multiplier: 2
```

**Check GunBot (Inspector):**
```
Level 1:
  Patrol Speed: 150
  Chase Speed: 300
  Detection Radius: 500
  Attack Damage: 10
  Attack Cooldown: 1.5

Level 2 (After LevelConfig applies):
  Patrol Speed: 300 (150 * 2)
  Chase Speed: 600 (300 * 2)
  Detection Radius: 1000 (500 * 2)
  Attack Damage: 20 (10 * 2)
  Attack Cooldown: 0.75 (1.5 / 2)
```

---

## 📝 Manual Tweaks (Optional)

### **Adjust Difficulty:**

**Make Level 2 HARDER:**
```csharp
LevelConfig (Inspector):
  Gun Bot Speed Multiplier: 3       // 3x speed!
  Gun Bot Damage Multiplier: 3      // 3x damage!
```

**Make Level 2 EASIER:**
```csharp
LevelConfig (Inspector):
  Gun Bot Speed Multiplier: 1.5     // 1.5x speed
  Gun Bot Damage Multiplier: 1.5    // 1.5x damage
```

**More Diamonds:**
```csharp
LevelConfig (Inspector):
  Diamond Count: 30                 // Level 2 = 30 diamonds
```

---

## 🎉 SUMMARY

**LEVEL SYSTEM NOW:**
- ✅ **AUTO-DETECT** scene name (Level1 vs Level2)
- ✅ **AUTO-CONFIGURE** diamond count per level
- ✅ **AUTO-CONFIGURE** GunBot difficulty per level
- ✅ **ZERO MANUAL SETUP** required
- ✅ **100% AUTOMATIC** on scene load

**LEVEL 1:**
```
10 diamonds
GunBot 1x (normal)
```

**LEVEL 2:**
```
20 diamonds (+10)
GunBot 2x AGGRESSIVE!
  - 2x speed
  - 2x detection
  - 2x damage
  - 2x attack rate
```

**WORKFLOW:**
```
1. Duplicate Level1 → Level2
2. Rename scene to "Level2"
3. Open scene
4. Auto-setup v6.0 runs
5. LevelConfig detects & configures
6. Save
7. PLAY!

ZERO MANUAL WORK! ✅
```

**TO CREATE LEVEL 2:**
```
1. Right-click Level1.unity → Duplicate
2. Rename to "Level2.unity"
3. Double-click to open
4. Wait for auto-setup
5. Save (Ctrl+S)
6. DONE! 🎮
```

**AUTO-SETUP v6.0 - LEVEL PROGRESSION!** 🎯✨

**100% OTOMATIS, GAK PERLU SETUP APAPUN!** 🚀
