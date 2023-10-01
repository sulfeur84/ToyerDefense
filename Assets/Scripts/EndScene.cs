using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EndScene : MonoBehaviour
{
    private bool CanClick = false;

    private void Start()
    {
        Invoke("CanPressMouse", 1f);
    }

    private void CanPressMouse()
    {
        CanClick = true;
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(1) || Input.GetMouseButtonDown(0) && CanClick) SceneManager.LoadScene("MainMenu");
    }
}
