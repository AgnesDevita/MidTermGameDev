using UnityEngine;

public class SetupHelper : MonoBehaviour
{
    [Header("Quick Setup Guide")]
    [TextArea(20, 30)]
    public string instructions = @"
=== GAME SETUP CHECKLIST ===

LEVEL 1 SCENE SETUP:
1. Create Level1 scene in Assets/UTS/MidTermGameDev/Assets/Scenes/
2. Add Room model from Assets/Asset/Room.glb
3. Setup NavMesh: Window > AI > Navigation, select Room, mark as Navigation Static, Bake

PLAYER SETUP (Zombie):
1. Drag Zombie.fbx to scene
2. Add Tag 'Player' to Zombie
3. Add components:
   - Rigidbody (Constraints: Freeze Rotation X,Z)
   - CapsuleCollider
   - PlayerController script
   - PlayerHealth script
   - PlayerInputActions (set to InputSystem_Actions asset)
4. Camera: Create child camera or assign Main Camera

ENEMY SETUP (GunBot):
1. Drag gun-bot_with_walk_and_idle_animation.glb to scene
2. Scale to 0.01 (important!)
3. Add components:
   - NavMeshAgent
   - CapsuleCollider
   - GunBotAI script
   - Animator (use GunBot controller)
4. GunBotAI settings:
   - Detection: 500
   - Chase Speed: 300
   - Patrol Speed: 150
   - Attack Range: 80

DIAMOND SYSTEM:
1. Create Diamond prefab from Diamond_ori.glb
2. Scale diamond to be visible (3x)
3. Add BoxCollider (isTrigger = true)
4. Add Diamond script
5. Create DiamondSpawner GameObject:
   - Add DiamondSpawner script
   - Assign diamond prefab
   - Create spawn areas (BoxColliders)
   - Set Layer 'SpawnArea'

GAME MANAGER:
1. Create empty GameObject 'GameManager'
2. Add GameManager script
3. Link UI elements

UI SETUP:
1. Create Canvas (Screen Space - Overlay)
2. Add GameUI with:
   - ScoreText
   - DiamondCountText
   - LevelText
3. Add HealthUI with:
   - HealthBar (Slider)
   - HealthBarFill (Image)
   - HealthText
4. Create WinPanel and GameOverPanel
5. Add PauseMenu

BUILD SETTINGS:
1. File > Build Settings
2. Add scenes in order:
   - MainMenu
   - Level1
3. Player Settings > Input System Package (Both/New)

LAYERS & TAGS:
- Tags: Player, Finish
- Layers: Level, SpawnArea

INPUT SYSTEM:
- Use InputSystem_Actions.inputactions
- Move: WASD/Arrows
- Look: Mouse
- Sprint: Left Shift
";

    [ContextMenu("Show Setup Guide")]
    public void ShowGuide()
    {
        Debug.Log(instructions);
    }
}
