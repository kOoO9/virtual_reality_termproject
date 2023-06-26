using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeshMassSpringExample : MonoBehaviour
{
    // 공 프리팹
    public GameObject ballPrefab;

    // 공들 사이를 연결할 강성
    public float stiffness = 10.0f;

    // Edge 생성
    public List<Vector2Int> edges = new List<Vector2Int>();

    // 생성한 공 배열
    public List<GameObject> balls;

    // Start is called before the first frame update
    void Awake()
    {
        // mesh vertex 위치에 공 생성
        GenerateBalls();
        GenerateSprings();
    }

    // Update is called once per frame
    void Update()
    {
        // 공 위치로부터 mesh 업데이트
        UpdateFromBallToMesh();
    }

    void GenerateBalls()
    {
        // MeshFilter 컴포넌트 가져오기
        MeshFilter meshFilter = GetComponent<MeshFilter>();
        if (meshFilter == null)
        {
            Debug.LogError("MeshFilter 컴포넌트를 찾을 수 없습니다.");
            return;
        }

        // Mesh 컴포넌트 가져오기
        Mesh mesh = meshFilter.mesh;
        if (mesh == null)
        {
            Debug.LogError("Mesh 컴포넌트를 찾을 수 없습니다.");
            return;
        }

        // 버텍스 정보 가져오기
        Vector3[] vertices = mesh.vertices;

        // 공들을 담을 빈 객체를 자식으로 생성
        GameObject ballsGameObject = new GameObject("Balls");
        ballsGameObject.transform.SetParent(transform);

        // 배열 초기화
        balls = new List<GameObject>();


        // 각 버텍스마다 프리팹을 이용해 게임 오브젝트 생성
        for (int i = 0; i < vertices.Length; i++)
        {
            // 국부좌표계 값을 쓰는 Vertex를 전역좌표값으로 변환
            Vector3 globalPosition = transform.TransformPoint(vertices[i]);

            // 해당 전역 좌표계 위치에 공 생성
            GameObject ball = Instantiate(ballPrefab, globalPosition, Quaternion.identity);

            // Rigidbody 컴포넌트 추가
            Rigidbody ballRigidbody = ball.GetComponent<Rigidbody>();
            if (ballRigidbody == null)
            {
                ballRigidbody = ball.AddComponent<Rigidbody>();
            }

            // 생성한 오브젝트를 공들 오브젝트의 자식으로 할당
            ball.transform.SetParent(ballsGameObject.transform);

            // 배열에 추가
            balls.Add(ball);
        }
    }

    void GenerateSprings()
    {
        // MeshFilter 컴포넌트 가져오기
        MeshFilter meshFilter = GetComponent<MeshFilter>();
        if (meshFilter == null)
        {
            Debug.LogError("MeshFilter 컴포넌트를 찾을 수 없습니다.");
            return;
        }

        // Mesh 컴포넌트 가져오기
        Mesh mesh = meshFilter.mesh;
        if (mesh == null)
        {
            Debug.LogError("Mesh 컴포넌트를 찾을 수 없습니다.");
            return;
        }

        // Mesh의 삼각형 정보로부터 edge 정보 추출
        int[] triangles = mesh.triangles;

        // 각 삼각형에 대해 edge 정보 추출
        for (int i = 0; i < triangles.Length; i += 3)
        {
            int vertexIndex1 = triangles[i];
            int vertexIndex2 = triangles[i + 1];
            int vertexIndex3 = triangles[i + 2];

            Vector2Int edge1 = new Vector2Int(vertexIndex1, vertexIndex2);
            Vector2Int edge2 = new Vector2Int(vertexIndex2, vertexIndex3);
            Vector2Int edge3 = new Vector2Int(vertexIndex3, vertexIndex1);

            AddEdge(edge1, ref edges);
            AddEdge(edge2, ref edges);
            AddEdge(edge3, ref edges);
        }

        // 스프링들을 담을 빈 객체 생성
        GameObject springs = new GameObject("Springs");
        springs.transform.SetParent(transform);

        // 각 edge 정보를 활용해 두 점을 연결하는 스프링 설정
        foreach (Vector2Int edge in edges)
        {
            GameObject spring = new GameObject("Spring");
            spring.transform.SetParent(springs.transform);

            // 스프링 추가
            SpringJoint springJoint = spring.AddComponent<SpringJoint>();

            // 스프링 두 점 할당
            GameObject ball1 = balls[edge.x];
            GameObject ball2 = balls[edge.y];

            Rigidbody ball1Rigidbody = ball1.GetComponent<Rigidbody>();
            Rigidbody ball2Rigidbody = ball2.GetComponent<Rigidbody>();

            if (ball1Rigidbody == null)
            {
                ball1Rigidbody = ball1.AddComponent<Rigidbody>();
            }

            if (ball2Rigidbody == null)
            {
                ball2Rigidbody = ball2.AddComponent<Rigidbody>();
            }

            springJoint.connectedBody = ball2Rigidbody;
            springJoint.connectedAnchor = Vector3.zero;

            // 스프링 상수 할당
            springJoint.spring = stiffness;
            springJoint.enableCollision = true;
        }
    }

    void UpdateFromBallToMesh()
    {
        // MeshFilter 컴포넌트 가져오기
        MeshFilter meshFilter = GetComponent<MeshFilter>();
        if (meshFilter == null)
        {
            Debug.LogError("MeshFilter 컴포넌트를 찾을 수 없습니다.");
            return;
        }

        // Mesh 컴포넌트 가져오기
        Mesh mesh = meshFilter.mesh;
        if (mesh == null)
        {
            Debug.LogError("Mesh 컴포넌트를 찾을 수 없습니다.");
            return;
        }

        // 버텍스 정보 가져오기
        Vector3[] vertices = mesh.vertices;

        // 전역 좌표계 값을 국부 좌표계 값으로 변환하여 대입
        for (int i = 0; i < balls.Count; i++)
        {
            Vector3 localPosition = transform.InverseTransformPoint(balls[i].transform.position);
            vertices[i] = localPosition;
        }

        // 업데이트된 버텍스 정보 대입
        mesh.vertices = vertices;

        // 업데이트된 Mesh로부터 Mesh Collider 업데이트
        if (mesh != null)
        {
            // MeshCollider에 Mesh 정보 업데이트
            MeshCollider meshCollider = GetComponent<MeshCollider>();
            meshCollider.sharedMesh = null; // 먼저 null로 설정
            meshCollider.sharedMesh = mesh; // Mesh 정보 설정
        }
    }

    // 추가하는 edge가 기존 edge와 중복되는지 검사 후 추가
    void AddEdge(Vector2Int edge, ref List<Vector2Int> edges)
    {
        bool isDuplicate = false;

        foreach (Vector2Int existingEdge in edges)
        {
            if (existingEdge == edge || existingEdge == new Vector2Int(edge.y, edge.x))
            {
                isDuplicate = true;
                break;
            }
        }

        if (!isDuplicate)
        {
            edges.Add(edge);
        }
    }
}
