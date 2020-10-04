using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HideByPos : MonoBehaviour
{
    public Transform Target;
    public SnapAxis Axis;
    public float Value;
    public bool Reverse;
    public Material AltMaterial;
    private Renderer renderer;
    private Material material;
    private void Start()
    {
        renderer = GetComponent<Renderer>();
        material = renderer.material;
    }
    private void Update()
    {
        int sign = Reverse ? 1 : -1;
        switch (Axis)
        {
            case SnapAxis.None:
                break;
            case SnapAxis.X:
                if (Target.position.x * sign <= Value * sign)
                {
                    renderer.material = AltMaterial;
                    renderer.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
                }
                else
                {
                    renderer.material = material;
                    renderer.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.On;
                }
                break;
            case SnapAxis.Y:
                if (Target.position.y * sign <= Value * sign)
                {
                    renderer.material = AltMaterial;
                    renderer.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
                }
                else
                {
                    renderer.material = material;
                    renderer.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.On;
                }
                break;
            case SnapAxis.Z:
                if (Target.position.z * sign <= Value * sign)
                {
                    renderer.material = AltMaterial;
                    renderer.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
                }
                else
                {
                    renderer.material = material;
                    renderer.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.On;
                }
                break;
            case SnapAxis.All:
                break;
            default:
                break;
        }
    }
}
