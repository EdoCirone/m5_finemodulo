using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuUI : MonoBehaviour
{
    public void StartGame()
    {
        SceneManager.LoadScene("Level1"); // Assicurati che la scena si chiami così
    }

    public void ComandInfoMenu()
    {
        SceneManager.LoadScene("ComandiInfoMenu"); // Assicurati che la scena si chiami così
    }

    public void ExitGame()
    {
        Debug.Log("Hai premuto Exit Game (placeholder)");
    }
}