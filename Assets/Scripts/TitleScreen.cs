using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TitleScreen : MonoBehaviour
{
    public GameObject TextToAppear;
    private bool CanClick = false;

    private void Start()
    {
        Invoke("CanPressMouse", 1.5f);
    }

    private void CanPressMouse()
    {
        CanClick = true;
        TextToAppear.gameObject.SetActive(true);
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(1) || Input.GetMouseButtonDown(0) && CanClick) SceneManager.LoadScene("MainMenu");
    }
}
