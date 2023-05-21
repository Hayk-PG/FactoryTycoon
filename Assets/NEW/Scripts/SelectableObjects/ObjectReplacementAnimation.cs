using UnityEngine;
using System.Collections;

public class ObjectReplacementAnimation : MonoBehaviour
{
    [SerializeField] private float bounceHeight = 1.0f;
    [SerializeField] private float bounceDuration = 0.5f;

    private Vector3 _initialScale;
    private bool _isAnimating;




    private void Awake()
    {
        _initialScale = transform.localScale;
        _isAnimating = false;
    }

    public void StartReplacementAnimation()
    {
        if (_isAnimating)
        {
            return;
        }

        _isAnimating = true;
        StartCoroutine(BounceAnimation());
    }

    private IEnumerator BounceAnimation()
    {     
        Vector3 firstTargetScale = new Vector3(_initialScale.x, _initialScale.y / 2, _initialScale.z);
        Vector3 secondTargetScale = new Vector3(_initialScale.x, _initialScale.y * 1.7f, _initialScale.z);
        Vector3 thirdTargetScale = new Vector3(_initialScale.x, _initialScale.y / 1.3f, _initialScale.z);

        yield return StartCoroutine(Scale(_initialScale, firstTargetScale, 0.1f));
        yield return StartCoroutine(Scale(firstTargetScale, secondTargetScale, 0.13f));
        yield return StartCoroutine(Scale(secondTargetScale, thirdTargetScale, 0.12f));
        yield return StartCoroutine(Scale(thirdTargetScale, _initialScale, 0.1f, true));
    }

    private IEnumerator Scale(Vector3 initialScale, Vector3 targetScale, float bounceDuration, bool resetScale = false)
    {
        float timeElapsed = 0.0f;

        while (timeElapsed < bounceDuration)
        {
            timeElapsed += Time.deltaTime;

            // Calculate the current scale based on the animation progress
            float t = timeElapsed / bounceDuration;
            Vector3 currentScale = Vector3.Lerp(initialScale, targetScale, t);

            // Apply the current scale to the object
            transform.localScale = currentScale;

            yield return null;
        }

        // Reset the scale to the initial scale
        if (resetScale)
        {
            transform.localScale = _initialScale;
        }
    }
}