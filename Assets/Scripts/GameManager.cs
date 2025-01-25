using UnityEngine;
using UnityEngine.Events;

public class GameManager: MonoBehaviour {
    public static GameManager Instance { get; private set; }
    public UnityEvent<int> incrementRedEvent;
    public UnityEvent<int> incrementBlueEvent;
    public UnityEvent<int> incrementYellowEvent;
    public UnityEvent<int> incrementGreenEvent;
    private int red = 0;
    private int blue = 0;
    private int yellow = 0;
    private int green = 0;

    public void Pop(BubbleColor color) {
        switch (color) {
            case BubbleColor.Red:
                IncrementRed();
                break;
            case BubbleColor.Green:
                IncrementGreen();
                break;
            case BubbleColor.Yellow:
                IncrementYellow();
                break;
            case BubbleColor.Blue:
                IncrementBlue();
                break;
        }
    }

    private void IncrementRed() {
        red += 1;
        incrementRedEvent.Invoke(red);
    }

    private void IncrementGreen() {
        green += 1;
        incrementGreenEvent.Invoke(green);
    }

    private void IncrementYellow() {
        yellow += 1;
        incrementYellowEvent.Invoke(yellow);
    }

    private void IncrementBlue() {
        blue += 1;
        incrementBlueEvent.Invoke(blue);
    }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }
}