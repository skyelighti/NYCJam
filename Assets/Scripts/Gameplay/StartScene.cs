using UnityEngine;

public class StartScene : SceneController
{
    public override void EnterState()
    {
        Time.timeScale = 1f;

        if (GameManager.Instance != null)
        {
            GameManager.Instance.SwapMap(ActionMap.UI);
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
