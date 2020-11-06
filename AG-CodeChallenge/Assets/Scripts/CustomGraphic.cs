using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CustomGraphic : Graphic, ICreatePolygon
{
    [SerializeField]
    private int sides = 6;
    [SerializeField]
    private float radius = 100;

    public int Sides
    {
        get => sides;
        set
        {
            sides = value;
            SetVerticesDirty();
        }
    }

    public float Radius
    {
        get => radius;
        set
        {
            radius = value;
            SetVerticesDirty();
        }
    }

    protected override void OnPopulateMesh(VertexHelper vh)
    {
        vh.Clear();

        UIVertex vert = UIVertex.simpleVert;
        vert.color = this.color;
        vert.uv0 = Vector2.zero;

        vh.AddVert(vert);

        for (int i = 0; i < sides; i++) //generate vertex based on the sides of the polygon
        {
            float x = radius * Mathf.Sin(2 * Mathf.PI * i / (float)sides);
            float y = radius * Mathf.Cos(2 * Mathf.PI * i / (float)sides);

            Vector3 vertex = new Vector3(x, y, 0);

            UIVertex vert2 = UIVertex.simpleVert;
            vert.color = this.color;
            vert2.position = vertex;
            vh.AddVert(vert2);

            int last = i + 2;
            if (i == sides - 1) last = 1;

            vh.AddTriangle(0, i + 1, last);
        }

        //vh.AddTriangle(0, 1, 2);
        //vh.AddTriangle(0, 2, 3);
        //vh.AddTriangle(0, 3, 4);
        //vh.AddTriangle(0, 4, 5);
        //vh.AddTriangle(0, 5, 6);
        //vh.AddTriangle(0, 6, 1);
    }

    void CreateRandomGraphicMesh()
    {
        Sides = Random.Range(3, 20);
        Radius = Random.Range(100, 500);
    }

    void CreateGraphicMesh(int sidesInputVal, float radiusInputVal)
    {
        Sides = sidesInputVal;
        Radius = radiusInputVal;
    }

    public void CreatePolygon(int sides, float radius)
    {
        CreateGraphicMesh(sides, radius);
    }

    public void CreateRandomPolygon()
    {
        CreateRandomGraphicMesh();
    }

    public void ChangeRandomColor()
    {
        Color newColor = new Color(
            Random.Range(0f, 1f),
            Random.Range(0f, 1f),
            Random.Range(0f, 1f));

        material.color = newColor;
    }
}
