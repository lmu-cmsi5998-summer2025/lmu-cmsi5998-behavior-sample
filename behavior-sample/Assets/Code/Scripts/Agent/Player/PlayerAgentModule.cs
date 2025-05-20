using UnityEngine.InputSystem;

public abstract class PlayerAgentModule : AgentModule<PlayerAgent>
{
    public PlayerInput PlayerInput { get; private set; }

    public abstract void EnableModuleInput(PlayerInput playerInput, InputActionMap activeActionMap);

    public abstract void DisableModuleInput(PlayerInput playerInput, InputActionMap activeActionMap);

    public override void InitModule(Agent controller)
    {
        base.InitModule(controller);

        PlayerInput = moduleOwner.playerInput;
    }
}