using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class FieldOfView : MonoBehaviour
{
    private const string WALL = "Wall";

    [SerializeField] private LayerMask layerMask;
    [SerializeField] private Enemy enemy;

    private Vector3 origin;
    private float startingAngle;
    private float fov = 90f;

    public float hitbox { get; set; }

    private Vector3 GetVectorFormAngle(float angle)
    {
        float angleRad = angle * (Mathf.PI/180f);
        return new Vector3(Mathf.Cos(angleRad),Mathf.Sin(angleRad));
    }  

    private float GetAngleFromVectorFloat(Vector3 dir)
    {
        dir = dir.normalized;
        float n = Mathf.Atan2(dir.x , dir.y) * Mathf.Rad2Deg;
        if (n < 0) n += 360;

        return n;
    }

    public Player AttackPlayer()
    {
        int rayCount = 50;
        float angle = startingAngle;
        float angleIncrease = fov / rayCount;

        for (int i = 0; i <= rayCount; i++)
        {
            RaycastHit2D raycastHit2D = Physics2D.Raycast(origin, GetVectorFormAngle(angle), hitbox, layerMask);

            if (raycastHit2D.collider == null)
            {
                
            }
            else
            {
                return raycastHit2D.collider.GetComponent<Player>();
            }
            angle -= angleIncrease;
        }
        return null;
    }

    public void SetOrigin(Vector3 origin)
    {
        this.origin = origin;
    }

    public void SetAimDirection(Vector3 aimDirection)
    {
        startingAngle = -GetAngleFromVectorFloat (aimDirection) + fov / 2;
    }
}
