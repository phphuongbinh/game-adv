using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class ScreenFader : MonoBehaviour
{
    public static ScreenFader Instance;

    [SerializeField] private Image fadeImage;
    [SerializeField] private float fadeSpeed = 2f;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        if (fadeImage == null)
            fadeImage = GetComponent<Image>();
    }

    public IEnumerator FadeOut()
    {
        float alpha = fadeImage.color.a;

        while (alpha < 1f)
        {
            alpha += Time.deltaTime * fadeSpeed;
            fadeImage.color = new Color(0, 0, 0, alpha);
            yield return null;
        }
    }

    public IEnumerator FadeIn()
    {
        float alpha = fadeImage.color.a;

        while (alpha > 0f)
        {
            alpha -= Time.deltaTime * fadeSpeed;
            fadeImage.color = new Color(0, 0, 0, alpha);
            yield return null;
        }
    }

    public IEnumerator FadeOutFast()
    {
        float alpha = fadeImage.color.a;

        while (alpha < 1f)
        {
            alpha += Time.deltaTime * 6f; // nhanh
            fadeImage.color = new Color(0, 0, 0, alpha);
            yield return null;
        }
    }

    public IEnumerator FadeInFast()
    {
        float alpha = fadeImage.color.a;

        while (alpha > 0f)
        {
            alpha -= Time.deltaTime * 6f; // nhanh
            fadeImage.color = new Color(0, 0, 0, alpha);
            yield return null;
        }
    }

    public void SetBlack()
    {
        fadeImage.color = new Color(0, 0, 0, 1);
    }

    public void SetClear()
    {
        fadeImage.color = new Color(0, 0, 0, 0);
    }
}