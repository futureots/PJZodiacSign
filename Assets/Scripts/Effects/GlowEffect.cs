using UnityEngine;
using UnityEngine.VFX;

public class GlowEffect : MonoBehaviour
{
    public VisualEffect effect;
    public void Init(Mesh mesh)
    {
        effect.SetMesh("Mesh",mesh);
    }

    public void Play()
    {
        effect.Play();
    }
}
