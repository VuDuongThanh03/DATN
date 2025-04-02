using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugDrawGizmos : MonoBehaviour
{
    public static Vector3 PositionDrawGizmos;
    public static float Radius;
    public static Color GizmosColor;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public static void SetDrawGizmos(Vector3 position, float radius, Color color){
        PositionDrawGizmos = position;
        Radius = radius;
        GizmosColor = color;
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(PositionDrawGizmos,Radius);
    }
}
