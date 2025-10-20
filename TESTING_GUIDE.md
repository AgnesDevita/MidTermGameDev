# Testing Guide for Camera Fixes

## Prerequisites
- Unity Editor version 6000.2.1f1 or compatible
- Project opened in Unity Editor

## Test Scenarios

### 1. Basic Camera Following
**Objective**: Verify camera follows player smoothly without jitter

**Steps**:
1. Open scene `Assets/Scenes/Level1.unity`
2. Press Play in Unity Editor
3. Use WASD keys to move the player character
4. Observe camera behavior

**Expected Results**:
- ✓ Camera follows player smoothly
- ✓ No sudden jumps or position changes
- ✓ Camera maintains consistent distance from player
- ✓ Smooth interpolation is visible in all directions

**Potential Issues Before Fix**:
- ✗ Camera jitters when moving
- ✗ Sudden position changes
- ✗ Inconsistent following speed

---

### 2. Wall Collision Handling
**Objective**: Verify camera handles walls correctly without clipping

**Steps**:
1. Play the scene
2. Move player character toward a wall
3. Get close to the wall so camera would normally clip through
4. Observe camera behavior

**Expected Results**:
- ✓ Camera stops at appropriate distance from wall
- ✓ Camera doesn't clip through geometry
- ✓ Movement remains smooth as player approaches wall
- ✓ Camera transitions smoothly when moving away from wall
- ✓ Collision padding (0.35f) is respected

**Potential Issues Before Fix**:
- ✗ Camera clips through walls
- ✗ Jerky movement when near walls
- ✗ Collision detection misses some obstacles
- ✗ Camera jumps when collision is detected

---

### 3. Corner Navigation
**Objective**: Verify camera handles tight spaces and corners

**Steps**:
1. Play the scene
2. Move player into a corner or tight space
3. Try to navigate through narrow passages
4. Observe camera behavior

**Expected Results**:
- ✓ Camera finds best position when in corners
- ✓ No rapid position changes or flickering
- ✓ Camera doesn't get stuck
- ✓ Smooth transitions as player enters/exits tight spaces

**Potential Issues Before Fix**:
- ✗ Camera position flickers in corners
- ✗ Rapid oscillation between positions
- ✗ Camera gets stuck or behaves unpredictably

---

### 4. Rotation Testing
**Objective**: Verify camera follows player rotation smoothly

**Steps**:
1. Play the scene
2. Use A/D keys to rotate player quickly
3. Rotate while moving
4. Rotate while near walls

**Expected Results**:
- ✓ Camera rotates smoothly with player
- ✓ Camera maintains proper offset during rotation
- ✓ No stuttering during rotation
- ✓ Collision detection works during rotation

**Potential Issues Before Fix**:
- ✗ Camera lags during rotation
- ✗ Incorrect position after rotation
- ✗ Collision detection fails during rotation

---

### 5. Speed Variation Testing
**Objective**: Verify camera handles different movement speeds

**Steps**:
1. Play the scene
2. Walk normally (W key)
3. Run by holding Left Shift + W
4. Test both near open areas and walls

**Expected Results**:
- ✓ Camera follows smoothly at walk speed
- ✓ Camera follows smoothly at run speed
- ✓ Collision detection works at all speeds
- ✓ No overshooting or lagging at any speed

**Potential Issues Before Fix**:
- ✗ Camera can't keep up at higher speeds
- ✗ Collision detection misses at higher speeds
- ✗ Jerky movement at speed transitions

---

## Performance Testing

### Frame Rate Check
**Steps**:
1. Open Unity Stats window (Window → Analysis → Stats)
2. Play the scene
3. Monitor FPS while testing all scenarios

**Expected Results**:
- ✓ No significant FPS drop
- ✓ Consistent frame times
- ✓ No performance regression from the fix

---

## Debug Visualization (Optional)

To visualize the raycast for collision detection, you can temporarily add this to `CameraFollow.cs`:

```csharp
private Vector3 HandleCollision()
{
    RaycastHit hit;
    Vector3 targetPosition = target.position + Vector3.up * 1.0f;
    
    Vector3 direction = (desiredPosition - targetPosition).normalized;
    float distanceToCamera = Vector3.Distance(targetPosition, desiredPosition);
    
    // DEBUG: Visualize the raycast
    Debug.DrawRay(targetPosition, direction * distanceToCamera, Color.red);
    
    if (Physics.Raycast(targetPosition, direction, out hit, distanceToCamera, collisionMask))
    {
        // DEBUG: Visualize hit point
        Debug.DrawLine(targetPosition, hit.point, Color.yellow);
        return hit.point + hit.normal * collisionPadding;
    }
    
    // DEBUG: Visualize clear path
    Debug.DrawRay(targetPosition, direction * distanceToCamera, Color.green);
    return desiredPosition;
}
```

**What to look for**:
- Red/Green line: Shows raycast path (red = default, green = no collision)
- Yellow line: Shows actual collision point
- Lines should always point from player to camera
- Line length should match actual camera distance

---

## Inspector Verification

### Camera Settings to Check
Navigate to the Camera GameObject in the Hierarchy and verify:

**CameraFollow Component**:
- `Target`: Should reference the Player/Zombie GameObject
- `Distance`: Default 5.0 (adjust as needed)
- `Height`: Should NOT be 0 (default 2.0)
- `Smooth Speed`: Default 10.0 (adjust for more/less smoothing)
- `Collision Mask`: Should include walls, floors, obstacles
- `Collision Padding`: Default 0.35 (minimum distance from walls)

---

## Automated Testing (If Available)

If you have Unity Test Framework set up:

```csharp
[Test]
public void CameraFollow_RaycastDirection_IsNormalized()
{
    // Setup camera and target
    var camera = new GameObject().AddComponent<CameraFollow>();
    var target = new GameObject().transform;
    
    // Verify direction is normalized in HandleCollision
    // (Would need to make HandleCollision testable or use reflection)
}
```

---

## Regression Testing

### Compare Before and After
If possible, record gameplay footage before and after the fix to compare:

1. Record 30 seconds of each test scenario BEFORE applying fix
2. Apply the fix (checkout the branch)
3. Record 30 seconds of each test scenario AFTER fix
4. Compare side-by-side for:
   - Smoothness of movement
   - Collision handling
   - Camera positioning accuracy

---

## Issue Reporting

If you find any issues during testing, please report them with:

1. **Unity Version**: [version]
2. **Test Scenario**: [which test above]
3. **Expected Behavior**: [what should happen]
4. **Actual Behavior**: [what actually happened]
5. **Steps to Reproduce**: [detailed steps]
6. **Screenshots/Video**: [if applicable]
7. **Console Errors**: [any errors or warnings]

---

## Success Criteria

The fix is successful if:
- ✓ All 5 main test scenarios pass
- ✓ No performance regression
- ✓ No console errors or warnings
- ✓ Camera behavior feels natural and smooth
- ✓ Collision detection is accurate and consistent

---

## Notes

- The fix is purely in `CameraFollow.cs` - no changes to Unity scenes or settings
- No changes to public API - all inspector settings remain the same
- No breaking changes - existing projects should work without modification
- If you modify `smoothSpeed` in inspector, use higher values (10-15) for smoother movement
