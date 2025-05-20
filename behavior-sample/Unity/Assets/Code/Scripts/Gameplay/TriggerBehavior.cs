using System;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class TriggerBehavior : MonoBehaviour
{
    [SerializeField] private string m_Tag = "Pickup";

    public Rigidbody body { get; private set; }

    public event Action<TriggerBehavior> onTrigger;

    public virtual void OnTrigger(Collider other)
    {

    }

    void Awake()
    {
        gameObject.tag = m_Tag;

        body = GetComponent<Rigidbody>();
        body.useGravity = false;
        body.isKinematic = true;
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == m_Tag)
        {
            return;
        }

        OnTrigger(other);
        onTrigger?.Invoke(this);
    }
}
