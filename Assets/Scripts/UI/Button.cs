using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Button : MonoBehaviour
{
    private GameObject FadeImage;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    public void GoToScene(string scene)
    {
        GameObject[] allObjs = FindObjectsOfType<GameObject>();
        foreach (GameObject go in allObjs)
        {
            if (go.name == "FadeImage")
                FadeImage = go;
        }
        if(FadeImage!=null)
            FadeImage.GetComponent<FadeImage>().StartFadeOut(scene);
        else
            UnityEngine.SceneManagement.SceneManager.LoadScene(scene, LoadSceneMode.Single);
    }

    public void Exit()
    {
        //Application.Quit(); 
        UnityEditor.EditorApplication.isPlaying = false;
    }
    
    public void OnPointerEnter(PointerEventData eventData)
    {
        transform.localScale = new Vector3(1.1f,1.1f);
        Debug.Log("laile");
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        transform.localScale = new Vector3(1.0f,1.0f);
        Debug.Log("Zoule");
    }

    public void OnMouseOver()
    {
        transform.localScale = new Vector3(1.1f,1.1f);
        Debug.Log("laile");
    }

    public void OnMouseExit()
    {
        transform.localScale = new Vector3(1.0f,1.0f);
        Debug.Log("Zoule");
    }
}
