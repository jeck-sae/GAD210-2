
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : Singleton<GameManager>
{
    public GameObject badEnding;
    public GameObject goodEnding;

    private void Awake()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    public void GoodEnding()
    {
        badEnding.SetActive(true);
        Cursor.lockState = CursorLockMode.None;
        FindAnyObjectByType<MosquitoMovement>().enabled = false;
    }
    public void BadEnding()
    {
        goodEnding.SetActive(true);
        Cursor.lockState = CursorLockMode.None;
        FindAnyObjectByType<MosquitoMovement>().enabled = false;
    }

    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
