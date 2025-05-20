using UnityEngine;

/// <summary>
/// Abstraction to control a Character.
/// </summary>
public abstract class Agent : MonoBehaviour
{
    public enum EnableInputBehaviour
    {
        OnAwake,
        OnEnable,
        OnStart,
        Manual
    }

    public enum DisableInputBehaviour
    {
        OnDisable,
        Manual
    }

    [Header("Controller")]
    [SerializeField]
    protected CharacterController m_controlledCharacter;

    public CharacterController ControlledCharacter => m_controlledCharacter;

    [SerializeField]
    private EnableInputBehaviour m_enableInputBehaviour = EnableInputBehaviour.OnAwake;

    [SerializeField, Tooltip("DisableInput is always called OnDestroy.")]
    private DisableInputBehaviour m_disableInputBehaviour = DisableInputBehaviour.Manual;

    public abstract void EnableInput();

    public abstract void DisableInput();

    protected virtual void Awake()
    {
        if (m_enableInputBehaviour == EnableInputBehaviour.OnAwake)
        {
            EnableInput();
        }
    }

    protected virtual void Start()
    {
        if (m_enableInputBehaviour == EnableInputBehaviour.OnStart)
        {
            EnableInput();
        }
    }

    protected virtual void UpdateController(float deltaTime)
    { }

    protected virtual void OnEnable()
    {
        if (m_enableInputBehaviour == EnableInputBehaviour.OnEnable)
        {
            EnableInput();
        }
    }

    protected virtual void OnDisable()
    {
        if (m_disableInputBehaviour == DisableInputBehaviour.OnDisable)
        {
            DisableInput();
        }
    }
}

public abstract class Agent<T> : Agent
    where T : AgentModule
{
    [Header("Modules")]
    [SerializeField]
    protected T[] m_modules;

    [SerializeField] private bool m_autoCaptureModule = true;

    [SerializeField, Tooltip("Is the controller use in standalone without a character?")]
    private bool m_isStandalone = false;

    public abstract bool isAI { get; }
    public bool IsStandalone => m_isStandalone;

    public bool TryGetModule<ModuleType>(out ModuleType outModule) where ModuleType : AgentModule
    {
        outModule = null;
        for (int i = 0, c = m_modules.Length; i < c; ++i)
        {
            var module = m_modules[i];
            if (module.GetType() == typeof(ModuleType))
            {
                outModule = module as ModuleType;
                return true;
            }
        }

        return false;
    }

    protected override void Awake()
    {
        if (!m_controlledCharacter && !m_isStandalone)
        {
            Debug.LogWarning($"No Character set on {this}, searching for one in the parent hierarchy...");
            m_controlledCharacter = transform.parent.GetComponentInChildren<CharacterController>();
            Debug.Assert(m_controlledCharacter, $"No Character found for {this.transform.parent.gameObject.name}/{this}");
        }

        if (m_autoCaptureModule)
        {
            CaptureCharacterControllerModules();
        }

        foreach (var module in m_modules)
        {
            module.InitModule(this);
        }

        base.Awake();
    }

    protected virtual void OnDestroy()
    {
        DisableInput();
    }

    protected override void UpdateController(float deltaTime)
    {
        if (!m_isStandalone && m_controlledCharacter == null)
        {
            return;
        }

        foreach (var module in m_modules)
        {
            if (!module.IsAvailable())
            {
                continue;
            }

            module.UpdateModule(deltaTime);
        }
    }

    protected override void OnEnable()
    {
        foreach (var module in m_modules)
        {
            module.enabled = true;
        }

        base.OnEnable();
    }

    protected override void OnDisable()
    {
        foreach (var module in m_modules)
        {
            module.enabled = false;
        }

        base.OnDisable();
    }

    protected virtual void OnValidate()
    {
        if (m_autoCaptureModule)
        {
            CaptureCharacterControllerModules();
        }
    }

    protected virtual void CaptureCharacterControllerModules()
    {
        m_modules = GetComponents<T>();
    }
}
