using UnityEngine;

public class MeshCombiner : MonoBehaviour
{
    public GameObject object1;
    public GameObject object2;

    void Start()
    {
        CombineMeshes();
    }

    void CombineMeshes()
    {
        MeshFilter meshFilter1 = object1.GetComponent<MeshFilter>();
        MeshFilter meshFilter2 = object2.GetComponent<MeshFilter>();

        Mesh mesh1 = meshFilter1.mesh;
        Mesh mesh2 = meshFilter2.mesh;

        CombineInstance[] combineInstances = new CombineInstance[2];

        combineInstances[0].mesh = mesh1;
        combineInstances[0].transform = object1.transform.localToWorldMatrix;

        combineInstances[1].mesh = mesh2;
        combineInstances[1].transform = object2.transform.localToWorldMatrix;

        Mesh combinedMesh = new Mesh();
        combinedMesh.CombineMeshes(combineInstances, true, true);

        MeshFilter meshFilterCombined = gameObject.GetComponent<MeshFilter>();
        if (meshFilterCombined == null)
        {
            meshFilterCombined = gameObject.AddComponent<MeshFilter>();
        }
        meshFilterCombined.mesh = combinedMesh;

        MeshCollider meshColliderCombined = gameObject.GetComponent<MeshCollider>();
        if (meshColliderCombined == null)
        {
            meshColliderCombined = gameObject.AddComponent<MeshCollider>();
        }
        meshColliderCombined.sharedMesh = combinedMesh;
    }
}
