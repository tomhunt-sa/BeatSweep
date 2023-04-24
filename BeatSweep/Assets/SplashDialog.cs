using UnityEngine;
using UnityEngine.UI;

public class SplashDialog : MonoBehaviour
{
    public GameRunner gameRunner;
    public Button button;

    // Start is called before the first frame update
    void Start()
    {
        button.onClick.AddListener(OnClickHandler);
    }

    void OnClickHandler()
    {
        gameRunner.StartGame();
    }
}
