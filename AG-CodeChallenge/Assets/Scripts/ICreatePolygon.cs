using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ICreatePolygon
{
    void CreatePolygon(int sides, float radius);
    void CreateRandomPolygon();
    void ChangeRandomColor();
}
