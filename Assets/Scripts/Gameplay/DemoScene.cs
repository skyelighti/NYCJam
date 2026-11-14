using UnityEngine;

public class DemoScene : SceneController
{
    public override void EnterState()
    {
        Time.timeScale = 1f;

        if (GameManager.Instance != null)
        {
            GameManager.Instance.SwapMap(ActionMap.Gameplay);
        }
    }

    public override void ExitState()
    {
        if (GameManager.Instance != null)
        {
            GameManager.Instance.inputActions?.Disable();
        }
    }
}
