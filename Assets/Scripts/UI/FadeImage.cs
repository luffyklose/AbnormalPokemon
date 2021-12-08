using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class FadeImage : MonoBehaviour
{
    private Image image;
    private float alpha;
    
    // Start is called before the first frame update
    void Start()
    {
        image = GetComponent<Image>();
        if (image == null)
            Debug.Log("no image");
        else
            StartCoroutine(FadeIn());
        //Debug.Log("Fade Image Start");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void StartFadeOut(string SceneName)
    {
        StartCoroutine(FadeOut(SceneName));
    }

    IEnumerator FadeOut(string SceneName)
    {
        //Debug.Log("Start Fade Out");
        image.transform.SetSiblingIndex(100);
        alpha = 0;

        while (alpha < 1)
        {
            alpha += Time.deltaTime;
            image.color = new Color(0.0f, 0.0f, 0.0f, alpha);
            yield return null;
        }
        
        UnityEngine.SceneManagement.SceneManager.LoadScene(SceneName, LoadSceneMode.Single);
    }

    IEnumerator FadeIn()
    {
        //Debug.Log("Start Fade In");
        image.transform.SetSiblingIndex(100);
        alpha = 1;

        while (alpha > 0)
        {
            alpha -= Time.deltaTime;
            image.color = new Color(0.0f, 0.0f, 0.0f, alpha);
            yield return null;
        }
        image.transform.SetSiblingIndex(0);
    }
}
