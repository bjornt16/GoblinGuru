using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour {

    public GameObject loadingScreen;
    public GameObject mainMenuWrapper;
    public UnityEngine.UI.Slider slider;
    public UnityEngine.UI.Text percentageText;

    public void NewGame()
    {
        mainMenuWrapper.SetActive(false);
        StartCoroutine(LoadScene("Bjossi"));
    }

    IEnumerator LoadScene(string sceneName)
    {
        AsyncOperation operation = SceneManager.LoadSceneAsync("MainScene");

        loadingScreen.SetActive(true);

        while (!operation.isDone)
        {
            float progress = Mathf.Clamp01(operation.progress / .9f);

            slider.value = progress;
            percentageText.text = progress * 100f + "%";

            yield return null;
        }
    }

    public void ExitGame()
    {
        Application.Quit();
    }
}
