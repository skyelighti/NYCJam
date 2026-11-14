using UnityEngine;

public abstract class SceneController : MonoBehaviour, IState
{
    [SerializeField] SceneNames sceneName;
    public SceneNames SceneName => sceneName;

    public virtual void EnterState()
    {
        
    }

    public virtual void ExitState()
    {

    }

    public virtual void UpdateState()
    {

    }

}
