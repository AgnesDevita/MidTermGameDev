# 🎮 HOW TO CREATE LEVEL 2 - SUPER SIMPLE!

## ✅ 100% OTOMATIS - 5 LANGKAH AJA!

---

## 🚀 Quick Guide (5 Steps)

### **STEP 1: Duplicate Level 1**
```
1. In Unity Project window
2. Navigate to: /Assets/UTS/MidTermGameDev/Assets/Scenes
3. Find "Level1.unity"
4. RIGHT-CLICK → Duplicate
5. New scene created: "Level1 1.unity"
```

### **STEP 2: Rename to Level2**
```
1. Select "Level1 1.unity"
2. Press F2 (or right-click → Rename)
3. Type: "Level2"
4. Press Enter
5. Now you have "Level2.unity" ✅
```

### **STEP 3: Open Level2**
```
1. Double-click "Level2.unity"
2. Scene loads in Hierarchy
3. Wait for Unity to compile (if needed)
```

### **STEP 4: Auto-Setup Runs**
```
Unity detects new scene open
↓
Auto-Setup v6.0 runs (if needed)
↓
LevelConfig detects "Level2" in scene name
↓
Auto-configures:
  ✅ 20 diamonds (instead of 10)
  ✅ GunBot 2x speed
  ✅ GunBot 2x detection
  ✅ GunBot 2x damage
  ✅ GunBot 2x attack speed
↓
Check Console for confirmation:
"[LevelConfig] Level 2 detected: 20 diamonds, GunBot 2x aggressive"
```

### **STEP 5: Save & Test**
```
1. Press Ctrl+S (Save scene)
2. Press Play button
3. Test gameplay:
   - Counter shows: 0/20 diamonds
   - GunBot moves FASTER
   - GunBot detects from FURTHER
   - GunBot attacks FASTER
   - Much harder than Level 1!
4. DONE! ✅
```

---

## 📊 What Happens Automatically

### **When You Open Level2.unity:**

```
LevelConfig.Awake() runs at scene start
↓
Detects scene name = "Level2"
↓
Auto-applies settings:

DiamondSpawner:
  totalDiamonds = 20 (was 10)

GameManager:
  totalDiamonds = 20 (was 10)

GunBotAI (ALL bots in scene):
  patrolSpeed = 150 * 2 = 300
  chaseSpeed = 300 * 2 = 600
  detectionRadius = 500 * 2 = 1000
  losePlayerRadius = 600 * 2 = 1200
  attackDamage = 10 * 2 = 20
  attackCooldown = 1.5 / 2 = 0.75

Console Output:
"[LevelConfig] Level 2 detected: 20 diamonds, GunBot 2x aggressive"
"[LevelConfig] DiamondSpawner configured: 20 diamonds"
"[LevelConfig] GameManager configured: 20 diamonds target"
"[LevelConfig] GunBot configured: Speed 2x, Detection 2x, Damage 2x"
```

---

## ✅ Verification Checklist

### **After Opening Level2:**

**Check Hierarchy:**
- [x] _LevelConfig exists
- [x] _GameManager exists
- [x] GameUI_Canvas exists
- [x] DiamondSpawner exists
- [x] GunBot exists
- [x] Zombie exists

**Check Console:**
```
[LevelConfig] Level 2 detected: 20 diamonds, GunBot 2x aggressive
[LevelConfig] DiamondSpawner configured: 20 diamonds
[LevelConfig] GameManager configured: 20 diamonds target
[LevelConfig] GunBot configured: Speed 2x, Detection 2x, Damage 2x
```

**In Play Mode:**
- [x] UI shows "Diamonds: 0/20" (not 0/10!)
- [x] GunBot moves MUCH FASTER
- [x] GunBot sees you from FURTHER away
- [x] GunBot attacks MORE frequently
- [x] Much harder to survive!

---

## 🎯 Expected Behavior

### **LEVEL 1 vs LEVEL 2:**

| Feature | Level 1 | Level 2 | Difference |
|---------|---------|---------|------------|
| **Diamonds** | 10 | 20 | +10 |
| **Patrol Speed** | 150 | 300 | 2x faster |
| **Chase Speed** | 300 | 600 | 2x faster |
| **Detection** | 500 | 1000 | 2x range |
| **Damage** | 10 | 20 | 2x damage |
| **Attack Speed** | 1.5s | 0.75s | 2x faster |

---

## 🔧 Optional: Customize Level 2

### **Make it HARDER:**
```
1. Open Level2.unity
2. Select _LevelConfig in Hierarchy
3. Inspector:
   Gun Bot Speed Multiplier: 3        (3x speed!)
   Gun Bot Damage Multiplier: 3       (3x damage!)
4. Save
5. Play → SUPER HARD! 🔥
```

### **Make it EASIER:**
```
1. Open Level2.unity
2. Select _LevelConfig
3. Inspector:
   Gun Bot Speed Multiplier: 1.5      (1.5x speed)
   Gun Bot Damage Multiplier: 1.5     (1.5x damage)
4. Save
5. Play → More manageable
```

### **More Diamonds:**
```
1. Select _LevelConfig
2. Inspector:
   Diamond Count: 30                  (30 instead of 20)
3. Save
4. Play → More to collect!
```

---

## 🎮 Test Both Levels

### **Test Level 1:**
```
1. Open Level1.unity
2. Press Play
3. Should show: 0/10 diamonds
4. GunBot: Normal speed & behavior
5. Easy difficulty ✅
```

### **Test Level 2:**
```
1. Open Level2.unity
2. Press Play
3. Should show: 0/20 diamonds
4. GunBot: FAST & aggressive!
5. Hard difficulty 🔥
```

---

## 🎉 SUMMARY

**TO CREATE LEVEL 2:**
```
1. Duplicate Level1.unity
2. Rename to Level2.unity
3. Open Level2.unity
4. Auto-setup detects & configures
5. Save (Ctrl+S)

DONE! 🚀
```

**WHAT'S AUTO-CONFIGURED:**
- ✅ 20 diamonds (automatic!)
- ✅ GunBot 2x speed (automatic!)
- ✅ GunBot 2x detection (automatic!)
- ✅ GunBot 2x damage (automatic!)
- ✅ GunBot 2x attack rate (automatic!)

**ZERO MANUAL WORK!**
```
Just duplicate → rename → open → save
Everything else = AUTOMATIC! ✅
```

**TIME NEEDED:**
```
~1 minute total:
  10s: Duplicate scene
  5s:  Rename to Level2
  10s: Open scene
  30s: Wait for auto-setup & compile
  5s:  Save scene
  
DONE! 🎯
```

**SEKARANG BIKIN LEVEL 2 YUK!** 🎮✨
