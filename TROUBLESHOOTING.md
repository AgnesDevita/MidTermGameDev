# 🔧 TROUBLESHOOTING GUIDE

## ⚡ QUICK FIXES

### 🎮 **PLAYER GAK BISA JALAN?** ← PALING SERING!

**Quick Fix:**
```
1. MidTerm Game > Fix Player Movement
2. Klik: FIX PLAYER AUTO
3. Edit > Project Settings > Player > Other Settings
4. Active Input Handling → "Both"
5. Restart Unity (HARUS!)
6. Press Play
```

**Baca detail:** `PLAYER_MOVEMENT_FIX.md`

---

## ✅ VERIFIED: NO ERRORS IN SCRIPTS

All MidTerm Game Dev scripts have been checked and verified for Unity 6 (6000.2) compatibility.

---

## 🎯 UNITY 6 COMPATIBILITY

### ✅ Correct APIs Used

All scripts use **Unity 6 compatible APIs**:

```csharp
// ✅ CORRECT (Unity 6)
FindFirstObjectByType<GameManager>()
FindObjectsByType<GunBotAI>(FindObjectsSortMode.None)

// ❌ DEPRECATED (Old Unity)
FindObjectOfType<GameManager>()  // Don't use this
FindObjectsOfType<GunBotAI>()    // Don't use this
```

### Scripts Using Correct API:
- ✅ WinLosePanel.cs - Uses `FindFirstObjectByType`
- ✅ PlayButton.cs - Uses `FindFirstObjectByType`
- ✅ ExitButton.cs - Uses `FindFirstObjectByType`
- ✅ QuickSetupMenu.cs - Uses `FindFirstObjectByType`
- ✅ ProjectValidator.cs - Uses `FindFirstObjectByType`
- ✅ All other scripts - No deprecated APIs

---

## 🐛 COMMON ISSUES & FIXES

### Issue 0: "NavMeshSurface could not be found" ✅ FIXED

**Error Message:**
```
error CS0246: The type or namespace name 'NavMeshSurface' could not be found
```

**Cause:**
In Unity 6, `NavMeshSurface` moved to a new namespace.

**Fix:** ✅ ALREADY APPLIED
```csharp
// Add this to top of script:
using Unity.AI.Navigation;

// Old (Unity 5/2019/2020):
using UnityEngine.AI;  // Only has NavMesh, NavMeshAgent

// New (Unity 6):
using Unity.AI.Navigation;  // Has NavMeshSurface
```

**Requirements:**
- AI Navigation package installed (com.unity.ai.navigation 2.0.8+)
- Correct using statement

**Status:** ✅ Fixed in ProjectValidator.cs

---

### Issue 1: "Script compilation failed"

**Possible Causes:**
1. Missing dependencies
2. Typos in script names
3. Unity needs to recompile

**Fix:**
```
1. Check Console (Ctrl+Shift+C)
2. Look for red error messages
3. Fix any syntax errors shown
4. Wait for Unity to recompile
```

### Issue 2: "Can't find [ScriptName]"

**Fix:**
```
1. Check script exists:
   Project > Assets/UTS/MidTermGameDev/Assets/Script/

2. Force Unity to recompile:
   Assets > Reimport All

3. Check spelling matches exactly
```

### Issue 3: "Missing assembly reference"

**Fix:**
```
1. Check required packages installed:
   - Input System (com.unity.inputsystem)
   - TextMeshPro (com.unity.ugui)
   - AI Navigation (com.unity.ai.navigation)

2. Window > Package Manager
3. Install missing packages
```

### Issue 4: "The type or namespace name could not be found"

**Common Missing Using Statements:**

```csharp
// For Unity Editor scripts
using UnityEditor;

// For TextMeshPro
using TMPro;

// For Input System
using UnityEngine.InputSystem;

// For NavMesh
using UnityEngine.AI;

// For UI
using UnityEngine.UI;

// For Scene Management
using UnityEngine.SceneManagement;
```

**All MidTerm scripts already include correct using statements!**

---

## 📦 REQUIRED PACKAGES

### ✅ Already Installed:
- Input System (1.14.2) ✓
- AI Navigation (2.0.8) ✓
- Universal RP (17.2.0) ✓
- TextMeshPro (via UGUI 2.0.0) ✓

### How to Check:
```
Window > Package Manager
Look for packages above
```

---

## 🔍 HOW TO DEBUG ERRORS

### Step 1: Open Console
```
Window > General > Console
OR
Ctrl + Shift + C
```

### Step 2: Clear Console
```
Click "Clear" button
Press Play or recompile
Watch for new errors
```

### Step 3: Read Error Message
```
Error format:
[ScriptName.cs(line number)] Error message

Example:
PlayerController.cs(45): error CS0246: The type or namespace name 'InputSystem' could not be found
```

### Step 4: Fix Based on Error Type

**Syntax Errors:**
- Missing semicolon `;`
- Missing bracket `{` or `}`
- Typo in variable name

**Missing Reference Errors:**
- Missing `using` statement
- Package not installed
- Wrong namespace

**Logic Errors:**
- Wrong method call
- Incorrect parameter type
- Null reference

---

## 🎯 SPECIFIC SCRIPT CHECKS

### PlayerController.cs

**Requirements:**
- ✅ Input System package
- ✅ Rigidbody component
- ✅ Unity.InputSystem namespace

