using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnitySceneManager = UnityEngine.SceneManagement.SceneManager;

public enum SceneNameType
{
    Title,
    InGame
}

public class SceneManager : Singleton<SceneManager>
{
    [SerializeField] Image fade;
    [SerializeField] GameObject loadingImage;
    [SerializeField] Slider loadingBar;
    [SerializeField] float fadeTime;

    public void LoadScene(SceneNameType type)
    {
        StartCoroutine(LoadingRoutine(type));
    }

    IEnumerator LoadingRoutine(SceneNameType type)
    {
        fade.gameObject.SetActive(true);
        yield return FadeOut();

        Manager.Pool.ClearPool();

        Time.timeScale = 0f;
        loadingImage.SetActive(true);
        loadingBar.gameObject.SetActive(true);
        AsyncOperation oper = UnitySceneManager.LoadSceneAsync((int)type);
        while (oper.isDone == false)
        {
            if(loadingBar.value < 0.7f)
                loadingBar.value = oper.progress;
            yield return null;
        }
        // 페이크로딩 실행
        while (loadingBar.value < 1f)
        {
            loadingBar.value += 0.1f;
            yield return new WaitForSeconds(0.5f);
        }

        loadingImage.SetActive(false);
        loadingBar.gameObject.SetActive(false);
        Time.timeScale = 1f;
        yield return null;

        yield return FadeIn();
        fade.gameObject.SetActive(false);
    }

    IEnumerator FadeOut()
    {
        float rate = 0;
        Color fadeOutColor = new Color(fade.color.r, fade.color.g, fade.color.b, 1f);
        Color fadeInColor = new Color(fade.color.r, fade.color.g, fade.color.b, 0f);

        while (rate <= 1)
        {
            rate += Time.deltaTime / fadeTime;
            fade.color = Color.Lerp(fadeInColor, fadeOutColor, rate);
            yield return null;
        }
    }

    IEnumerator FadeIn()
    {
        float rate = 0;
        Color fadeOutColor = new Color(fade.color.r, fade.color.g, fade.color.b, 1f);
        Color fadeInColor = new Color(fade.color.r, fade.color.g, fade.color.b, 0f);

        while (rate <= 1)
        {
            rate += Time.deltaTime / fadeTime;
            fade.color = Color.Lerp(fadeOutColor, fadeInColor, rate);
            yield return null;
        }
    }
}