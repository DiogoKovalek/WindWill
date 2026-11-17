using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class redFilterControler : MonoBehaviour
{
    private Image imagem;

    private Coroutine animationRoutine = null;

    private const float maxAlpha = 0.8f;
    private const float minAlpha = 0.05f;

    [Min(0)] [SerializeField] private float animationSpeed = 0.01f;

    void Awake()
    {
        imagem = GetComponent<Image>();
    }

    public void OnSetAlert(bool batteryLow)
    {
        if(animationRoutine == null && batteryLow)
            animationRoutine = StartCoroutine(FilterAnimation());
    }   

    private IEnumerator FilterAnimation()
    {
        for(float i = minAlpha; i < maxAlpha; i += animationSpeed)
        {
            yield return null;
            SetAlpha(i);
        }

        for(float i = maxAlpha; i > minAlpha; i -= animationSpeed)
        {
            yield return null;
            SetAlpha(i);
        }

        animationRoutine = null;
    }

    private void SetAlpha(float alpha)
    {
        imagem.color = new Color(1.0f, 0, 0, alpha);
    }
}
