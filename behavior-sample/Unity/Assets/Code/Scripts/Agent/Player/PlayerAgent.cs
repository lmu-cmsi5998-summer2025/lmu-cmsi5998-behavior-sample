using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerAgent : Agent<PlayerAgentModule>
{
    [Header("Player Controller")]
    [SerializeField]
    protected PlayerInput m_PlayerInput;

    [SerializeField, Tooltip("Action map used by this controller to get bindings from.")]
    private string m_ActionMapName = "Player";

    public override bool isAI => false;
    public PlayerInput playerInput => m_PlayerInput;
    public bool isInputReady { get; private set; } = false;


    private InputActionMap m_ActionMap;

    protected override void Awake()
    {
        base.Awake();

        CapturePlayerInputAndSetBehaviour();
    }

    public virtual void MountPlayerInput(PlayerInput player, bool enableInput = true)
    {
        m_PlayerInput = player;

        if (enableInput)
        {
            EnableInput();
        }
    }

    public virtual void UnMountPlayerInput()
    {
        DisableInput();
        m_PlayerInput = null;
    }

    public override void EnableInput()
    {
        Debug.Assert(m_PlayerInput != null, $"[{Time.frameCount}] {this}: PlayerInput is required");

        m_PlayerInput.ActivateInput();
        m_PlayerInput.SwitchCurrentActionMap(m_ActionMapName);
        m_ActionMap = m_PlayerInput.actions.FindActionMap(m_ActionMapName);

        isInputReady = true;

        foreach (var extension in m_modules)
        {
            if (!extension.enabled)
            {
                continue;
            }

            extension.EnableModuleInput(playerInput, m_ActionMap);
        }
    }

    public override void DisableInput()
    {
        m_PlayerInput.DeactivateInput();
        m_ActionMap = null;
        isInputReady = false;

        foreach (var extension in m_modules)
        {
            if (!extension.enabled)
            {
                continue;
            }

            extension.DisableModuleInput(playerInput, m_ActionMap);
        }
    }

    protected override void OnDestroy()
    {
        UnMountPlayerInput();
    }

    private void Update()
    {
        UpdateController(Time.deltaTime);
    }

    protected override void OnValidate()
    {
        base.OnValidate();
        CapturePlayerInputAndSetBehaviour();
    }

    private void CapturePlayerInputAndSetBehaviour()
    {
        if (m_PlayerInput == null)
        {
            m_PlayerInput = GetComponentInParent<PlayerInput>();
        }

        if (m_PlayerInput)
        {
            m_PlayerInput.notificationBehavior = PlayerNotifications.InvokeCSharpEvents;
        }
    }
}
