using UnityEngine;
using UnityEngine.AI;
using UnityEngine.InputSystem;

public class PlayerAgentModule_Movement : PlayerAgentModule
{
    [SerializeField] InputActionReference m_InputAction;
    [SerializeField] float m_MoveSpeed = 1f;
    NavMeshAgent m_NavMeshAgent;
    InputAction m_MoveAction;
    Vector2 m_LastMoveInputValue;

    public override void EnableModuleInput(PlayerInput playerInput, InputActionMap activeActionMap)
    {
        m_NavMeshAgent = controlledCharacter.GetComponent<NavMeshAgent>();
        m_MoveAction = activeActionMap.FindAction(m_InputAction.action.name);
        m_MoveAction.performed += OnMoveActionPerformed;
        m_MoveAction.canceled += OnMoveActionCanceled;
    }

    public override void DisableModuleInput(PlayerInput playerInput, InputActionMap activeActionMap)
    {
        if (m_MoveAction != null)
        {
            m_MoveAction.performed -= OnMoveActionPerformed;
            m_MoveAction.canceled -= OnMoveActionCanceled;
        }
    }

    public override void UpdateModule(float deltaTime)
    {
        if (m_LastMoveInputValue == Vector2.zero)
        {
            return;
        }

        Vector3 dir = new Vector3(m_LastMoveInputValue.x, 0, m_LastMoveInputValue.y);
        if (m_NavMeshAgent)
        {
            m_NavMeshAgent.Move(dir * m_MoveSpeed * deltaTime);
        }
        else
        {
            controlledCharacter.Move(dir * m_MoveSpeed * deltaTime);
        }
    }

    private void OnMoveActionPerformed(InputAction.CallbackContext obj)
    {
        m_LastMoveInputValue = obj.ReadValue<Vector2>();
    }

    private void OnMoveActionCanceled(InputAction.CallbackContext obj)
    {
        m_LastMoveInputValue = Vector2.zero;
    }
}
