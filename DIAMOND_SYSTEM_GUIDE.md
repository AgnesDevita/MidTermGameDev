# 💎 Diamond Collection System - Complete Guide

## 📋 Overview

Sistem Diamond yang sudah dibuat untuk game Anda:
- ✅ **15 Diamonds** sudah di-spawn di scene
- ✅ **Rotation & Float Animation** untuk diamond
- ✅ **Score System** dengan UI
- ✅ **Diamond Counter** (collected/total)
- ✅ **Win Condition** saat semua diamond terkumpul
- ✅ **Auto-setup scripts** untuk kemudahan

---

## 🎯 Fitur Lengkap

### 1. **Diamond.cs** - Individual Diamond Behavior
- Rotasi otomatis (spinning effect)
- Float animation (naik-turun)
- Trigger collision untuk player
- Point value per diamond
- Audio support saat di-collect

### 2. **GameManager.cs** - Game Logic
- Track total score
- Track diamonds collected
- Win condition detection
- Game Over system
- Restart & Main Menu functions
- Singleton pattern (hanya 1 instance)

### 3. **GameUI.cs** - HUD Display
- Real-time score display
- Diamond counter (💎 5/15)
- Optional timer
- Auto-update setiap frame

### 4. **DiamondSpawner.cs** - Enhanced
- Auto-add Diamond script ke spawned diamonds
- Semua fitur spawn sebelumnya tetap ada

---

## 🚀 Quick Setup (Auto)

### Cara Paling Mudah:

1. **Buat Empty GameObject** di Hierarchy
   - Klik kanan → Create Empty
   - Rename jadi `"_DiamondSetup"`

2. **Add Component `DiamondSystemSetup`**
   - Select `_DiamondSetup`
   - Inspector → Add Component
   - Ketik "DiamondSystemSetup"

3. **Klik Setup Otomatis**
   - Klik kanan pada component **DiamondSystemSetup**
   - Pilih: **"4. Setup Complete System (All Steps)"**
   - Tunggu log di Console: `✅ Diamond System Setup Complete!`

4. **Verify Setup**
   - Check Hierarchy ada:
     - `_GameManager`
     - `GameUI_Canvas`
   - Semua diamond sudah punya script `Diamond`

5. **PLAY!** 🎮
   - Collect diamonds dengan Zombie
   - Lihat score & counter bertambah
   - Win screen saat semua collected!

---

## 🔧 Manual Setup (Detail)

### Step 1: Setup Diamonds

**Option A - Untuk Diamond yang Sudah Ada di Scene:**

1. Pilih semua Diamond di Hierarchy
   - Hold Ctrl, klik Diamond(Clone) satu-satu
   - Atau expand DiamondSpawner, pilih semua children

2. Add Component `Diamond`
   - Inspector → Add Component
   - Ketik "Diamond" → Enter

3. Set BoxCollider sebagai Trigger
   - Centang **"Is Trigger"** di BoxCollider

**Option B - Setup Diamond Prefab (Recommended):**

1. Buka Prefab Diamond
   - Project → Assets/Asset → Diamond.prefab
   - Double-click untuk edit

2. Add Component `Diamond`
   - Inspector → Add Component → Diamond

3. Set BoxCollider
   - Centang **"Is Trigger"**

4. Apply Prefab Changes
   - Inspector → Overrides → Apply All

5. Respawn Diamonds
   - Pilih DiamondSpawner
   - Inspector → Right-click Component → "Spawn Diamonds Now"

---

### Step 2: Create GameManager

1. **Create GameObject**
   ```
   Hierarchy → Right-click → Create Empty
   Rename: "_GameManager"
   ```

2. **Add GameManager Script**
   ```
   Inspector → Add Component → GameManager
   ```

3. **Configure Settings**
   ```
   Total Diamonds: 15 (auto-detect)
   Auto Win On Complete: ✅ (centang)
   ```

4. **Optional: Assign UI (nanti di Step 3)**

---

### Step 3: Create Game UI

1. **Create Canvas**
   ```
   Hierarchy → Right-click → UI → Canvas
   Rename: "GameUI_Canvas"
   ```

2. **Set Canvas Settings**
   ```
   Render Mode: Screen Space - Overlay
   Canvas Scaler: Scale With Screen Size
   Reference Resolution: 1920 x 1080
   ```

3. **Create Score Text**
   ```
   Right-click Canvas → UI → Text - TextMeshPro
   Rename: "ScoreText"
   
   RectTransform:
   - Anchor: Top-Left
   - Position X: 20, Y: -20
   - Width: 300, Height: 50
   
   TextMeshPro:
   - Text: "Score: 0"
   - Font Size: 32
   - Color: White
   - Font Style: Bold
   ```

4. **Create Diamond Counter**
   ```
   Right-click Canvas → UI → Text - TextMeshPro
   Rename: "DiamondCountText"
   
   RectTransform:
   - Anchor: Top-Left
   - Position X: 20, Y: -70
   - Width: 300, Height: 50
   
   TextMeshPro:
   - Text: "💎 0/15"
   - Font Size: 32
   - Color: Cyan
   - Font Style: Bold
   ```

5. **Add GameUI Script to Canvas**
   ```
   Select GameUI_Canvas
   Inspector → Add Component → GameUI
   
   Drag References:
   - Score Text → ScoreText
   - Diamond Count Text → DiamondCountText
   ```

---

### Step 4: Connect GameManager & UI

1. **Select _GameManager**

