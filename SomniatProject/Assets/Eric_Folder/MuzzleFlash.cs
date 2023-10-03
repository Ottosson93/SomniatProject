using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Eric_folder { 
public class MuzzleFlash : MonoBehaviour
{
    [Range(0.1f, 1f)][SerializeField] float timeBetweenFrames = 1f;
    [SerializeField] Texture[] frames;
    private MeshRenderer rendererMy;
    Coroutine firingCoroutine;
    private bool coroutineRunning = false;

    void Start()
    {
        rendererMy = GetComponent<MeshRenderer>();
        rendererMy.sortingLayerName = "Player";
        rendererMy.sharedMaterial.SetTexture("_MainTex", frames[0]);
        rendererMy.enabled = false;
    }

    public void PlayAnimation()
    {
        if (coroutineRunning)
        {
            StopCoroutine(firingCoroutine);
        }
        Animate();
    }

    void Animate()
    {
        rendererMy.enabled = true;
        rendererMy.sharedMaterial.SetTexture("_MainTex", frames[0]);

        NextFrame();
        firingCoroutine = StartCoroutine(NextFrame());
    }

    IEnumerator NextFrame()
    {
        coroutineRunning = true;

        foreach (Texture frame in frames)
        {
            rendererMy.sharedMaterial.SetTexture("_MainTex", frame);
            yield return new WaitForSeconds(timeBetweenFrames);
        }
        rendererMy.enabled = false;
        coroutineRunning = false;
    }
}

}