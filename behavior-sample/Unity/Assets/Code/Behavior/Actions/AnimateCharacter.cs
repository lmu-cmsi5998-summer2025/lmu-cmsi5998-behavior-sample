using UnityEngine;

public class AnimateCharacter : MonoBehaviour
{
    [SerializeField] float m_RotationSpeed = 1f;
    Vector3 m_LastPosition;
    Animator m_Animator;

    void Start()
    {
        m_Animator = GetComponent<Animator>();
        m_LastPosition = transform.position;
    }

    void LateUpdate()
    {
        Vector3 velocity = (transform.position - m_LastPosition) / Time.deltaTime;
        m_LastPosition = transform.position;

        if (velocity.magnitude > 0.1f)
        {
            m_Animator.SetFloat("Speed", velocity.magnitude);
            var dirRot = Quaternion.LookRotation(velocity.normalized);
            transform.rotation = Quaternion.Slerp(transform.rotation, dirRot, m_RotationSpeed * Time.deltaTime);
            transform.eulerAngles = new Vector3(0, transform.eulerAngles.y, 0);
        }
        else
        {
            m_Animator.SetFloat("Speed", 0);
        }
    }
}
