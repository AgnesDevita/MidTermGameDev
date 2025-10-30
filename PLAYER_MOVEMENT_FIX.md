# 🎮 FIX PLAYER TIDAK BISA JALAN!

## ⚡ QUICK FIX (CEPAT!)

### Langkah 1: Gunakan Auto Fix Tool
```
MidTerm Game > Fix Player Movement
Klik: ✅ FIX PLAYER (Zombie) - AUTO
```

### Langkah 2: Set Input System ⚠️ PENTING!
```
Edit > Project Settings > Player
Scroll ke bawah ke: "Other Settings"
Cari: "Active Input Handling"
Pilih: "Both" atau "Input System Package (New)"
```

**IMPORTANT:** Unity akan minta **RESTART** setelah ganti setting ini!

### Langkah 3: Test
```
Save scene
Press Play
Coba gerakin dengan WASD
```

---

## 🔍 DETAIL MASALAH

### Kenapa Player Gak Bisa Jalan?

Ada 3 kemungkinan penyebab:

#### 1. **Input System Setting Salah** ⚠️ PALING SERING!

**Masalah:**
```
Player pakai Input System (NEW)
Tapi Unity masih set ke Input Manager (OLD)
Jadinya input tidak terdeteksi!
```

**Cek:**
```
Edit > Project Settings > Player > Other Settings
Active Input Handling: ???
```

**Harus:**
- ✅ "Both" (Old and New) ← **RECOMMENDED**
- ✅ "Input System Package (New)"
- ❌ "Input Manager (Old)" ← WRONG!

**Fix:**
```
1. Ganti ke "Both"
2. Klik "Apply"
3. Restart Unity (HARUS!)
4. Open scene lagi
5. Press Play
```

---

#### 2. **Rigidbody Constraints Salah**

**Masalah:**
```
Rigidbody bisa rotate ke semua arah
Player jadi jungkir balik / ter-flip
Stuck atau goyang-goyang
```

**Cek di Inspector:**
```
Select Zombie GameObject
Lihat Rigidbody component
Constraints: ???
```

**Harus:**
```
Freeze Position: [ ] X  [ ] Y  [ ] Z
Freeze Rotation: [✓] X  [ ] Y  [✓] Z
```

Artinya: Player cuma bisa rotate Y axis (look left/right), gak bisa jungkir balik!

**Fix Auto:**
```
MidTerm Game > Fix Player Movement
Klik: 2. Fix Rigidbody Constraints
```

**Fix Manual:**
```
1. Select Zombie
2. Rigidbody component
3. Constraints > Freeze Rotation
4. Check: X dan Z
5. Uncheck: Y
```

---

#### 3. **PlayerInput Component Missing/Wrong**

**Masalah:**
```
PlayerInput component tidak ada
Atau Input Actions tidak di-assign
Input tidak masuk ke script
```

**Cek:**
```
Select Zombie
Lihat components:
- PlayerInput component ada? ✓
- Actions: InputSystem_Actions? ✓
- Action Map: Player? ✓
- Behavior: Send Messages? ✓
```

**Fix Auto:**
```
MidTerm Game > Fix Player Movement
Klik: ✅ FIX PLAYER AUTO
```

---

## 🎯 CHECKLIST LENGKAP

Pastikan semua ini OK:

### Scene Setup
- [ ] Scene: Level1.unity loaded
- [ ] GameObject: Zombie exists
- [ ] Tag: Player assigned to Zombie
- [ ] Ground: Ada floor/plane untuk jalan

### Zombie Components
- [ ] Transform ✓
- [ ] Rigidbody ✓
  - [ ] Use Gravity: ON
  - [ ] Is Kinematic: OFF
  - [ ] Constraints: Freeze Rotation X, Z
- [ ] CapsuleCollider ✓
- [ ] PlayerController ✓
- [ ] PlayerInput ✓
  - [ ] Actions: InputSystem_Actions
  - [ ] Action Map: Player
  - [ ] Behavior: Send Messages
- [ ] Camera child: Main Camera ✓

### Project Settings
- [ ] Input System installed (Package Manager)
- [ ] Input Handling: "Both" or "New"
- [ ] InputSystem_Actions.inputactions exists

### Input Actions File
- [ ] Path: Assets/UTS/MidTermGameDev/Assets/InputSystem_Actions.inputactions
- [ ] Action Map: Player
- [ ] Actions:
  - [ ] Move (WASD)
  - [ ] Look (Mouse)
  - [ ] Sprint (Shift)
  - [ ] Attack (Mouse Left)

---

## 🎮 INPUT CONTROLS

Default controls yang harus work:

