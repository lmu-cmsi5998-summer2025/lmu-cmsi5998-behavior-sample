using UnityEngine;

public class FovMesh : MonoBehaviour
{
    public float radius = 10;
    public int segments = 32;
    public float angle = 45;
    public float height = 0.5f;
    public LayerMask CollisionLayers;
    MeshFilter m_Filter;

    void Start()
    {
        //create mesh filter if not present
        m_Filter = gameObject.GetComponent<MeshFilter>();

        if (m_Filter == null)
        {
            m_Filter = gameObject.AddComponent<MeshFilter>();
        }

        //create mesh renderer if not present
        MeshRenderer m_Renderer = gameObject.GetComponent<MeshRenderer>();
        if (m_Renderer == null)
        {
            m_Renderer = gameObject.AddComponent<MeshRenderer>();
            m_Renderer.material = new Material(Shader.Find("Standard"));
        }

        Mesh diskMesh = new Mesh();
        diskMesh.MarkDynamic();
        diskMesh.name = "Disk";
        m_Filter.mesh = diskMesh;
    }

    // Update is called once per frame
    void Update()
    {
        transform.eulerAngles = new Vector3(0, transform.eulerAngles.y, 0);
        UpdateDisk();
    }

    void UpdateDisk()
    {
        Vector3[] vertices = new Vector3[segments + 1];
        int[] triangles = new int[segments * 3];

        vertices[0] = Vector3.zero;
        float angleStep = angle / segments;
        for (int i = 1; i < segments + 1; i++)
        {
            float angleRad = Mathf.Deg2Rad * angleStep * i + (Mathf.PI - Mathf.Deg2Rad * angle) / 2;

            Vector3 directionWorld = transform.TransformDirection(new Vector3(Mathf.Cos(angleRad), 0, Mathf.Sin(angleRad)));

            float collisionRadius = radius;
            RaycastHit hit;
            if (Physics.Raycast(transform.position + Vector3.up * height, directionWorld, out hit, radius, CollisionLayers))
            {
                collisionRadius = hit.distance;
            }

            vertices[i] = new Vector3(Mathf.Cos(angleRad), 0, Mathf.Sin(angleRad)) * collisionRadius;
        }

        for (int i = 0; i < segments - 1; i++)
        {
            triangles[i * 3] = 0;
            triangles[i * 3 + 1] = i + 2;
            triangles[i * 3 + 2] = i + 1;
        }

        m_Filter.mesh.Clear();
        m_Filter.mesh.vertices = vertices;
        m_Filter.mesh.triangles = triangles;
        m_Filter.mesh.RecalculateNormals();
    }
}
