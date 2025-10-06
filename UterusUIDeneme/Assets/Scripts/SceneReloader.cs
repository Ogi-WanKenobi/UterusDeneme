using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneReloader : MonoBehaviour
{
    // UI Button'un OnClick'ine bu fonksiyonu bağlayın
    public void RestartScene()
    {
        // Oyun duraklatıldıysa normale al
        if (Time.timeScale == 0f) Time.timeScale = 1f;

        // Aktif sahneyi tekrar yükle
        Scene active = SceneManager.GetActiveScene();
        SceneManager.LoadScene(active.buildIndex, LoadSceneMode.Single);
        // Alternatif: SceneManager.LoadScene(active.name, LoadSceneMode.Single);
    }
}
