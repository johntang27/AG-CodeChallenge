using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShapeGenerator : MonoBehaviour, ICreatePolygon
{
    public GameObject vertexPrefab;
    public bool defaultHex = false;

    private int _sides;
    private Vector3[] _vertices;
    private int[] _meshTriangles;

    public int GetSides => _sides;

    private void Start()
    {
        if(defaultHex) CreatePolygonMesh();
    }

    void CreatePolygonMesh(int sides = 6, float radius = 3)
    {
        _sides = sides;

        Mesh mesh = new Mesh();        

        _vertices = new Vector3[sides + 1];

        _vertices[0] = Vector3.zero; //establish the center point

        _meshTriangles = new int[sides * 3]; //the vertices that will generate the required triangles to form the polygon

        Vector2[] colliderPoints = new Vector2[sides]; //use vertices to set the polygon collider

        for (int i = 0; i < sides; i++) //generate vertex based on the sides of the polygon
        {
            float x = radius * Mathf.Sin(2 * Mathf.PI * i / (float)sides);
            float y = radius * Mathf.Cos(2 * Mathf.PI * i / (float)sides);            

            Vector3 vertex = new Vector3(x, y, 0);
            _vertices[i + 1] = vertex;
            colliderPoints[i] = vertex;

            _meshTriangles[i * 3] = 0; //the first vertex for the triangle will always be the center point

            if (i == 0) //the first vertex created will also be the last vertex used for the last triangle
            {
                _meshTriangles[1] = _meshTriangles[_meshTriangles.Length - 1] = i + 1;
            }
            else //all other vertex will be used twice in back to back triangles
            {
                _meshTriangles[i * 3 - 1] = _meshTriangles[i * 3 + 1] = i + 1;
            }
        }

        mesh.vertices = _vertices;
        mesh.triangles = _meshTriangles;
        GetComponent<MeshFilter>().mesh = mesh;
        //GetComponent<MeshCollider>().sharedMesh = mesh;
        GetComponent<PolygonCollider2D>().SetPath(0, colliderPoints);

        #region test manual vertex hexagon
        //vertices = new Vector3[7];

        //vertices[0] = new Vector3(0, 0);
        //vertices[1] = new Vector3(3, -4);
        //vertices[2] = new Vector3(-2, -4);
        //vertices[3] = new Vector3(-5, 0);
        //vertices[4] = new Vector3(-3, 4);
        //vertices[5] = new Vector3(2, 4);
        //vertices[6] = new Vector3(5, 0);       

        //mesh.vertices = vertices;

        //mesh.triangles = new int[] { 0, 1, 2, 0, 2, 3, 0, 3, 4, 0, 4, 5, 0, 5, 6, 0, 6, 1 };

        //GetComponent<MeshFilter>().mesh = mesh;

        //for (int i = 0; i < vertices.Length; i++)
        //{
        //    GameObject go = Instantiate(vertexPrefab, vertices[i], Quaternion.identity);
        //    go.name = i.ToString();
        //}
        #endregion
    }

    void CreateRandomMesh()
    {
        int randSides = Random.Range(3, 20);
        float randRadius = Random.Range(1, 5);      

        CreatePolygonMesh(randSides, randRadius);
    }

    void ChangeColorRandom()
    {
        Color newColor = new Color(
            Random.Range(0f, 1f),
            Random.Range(0f, 1f),
            Random.Range(0f, 1f));

        GetComponent<MeshRenderer>().material.color = newColor;
    }

    void ICreatePolygon.CreatePolygon(int sides, float radius)
    {
        CreatePolygonMesh(sides, radius);
    }

    void ICreatePolygon.CreateRandomPolygon()
    {
        CreateRandomMesh();
    }

    void ICreatePolygon.ChangeRandomColor()
    {
        ChangeColorRandom();
    }

    public void InitMove()
    {
        StartCoroutine(MoveAndRotate());
    }

    IEnumerator MoveAndRotate()
    {
        while(transform.position.y > MeshSpawner.END_LIMIT)
        {
            transform.Rotate(Vector3.forward * Time.deltaTime * 45f);
            transform.Translate(Vector3.down * Time.deltaTime * _sides * 0.75f, Space.World);
            yield return null;
        }

        MeshSpawner.Instance.AddToPool(this);
    }
}