2. **Assign UI References**
   ```
   Score Text: drag "ScoreText" dari Hierarchy
   Diamond Count Text: drag "DiamondCountText" dari Hierarchy
   ```

3. **Optional: Add Win Panel**
   - Create Panel untuk Win Screen
   - Assign ke "Win Panel"
   - Script akan auto-show saat menang

---

## 🎮 Testing Checklist

### Pre-Play Checks:
- [ ] Zombie GameObject punya tag **"Player"**
- [ ] Diamonds punya script **Diamond**
- [ ] Diamonds BoxCollider **Is Trigger = ON**
- [ ] Ada GameObject **_GameManager** di scene
- [ ] Ada Canvas **GameUI_Canvas** dengan UI texts
- [ ] GameManager connected ke UI texts

### During Play:
- [ ] Score text muncul di kiri atas
- [ ] Diamond counter muncul di bawah score
- [ ] Diamond berputar & floating
- [ ] Zombie bisa collect diamond (touch = hilang)
- [ ] Score bertambah saat collect
- [ ] Counter bertambah (contoh: 💎 1/15)
- [ ] Console log: "Diamond collected!"

### When All Collected:
- [ ] Console log: "🎉 YOU WIN!"
- [ ] (Optional) Win panel muncul
- [ ] Game pause (Time.timeScale = 0)

---

## 🔊 Optional: Add Sound Effects

### Setup Audio:

1. **Import Sound Files**
   - Drag .mp3/.wav ke folder Assets/Audio

2. **Assign to Diamond Script**
   ```
   Select Diamond Prefab
   Inspector → Diamond Component
   Collect Sound: drag your audio clip
   ```

3. **Assign to GameManager**
   ```
   Select _GameManager
   Inspector → GameManager Component
   Win Sound: drag win audio clip
   Game Over Sound: drag gameover clip
   ```

---

## ⚙️ Customization Options

### Diamond Settings (per diamond):

```csharp
Rotation Speed: 50        // Kecepatan putar
Enable Floating: ✅       // Animasi naik-turun
Float Amplitude: 0.5      // Tinggi naik-turun
Float Speed: 1            // Kecepatan floating
Point Value: 10           // Points per diamond
```

### GameManager Settings:

```csharp
Total Diamonds: 15              // Target diamonds
Auto Win On Complete: ✅        // Auto win screen
```

### GameUI Settings:

```csharp
Show Timer: ☐                   // Show elapsed time
```

---

## 🐛 Troubleshooting

### Diamond tidak hilang saat di-touch:

**Solusi:**
1. Check Zombie tag = "Player"
2. Check Diamond BoxCollider → Is Trigger = ON
3. Check Diamond punya script `Diamond`

### Score tidak update:

**Solusi:**
1. Check ada _GameManager di scene
2. Check GameManager → Score Text assigned
3. Check Console ada error

### UI tidak muncul:

**Solusi:**
1. Check Canvas → Render Mode = Screen Space Overlay
2. Check TextMeshPro imported (Window → TextMeshPro → Import TMP Essentials)
3. Check GameUI script attached ke Canvas

### Diamond tidak berputar:

**Solusi:**
1. Check Diamond script ada
2. Check Rotation Speed > 0
3. Play Mode → select diamond → check script enabled

---

## 🎨 Advanced Features

### Add Particle Effects:

1. Create Particle System
2. Attach sebagai child of Diamond prefab
3. Atau spawn saat collected di Diamond.cs:
   ```csharp
   if (collectEffect != null)
   {
       Instantiate(collectEffect, transform.position, Quaternion.identity);
   }
   ```

### Add Distance Display:

```csharp
// Di GameUI.cs, tambahkan:
public Transform player;
public Transform nearestDiamond;

void Update()
{
    if (player && nearestDiamond)
    {
        float distance = Vector3.Distance(player.position, nearestDiamond.position);
        distanceText.text = $"Nearest: {distance:F1}m";
    }
}
```

### Add Combo System:

```csharp
// Di GameManager.cs, tambahkan:
private float lastCollectTime;
private int comboMultiplier = 1;

public void CollectDiamond(int points)
{
    if (Time.time - lastCollectTime < 3f)
    {
        comboMultiplier++;
    }
    else
    {
        comboMultiplier = 1;
    }
    
    currentScore += points * comboMultiplier;
    lastCollectTime = Time.time;
}
```

---

## 📝 Script Integration

### Panggil dari Script Lain:

```csharp
// Get score
GameManager gm = GameManager.Instance;
int score = gm.GetScore();

// Get diamonds collected
int collected = gm.GetDiamondsCollected();

// Trigger game over manually
gm.GameOver();

// Restart level
gm.RestartLevel();
```

---

## ✅ Final Checklist

Setup Complete Jika:
- [x] 4 Script baru dibuat (Diamond, GameManager, GameUI, DiamondSystemSetup)
- [x] DiamondSpawner.cs updated
- [x] Semua diamonds punya script Diamond
- [x] _GameManager di scene
- [x] GameUI_Canvas dengan score & counter
- [x] Zombie tag = "Player"
- [x] Play test berhasil collect diamond
- [x] Score & counter update
- [x] Win condition triggered saat semua collected

---

## 🎉 You're Done!

Game Anda sekarang punya:
- 💎 Full diamond collection system
- 📊 Score tracking
- 🎯 Win condition
- 🎨 Visual feedback (rotation, floating)
- 🔊 Audio support ready
- 📱 Clean UI

**Selamat bermain dan kumpulkan semua diamonds!** 🚀
