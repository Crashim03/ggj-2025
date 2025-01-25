using TMPro;
using UnityEngine;

public class ScoreUI : MonoBehaviour
{
    private TMP_Text text;
    public void SetText(int score) {
        text.text = score.ToString();
    }

    private void Awake() {
        text = GetComponent<TMP_Text>();
    }
}