```
WASD      → Move player
Mouse     → Look around
Shift     → Sprint/Run
Esc       → Pause menu
Left Click → Attack (if implemented)
```

---

## 🐛 COMMON ERRORS & FIX

### Error 1: "Player jalan tapi rotate weird"

**Penyebab:** Rigidbody constraints salah

**Fix:**
```
Rigidbody > Constraints
Freeze Rotation: X dan Z (Y bebas)
```

---

### Error 2: "Player stuck di tempat"

**Penyebab:**
- Rigidbody is Kinematic ON
- Atau Input System setting salah
- Atau mass terlalu besar

**Fix:**
```
1. Rigidbody > Is Kinematic: OFF
2. Rigidbody > Mass: 1
3. Check Input System settings
```

---

### Error 3: "Input tidak respond"

**Penyebab:** Input System setting salah!

**Fix:**
```
Edit > Project Settings > Player
Active Input Handling: "Both"
Restart Unity!
```

---

### Error 4: "Player jalan lambat banget"

**Penyebab:** Speed settings terlalu kecil

**Fix:**
```
Select Zombie
PlayerController component
Move Speed: 200 (default)
Run Speed: 250 (default)
```

Kalau masih lambat, naikin values nya!

---

### Error 5: "Player jungkir balik pas jalan"

**Penyebab:** Rotation constraints tidak frozen

**Fix:**
```
MidTerm Game > Fix Player Movement
Klik: 2. Fix Rigidbody Constraints
```

---

## 📋 VERIFICATION STEPS

Test player movement:

### Test 1: Basic Movement
```
1. Press Play
2. Press W → Player gerak maju? ✓
3. Press S → Player gerak mundur? ✓
4. Press A → Player gerak kiri? ✓
5. Press D → Player gerak kanan? ✓
```

### Test 2: Camera Look
```
1. Press Play
2. Gerakin mouse kiri-kanan → Camera rotate? ✓
3. Gerakin mouse atas-bawah → Camera look up/down? ✓
```

### Test 3: Sprint
```
1. Press Play
2. Hold Shift + W → Jalan lebih cepat? ✓
```

### Test 4: Rotation
```
1. Press Play
2. Jalan sambil mouse look → Player rotate smooth? ✓
3. Player tidak jungkir balik? ✓
```

---

## 🚀 FINAL CHECK

Before playing:

1. **Scene Saved?** ✓
2. **Input System = Both?** ✓
3. **Zombie has all components?** ✓
4. **Ground exists?** ✓
5. **No red errors in Console?** ✓

If all ✓ → **READY TO PLAY!** 🎮

---

## 💡 PRO TIPS

### Tip 1: Test in Editor
Kalau movement masih aneh, coba adjust values di Inspector **saat Play mode**:
```
Play mode ON
Select Zombie
PlayerController > Move Speed
Coba ganti-ganti nilai sambil jalan
Kalau udah pas, Stop Play dan set ulang nilai yang bagus
```

### Tip 2: Debug Movement
Tambah ini di Console untuk test input:
```
MidTerm Game > Fix Player Movement
Klik: 3. Verify PlayerInput Component
```

### Tip 3: Camera Problems?
Kalau camera aneh:
```
Select: Zombie/Main Camera
Check:
- Position: (0, 1.5, -3) atau sesuai
- Camera component ada
- Tag: MainCamera
```

---

## 🆘 MASIH GAK BISA JALAN?

Coba ini step-by-step:

### Step 1: Clean Restart
```
1. Save scene
2. Close Unity
3. Buka lagi
4. Open scene Level1
5. Press Play
```

### Step 2: Rebuild Input Actions
```
1. Select: InputSystem_Actions.inputactions
2. Inspector > "Edit Asset"
3. Klik: "Generate C# Class" (if available)
4. Klik: "Save Asset"
5. Close window
```

### Step 3: Verify Everything
```
MidTerm Game > Fix Player Movement
Test all 3 checks
```

### Step 4: Auto Fix
```
MidTerm Game > Fix Player Movement
Klik: ✅ FIX PLAYER AUTO
Restart Unity
```

---

## ✅ SUCCESS CHECKLIST

Kalau player bisa jalan, kamu harus lihat:

- ✅ Player bergerak smooth dengan WASD
- ✅ Camera follow player
- ✅ Mouse look berfungsi
- ✅ Sprint (Shift) lebih cepat
- ✅ Player tidak jungkir balik
- ✅ Collision dengan dinding works
- ✅ Gravity works (player gak melayang)

---

**🎉 GOOD LUCK! PLAYER KAMU HARUSNYA BISA JALAN SEKARANG!**

*Fix Player Movement Guide - MidTerm Game Dev*
*Unity 6 + New Input System*
