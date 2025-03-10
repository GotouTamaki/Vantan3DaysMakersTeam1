using Cysharp.Threading.Tasks;
using UnityEngine;

public class FadeController : MonoBehaviour
{
    private static readonly int FadeThresholdID = Shader.PropertyToID("_DissolveAmount");

    [SerializeField] private Material _fadeMaterial;
    [SerializeField] private AnimationCurve _fadeInCurve;
    [SerializeField] private AnimationCurve _fadeOutCurve;

    private void Start()
    {
        GameManager.Instance._fadeController = this;
    }

#if UNITY_EDITOR
    public void TestFadeIn(float fadeTime) => FadeIn(fadeTime).Forget();

    public void TestFadeOut(float fadeTime) => FadeOut(fadeTime).Forget();
#endif

    public async UniTask FadeIn(float fadeTime)
    {
        _fadeMaterial.SetFloat(FadeThresholdID, 0f);
        float elapsed = 0f;

        while (elapsed < fadeTime)
        {
            elapsed += Time.deltaTime;
            float t = Mathf.Clamp01(elapsed / fadeTime);
            float value = _fadeInCurve.Evaluate(t);
            _fadeMaterial.SetFloat(FadeThresholdID, value);
            await UniTask.Yield(PlayerLoopTiming.Update);
        }

        _fadeMaterial.SetFloat(FadeThresholdID, 1.2f);
    }

    public async UniTask FadeOut(float fadeTime)
    {
        _fadeMaterial.SetFloat(FadeThresholdID, 1.2f);
        float elapsed = 0f;

        while (elapsed < fadeTime)
        {
            elapsed += Time.deltaTime;
            float t = Mathf.Clamp01(elapsed / fadeTime);
            float value = _fadeOutCurve.Evaluate(t);
            _fadeMaterial.SetFloat(FadeThresholdID, value);
            await UniTask.Yield(PlayerLoopTiming.Update);
        }

        _fadeMaterial.SetFloat(FadeThresholdID, 0f);
    }

    private void OnDisable()
    {
        _fadeMaterial.SetFloat(FadeThresholdID, 1.2f);
    }
}
