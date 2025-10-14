using UnityEngine;

public class DropToGround : MonoBehaviour
{
    public float targetY = 0f;

    [ContextMenu("Drop To targetY")]
    void Drop()
    {
        var renderers = GetComponentsInChildren<Renderer>();
        if (renderers.Length == 0) return;

        Bounds b = renderers[0].bounds;
        foreach (var r in renderers) b.Encapsulate(r.bounds);

        Vector3 p = transform.position;
        float delta = b.min.y - targetY;   // selisih tepi bawah ke targetY
        p.y -= delta;                      // koreksi: bisa naik atau turun
        transform.position = p;
    }
}