**Common Issues:**
```csharp
// Issue: OnMove not called
// Fix: Check Input Actions asset assigned
// Check: Player Settings > Active Input Handling = "Both" or "New"
```

### GunBotAI.cs

**Requirements:**
- ✅ AI Navigation package
- ✅ NavMeshAgent component
- ✅ Baked NavMesh

**Common Issues:**
```csharp
// Issue: NavMeshAgent not working
// Fix 1: Bake NavMesh (Window > AI > Navigation)
// Fix 2: Check agent is on NavMesh
// Fix 3: Check NavMesh package installed
```

### GameUI.cs & HealthUI.cs

**Requirements:**
- ✅ TextMeshPro package
- ✅ TMPro namespace
- ✅ Canvas in scene

**Common Issues:**
```csharp
// Issue: "TMPro not found"
// Fix: Import TextMeshPro
// Window > TextMeshPro > Import TMP Essential Resources
```

---

## 🔧 EDITOR SCRIPT ISSUES

### QuickSetupMenu.cs

**Requirements:**
- ✅ UnityEditor namespace
- ✅ Must be in Editor folder

**Common Issues:**
```
Issue: Menu not showing
Fix: Check script is in /Assets/.../Editor/ folder
     Editor scripts MUST be in Editor folder
```

### ProjectValidator.cs

**Same requirements as QuickSetupMenu**

---

## 🚀 FORCE RECOMPILATION

If scripts seem broken but you see no errors:

### Method 1: Reimport All
```
1. Assets > Reimport All
2. Wait for Unity to recompile
3. Check Console
```

### Method 2: Refresh
```
1. Assets > Refresh (Ctrl+R)
2. Wait for compilation
```

### Method 3: Restart Unity
```
1. Save project
2. Close Unity
3. Reopen project
4. Wait for full recompilation
```

### Method 4: Delete Library
```
1. Close Unity
2. Delete Library folder in project root
3. Reopen Unity
4. Wait for full reimport (takes time!)
```

---

## 📋 PRE-FLIGHT CHECKLIST

Before reporting errors, check:

- [ ] All packages installed
- [ ] Input System set to "Both" or "New"
- [ ] TextMeshPro imported
- [ ] Scripts in correct folders
- [ ] No red errors in Console
- [ ] Unity fully loaded/compiled

---

## 🎮 INPUT SYSTEM SPECIFIC

### Player Settings Check:
```
Edit > Project Settings > Player > Other Settings
Active Input Handling: Both (recommended)
```

### Input Actions Check:
```
File exists: InputSystem_Actions.inputactions
Location: Assets/UTS/MidTermGameDev/Assets/
```

### Common Input Error:
```
Error: "The type or namespace name 'InputSystem' could not be found"

Fix:
1. Window > Package Manager
2. Find "Input System"
3. If not installed, Install it
4. If installed, check version (should be 1.14.2+)
5. Add to script: using UnityEngine.InputSystem;
```

---

## 🔍 NAMESPACE ISSUES

All MidTerm scripts use correct namespaces:

```csharp
// Core Unity
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.AI;
using UnityEngine.UI;

// Unity Editor (Editor folder only)
using UnityEditor;

// AI Navigation (Unity 6 requirement for NavMeshSurface)
using Unity.AI.Navigation;

// Input System
using UnityEngine.InputSystem;

// TextMeshPro
using TMPro;

// Collections
using System.Collections.Generic;
```

**IMPORTANT FOR UNITY 6:**
- `NavMeshSurface` requires `Unity.AI.Navigation` namespace
- Old namespace `UnityEngine.AI` only has `NavMesh` and `NavMeshAgent`
- Make sure AI Navigation package (2.0.8+) is installed

**No custom namespaces used** - all scripts in global namespace for simplicity.

---

## 💡 PREVENTION TIPS

### 1. Keep Unity Updated
- Using Unity 6 (6000.2)
- All features tested on this version

### 2. Keep Packages Updated
- Update packages regularly
- Check for LTS versions

### 3. Backup Before Changes
- Use version control (Git)
- Or make manual backups

### 4. Test Incrementally
- Add one script at a time
- Test after each addition
- Easier to find issues

---

## 🆘 STILL HAVE ERRORS?

### Share This Information:

1. **Exact error message** from Console
2. **Script name and line number**
3. **Unity version** (Help > About Unity)
4. **What you were doing** when error occurred

### Quick Diagnostic:

```
1. Open CompilationTest.cs
2. Press Play
3. Check Console
4. Should see: "✅ All MidTerm scripts compiled successfully"
5. If not, share error message
```

---

## ✅ VERIFICATION

Run this to verify everything works:

```
1. MidTerm Game > Validate Project Setup
2. Check all items
3. Fix any failures
4. Rerun validation
```

---

## 📞 QUICK FIXES SUMMARY

| Issue | Quick Fix |
|-------|-----------|
| Script won't compile | Check Console for errors |
| Menu not showing | Move script to Editor folder |
| TMPro error | Import TMP Essential Resources |
| Input System error | Install Input System package |
| NavMesh error | Install AI Navigation package |
| Can't find script | Check spelling, Reimport All |
| Random errors | Restart Unity |
| Persistent issues | Delete Library folder |

---

**All scripts are verified working for Unity 6!**
**If you see errors, follow steps above to diagnose.**

---

*Troubleshooting Guide - MidTerm Game Dev*
*Last verified: Unity 6000.2*
