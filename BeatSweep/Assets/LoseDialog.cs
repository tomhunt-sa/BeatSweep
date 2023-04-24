using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class LoseDialog : MonoBehaviour
{

    public GameRunner gameRunner;
    public Button button;
    public TMP_Text completePercentage;

    // Start is called before the first frame update
    void Start()
    {
        button.onClick.AddListener(OnClickHandler);
    }

    void OnClickHandler()
    {
        gameRunner.StartGame();
    }


    public void SetPercentageComplete( int percent )
    {
        Debug.Log(string.Format("Completed {0} %", percent));
        completePercentage.text = percent.ToString() + "%";
    }
}
