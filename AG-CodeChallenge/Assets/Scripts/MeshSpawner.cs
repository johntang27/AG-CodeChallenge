using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeshSpawner : Singleton<MeshSpawner>
{
    public ShapeGenerator meshPrefab;

    float _screenHeight;
    float _screenWidth;

    Vector3 spawnCoord = Vector3.zero;

    public const float END_LIMIT = -4f;

    public List<ShapeGenerator> availableMesh = new List<ShapeGenerator>();

    private void Start()
    {
        _screenHeight = Screen.height;
        _screenWidth = Screen.width;
    }

    public void SpawnMesh()
    {
        int randSides = Random.Range(3, 7);
        float spawnX = Random.Range(50f, _screenWidth - 50f);

        spawnCoord = Camera.main.ScreenToWorldPoint(new Vector2(spawnX, _screenHeight + 50f));
        spawnCoord = new Vector3(spawnCoord.x, spawnCoord.y, 0);

        ShapeGenerator existingMesh = availableMesh.Find(x => x.GetSides == randSides);

        if(existingMesh == null)
        {
            //Debug.Log("Polygon sides: " + randSides + " not in pool");

            ShapeGenerator mesh = Instantiate(meshPrefab, spawnCoord, Quaternion.identity);
            mesh.GetComponent<ICreatePolygon>().CreatePolygon(randSides, 0.5f);
            mesh.GetComponent<ICreatePolygon>().ChangeRandomColor();
            mesh.InitMove();
        }
        else
        {
            availableMesh.Remove(existingMesh);
            existingMesh.gameObject.SetActive(true);
            existingMesh.transform.position = spawnCoord;
            existingMesh.GetComponent<ICreatePolygon>().ChangeRandomColor();
            existingMesh.InitMove();
        }        
    }

    public void AddToPool(ShapeGenerator mesh)
    {
        mesh.gameObject.SetActive(false);
        availableMesh.Add(mesh);        
    }
}
