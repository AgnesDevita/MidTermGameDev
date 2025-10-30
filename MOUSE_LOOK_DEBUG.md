# 🔍 MOUSE LOOK NOT WORKING - DEBUG GUIDE

## 🐛 **PROBLEM:**
Mouse gerak kiri/kanan tapi player/camera gak rotate!

---

## ✅ **SOLUTION STEPS:**

### **STEP 1: Check Console Logs**

Script sekarang ada logging untuk debug.

**Test:**
```
1. Press Play
2. Gerakin mouse
3. Check Console
```

**What to look for:**

**A. WASD Input Works?**
```
Console shows: "Move Input: (1, 0)" when press D?
Console shows: "Move Input: (0, 1)" when press W?
```
✅ YES → WASD input OK
❌ NO → PlayerInput component masalah!

**B. Mouse Input Works?**
```
Console shows: "Look Input Received: (X, Y)" when gerakin mouse?
```
✅ YES → Mouse input masuk!
❌ NO → Input System gak detect mouse!

**C. Rotation Calculated?**
```
Console shows: "Look Input: X=..., Y=..." when mouse gerak?
```
✅ YES → Rotation harusnya work
❌ NO → Input terlalu kecil atau sensitivity issue

---

## 🔧 **COMMON FIXES:**

### **FIX 1: Mouse Input Tidak Terdetect**

**Kemungkinan:** Input System belum enable mouse delta.

**Check Input Actions:**
```
1. Project window
2. Assets/UTS/MidTermGameDev/Assets/
3. Double-click: InputSystem_Actions.inputactions
```

**Verify "Look" Action:**
```
Action: Look
Action Type: Value
Control Type: Vector2

Bindings:
  ✅ Gamepad Right Stick
  ✅ Mouse > Delta (PENTING!)
  
Path harus: <Mouse>/delta
```

**Kalau gak ada Mouse Delta binding:**
```
1. Select "Look" action
2. Click "+" → Add Binding
3. Path: <Mouse>/delta
4. Save (Ctrl+S)
```

---

### **FIX 2: PlayerInput Component Salah**

**Check Zombie GameObject:**
```
Component: PlayerInput
  
Settings harus:
  Actions: InputSystem_Actions
  Default Map: Player
  Behavior: Send Messages (PENTING!)
  
Kalau salah:
  MidTerm Game > Fix Player Movement
  Klik: FIX PLAYER AUTO
```

---

### **FIX 3: Sensitivity Terlalu Rendah**

**Symptoms:**
- Console shows tiny numbers like "Look Input: X=0.0001"
- Mouse gerak tapi rotation hampir gak kelihatan

**Fix:**
```
Select: Zombie
Component: PlayerController

Increase sensitivity:
  Mouse Sensitivity X: 300 (dari 150)
  Mouse Sensitivity Y: 300 (dari 150)
```

---

### **FIX 4: Camera Transform Tidak Assigned**

**Check:**
```
Select: Zombie
Component: PlayerController
  
Camera Transform: ??? (harus ada!)

Kalau NULL:
  Drag "Main Camera" child ke field ini
```

---

### **FIX 5: Cursor Locked Issue**

**Symptoms:**
- Cursor kelihatan
- Cursor gak locked

**Fix:**
```csharp
// Tekan ESC untuk unlock
// Klik game window lagi untuk lock

Atau di script, Awake() sudah ada:
Cursor.lockState = CursorLockMode.Locked;
Cursor.visible = false;
```

---

## 🎯 **QUICK TEST:**

### **Test 1: Input System Active?**

**Check Project Settings:**
```
Edit > Project Settings > Player > Other Settings

Active Input Handling:
  ✅ Both (Recommended)
  ✅ Input System Package (New)
  ❌ Input Manager (Old) - WRONG!
```

**Kalau salah:**
```
Change to: Both
Restart Unity!
```

---

### **Test 2: Console Log Test**

**Expected Console Output:**

**When pressing WASD:**
```
Move Input: (1, 0)   // Press D
Move Input: (-1, 0)  // Press A
Move Input: (0, 1)   // Press W
Move Input: (0, -1)  // Press S
```

