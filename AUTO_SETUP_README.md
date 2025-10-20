# 🚀 DIAMOND SYSTEM - AUTO SETUP

## ✨ FULLY AUTOMATIC - NO MANUAL WORK NEEDED!

Sistem ini akan **OTOMATIS SETUP SENDIRI** tanpa kamu perlu klik apapun!

---

## 🎯 Apa yang Auto-Setup?

Script Editor akan otomatis:
1. ✅ Add `Diamond` script ke semua diamond di scene
2. ✅ Set semua diamond BoxCollider jadi Trigger
3. ✅ Create `_GameManager` GameObject
4. ✅ Create `GameUI_Canvas` dengan Score & Counter
5. ✅ Link semua references otomatis
6. ✅ Set Zombie tag jadi "Player"
7. ✅ Setup Diamond prefab dengan script

---

## 📋 Cara Kerja

### **AUTOMATIC MODE** (Default)

1. **Save all scripts** (Ctrl+S)
2. **Kembali ke Unity**
3. **Tunggu compile selesai**
4. **AUTO-SETUP JALAN SENDIRI!**
5. **Lihat Console** untuk konfirmasi:
   ```
   🚀 AUTO-SETUP: Starting Diamond System setup...
   Step 1: Adding Diamond scripts...
     ✅ Added Diamond script to 15 objects
   Step 2: Creating GameManager...
     ✅ GameManager created
   Step 3: Creating Game UI...
     ✅ Canvas created
     ✅ UI elements created and linked
   Step 4: Linking references...
     ✅ References linked
   Step 5: Verifying Zombie tag...
     ✅ Zombie tag set to 'Player'
   💾 Scene marked as dirty. Remember to SAVE scene! (Ctrl+S)
   ✅ AUTO-SETUP COMPLETE! Diamond system ready to play!
   ```

6. **SAVE SCENE** (Ctrl+S)
7. **PLAY!** 🎮

---

## 🔧 Manual Controls (Jika Diperlukan)

### Force Run Setup Lagi:
```
Unity Menu → Tools → Diamond System → Force Auto-Setup Now
```

### Reset dan Run Ulang:
```
Unity Menu → Tools → Diamond System → Reset Setup (Run Again)
```
Lalu reload scene atau force setup.

### Setup Diamond Prefab Only:
```
Unity Menu → Tools → Diamond System → Setup Diamond Prefab
```

---

## ✅ Verification Checklist

Setelah auto-setup selesai, check:

### Di Hierarchy:
- [ ] Ada GameObject `_GameManager`
- [ ] Ada GameObject `GameUI_Canvas`
  - [ ] Child: `ScoreText`
  - [ ] Child: `DiamondCountText`

### Di Scene:
- [ ] Pilih Diamond manapun
- [ ] Check component `Diamond` ada
- [ ] Check `BoxCollider` → Is Trigger = ✅

### Di Zombie:
- [ ] Select Zombie
- [ ] Inspector → Tag = "Player" ✅

### Di Project:
- [ ] Diamond.prefab sudah punya script `Diamond`

---

## 🎮 Testing

1. **Save Scene** (Ctrl+S)
2. **Play Mode**
3. **Gerakkan Zombie ke Diamond**
4. **Diamond hilang** ✅
5. **Score bertambah** ✅
6. **Counter update** (💎 1/15) ✅
7. **Console log**: "Diamond collected!" ✅
8. **Collect semua** → "🎉 YOU WIN!" ✅

---

## 🐛 Troubleshooting

### Setup tidak jalan otomatis?

**Coba ini:**
1. Check Console ada error compile atau tidak
2. Menu: `Tools → Diamond System → Force Auto-Setup Now`
3. Jika masih gagal, check script ada di folder:
   ```
   /Assets/UTS/MidTermGameDev/Assets/Script/Editor/
   - AutoSetupDiamondSystem.cs
   - AutoSetupDiamondPrefab.cs
   ```

### Diamond tidak hilang saat collected?

**Check:**
1. Zombie tag = "Player" (auto-set by script)
2. Diamond BoxCollider Is Trigger = ON (auto-set by script)
3. Diamond ada component `Diamond` (auto-added by script)

### UI tidak muncul?

**Check:**
1. TextMeshPro imported:
   ```
   Window → TextMeshPro → Import TMP Essential Resources
   ```
2. GameUI_Canvas ada di Hierarchy
3. GameManager references terisi (auto-linked)

### Setup jalan 2x atau lebih?

**Reset flag:**
```
Tools → Diamond System → Reset Setup (Run Again)
```
Ini akan clear flag, tapi setup tidak jalan lagi kecuali scene reload atau force setup.

---

## 🎨 Customization

Setelah auto-setup, kamu bisa custom:

### Per Diamond:
```
Select any Diamond → Inspector → Diamond Component
- Rotation Speed: 50
- Enable Floating: ✅
- Float Amplitude: 0.5
- Point Value: 10
```

### GameManager:
```
Select _GameManager → Inspector
- Total Diamonds: auto-detect dari scene
- Auto Win On Complete: ✅
```

### UI Position/Style:
```
Select GameUI_Canvas → ScoreText / DiamondCountText
- Ubah position, size, color, font size
```

---

## 📝 Script Files Created

### Runtime Scripts:
- `/Assets/.../Script/Diamond.cs` - Individual diamond behavior
- `/Assets/.../Script/GameManager.cs` - Game logic & score
- `/Assets/.../Script/GameUI.cs` - HUD display
- `/Assets/.../Script/DiamondSystemSetup.cs` - Manual setup helper
- `/Assets/.../Script/FixGunBotSize.cs` - GunBot size helper

### Editor Scripts (AUTO-RUN):
- `/Assets/.../Script/Editor/AutoSetupDiamondSystem.cs` - Auto-setup scene
- `/Assets/.../Script/Editor/AutoSetupDiamondPrefab.cs` - Auto-setup prefab

---

## 💡 How It Works

### InitializeOnLoad Attribute:
```csharp
[InitializeOnLoad]
public class AutoSetupDiamondSystem
{
    static AutoSetupDiamondSystem()
    {
        EditorApplication.delayCall += OnEditorLoaded;
    }
}
```

Script ini jalan **OTOMATIS** saat:
- Unity startup
- Scripts recompile
- Scene load (jika Level1)

### One-Time Setup:
```csharp
private const string SETUP_KEY = "DiamondSystem_AutoSetup_Done_v1";

bool alreadySetup = EditorPrefs.GetBool(SETUP_KEY, false);
if (alreadySetup) return; // Skip jika sudah setup
```

Setup hanya jalan **SEKALI** per project. Flag disimpan di EditorPrefs.

---

## 🎉 Summary

| Action | Required? | When? |
|--------|-----------|-------|
| Manual Click | ❌ NO | NEVER |
| Save Scripts | ✅ YES | After creating files |
| Wait Compile | ✅ YES | Auto by Unity |
| Auto-Setup Runs | ✅ AUTO | After compile |
| Save Scene | ✅ YES | After setup complete |
| Play Test | ✅ YES | When ready |

---

## 🚀 You're All Set!

Script sudah dibuat dan akan **AUTO-SETUP SENDIRI**!

**Next Steps:**
1. ✅ Scripts already created
2. 🔄 **SAVE ALL** (Ctrl+S)
3. 🔄 **Return to Unity**
4. ⏳ **Wait for compile**
5. ✨ **Auto-setup runs automatically**
6. 💾 **Save scene** (Ctrl+S)
7. 🎮 **PLAY!**

**SELAMAT BERMAIN!** 🎉💎
