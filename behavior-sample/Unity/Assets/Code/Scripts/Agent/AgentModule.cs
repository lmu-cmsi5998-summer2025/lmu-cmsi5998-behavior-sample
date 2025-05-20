using UnityEngine;

public abstract class AgentModule : MonoBehaviour
{
    public CharacterController controlledCharacter { get; private set; }

    public virtual void InitModule(Agent controller)
    {
        controlledCharacter = controller.ControlledCharacter;
    }

    public virtual void UpdateModule(float deltaTime)
    {
    }

    public virtual bool IsAvailable()
    {
        return isActiveAndEnabled;
    }
}

public abstract class AgentModule<T> : AgentModule
    where T : Agent
{
    public T moduleOwner { get; private set; }

    public override void InitModule(Agent controller)
    {
        moduleOwner = controller as T;
        base.InitModule(controller);
    }
}