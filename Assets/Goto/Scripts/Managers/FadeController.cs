using Cysharp.Threading.Tasks;
using UnityEngine;

public class FadeController : MonoBehaviour
{
    private static readonly int FadeThresholdID = Shader.PropertyToID("_DissolveAmount");

    [SerializeField] private Material _fadeMaterial;
    [SerializeField] private AnimationCurve _fadeInCurve;
    [SerializeField] private AnimationCurve _fadeOutCurve;

#if UNITY_EDITOR
    public void TestFadeIn(float fadeTime) => FadeIn(fadeTime).Forget();

    public void TestFadeOut(float fadeTime) => FadeOut(fadeTime).Forget();
#endif

    public async UniTaskVoid FadeIn(float fadeTime)
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

        _fadeMaterial.SetFloat(FadeThresholdID, _fadeInCurve.Evaluate(1f));
    }

    public async UniTaskVoid FadeOut(float fadeTime)
    {
        _fadeMaterial.SetFloat(FadeThresholdID, 1f);
        float elapsed = 0f;

        while (elapsed < fadeTime)
        {
            elapsed += Time.deltaTime;
            float t = Mathf.Clamp01(elapsed / fadeTime);
            float value = _fadeOutCurve.Evaluate(t);
            _fadeMaterial.SetFloat(FadeThresholdID, value);
            await UniTask.Yield(PlayerLoopTiming.Update);
        }

        _fadeMaterial.SetFloat(FadeThresholdID, _fadeOutCurve.Evaluate(1f));
    }
}