**When moving mouse:**
```
Look Input Received: (2.3, -1.5)
Look Input Received: (1.8, -0.9)
Look Input: X=2.30, Y=-1.50 | MouseX=0.0345, MouseY=-0.0225
```

**Kalau WASD OK tapi Mouse gak ada log:**
→ Input Actions "Look" binding gak ada mouse!

**Kalau semua gak ada log:**
→ PlayerInput component not working!

---

## 🛠️ **MANUAL FIX - Input Actions:**

### **Step-by-Step Fix Look Action:**

```
1. Open: InputSystem_Actions.inputactions
2. Select: "Look" action (di Action Maps > Player)
3. Check Control Type: Vector2
4. Check Bindings list:
```

**MUST HAVE:**
```
Look
├─ <Gamepad>/rightStick
└─ <Mouse>/delta          ← MUST EXIST!
```

**Kalau gak ada Mouse delta:**
```
1. Click "Look" action
2. Right panel: "+" button
3. Add Binding
4. Listen button (atau manual type)
5. Type: <Mouse>/delta
6. Save Asset (Ctrl+S)
```

---

## 📋 **CHECKLIST:**

Before asking for more help, verify:

- [ ] Console shows "Move Input" when WASD? ✓
- [ ] Console shows "Look Input Received" when mouse move? ✓
- [ ] PlayerInput behavior = Send Messages? ✓
- [ ] Input Actions has <Mouse>/delta binding? ✓
- [ ] Camera Transform assigned in PlayerController? ✓
- [ ] Mouse Sensitivity > 100? ✓
- [ ] Cursor locked (not visible in game)? ✓
- [ ] Active Input Handling = Both/New? ✓

---

## 🎮 **EXPECTED BEHAVIOR:**

### **After Fix:**

**Mouse Left/Right:**
```
→ Player rotates horizontal (Y-axis)
→ Camera follows (child of player)
→ Smooth rotation
```

**Mouse Up/Down:**
```
→ Camera tilts up/down (X-axis)
→ Player stays upright
→ Clamped -80° to +80°
```

**WASD:**
```
W → Forward (player.forward)
S → Backward
A → Strafe left
D → Strafe right

Movement follows player rotation!
```

---

## 💡 **IF STILL NOT WORKING:**

### **Last Resort Fix:**

**Re-setup Input System completely:**

```
1. MidTerm Game > Fix Player Movement
2. Click: FIX PLAYER AUTO
3. Save Scene
4. Close Unity
5. Delete: Library folder (project root)
6. Reopen Unity (akan regenerate)
7. Press Play
8. Test mouse
```

---

## 🔍 **ADVANCED DEBUG:**

### **Add More Logging:**

Tambah di HandleRotation():

```csharp
void Update()
{
    Debug.Log($"LookInput: {lookInput}, Cursor Lock: {Cursor.lockState}");
    HandleRotation();
}
```

**Expected:**
```
LookInput: (X, Y), Cursor Lock: Locked
```

**If shows:**
```
LookInput: (0, 0), Cursor Lock: None
```
→ Cursor not locked or input not coming!

---

## ✅ **VERIFICATION:**

Test semuanya:

1. **Input Logged:**
   - WASD shows in console ✓
   - Mouse shows in console ✓

2. **Rotation Works:**
   - Mouse left → Player rotates left ✓
   - Mouse right → Player rotates right ✓
   - Mouse up → Camera looks up ✓
   - Mouse down → Camera looks down ✓

3. **Movement Works:**
   - W after rotating → Moves forward ✓
   - Direction follows player rotation ✓

**ALL CHECKED = FIXED!** 🎉

---

**Current Status:**
- ✅ Script updated with debug logging
- ✅ Rotation logic correct
- ⚠️ Need to verify Input Actions mouse binding

**Next Step:**
1. Press Play
2. Check Console for logs
3. Report what Console shows!

*Mouse Look Debug Guide - MidTerm Game Dev*
