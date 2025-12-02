using System.Collections;
using UnityEngine;
using UnityEngine.VFX;
public class DissolveEffect : MonoBehaviour
{
    public Material dissolveMat;
    MeshRenderer meshRender;
    public VisualEffect effect;
    float dissolveRate;
    const float refreshRate = 0.025f;

    private Material[] materials;

    public void Initialize(Mesh mesh, MeshRenderer renderer)
    {
        effect.SetMesh("Mesh", mesh);
        materials = renderer.materials;
        if (materials.Length > 0)
        {
            var color = materials[0].color;
            materials[0] = new Material(dissolveMat);
            materials[0].color = color;
            renderer.materials = materials;
        }
    }

    public void PlayEffect(float time = 1f)
    {
        dissolveRate = 1 / time * refreshRate;
        effect.SetFloat("Duration", time * 0.75f);
        StartCoroutine(DissolveCo());
    }

    IEnumerator DissolveCo()
    {
        if(effect != null)
        {
            effect.Play();
        }

        if(materials.Length > 0)
        {
            float count = 0;
            while (materials[0].GetFloat("_DissolveAmount") < 1)
            {
                count += dissolveRate;
                materials[0].SetFloat("_DissolveAmount", count);
                yield return new WaitForSeconds(refreshRate);
            }
        }
    }
}
