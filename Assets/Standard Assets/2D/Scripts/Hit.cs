using UnityEngine;
using System.Collections;

public class Hit : MonoBehaviour
{
    private bool immortal_because_invisible;
    public void Hitted(int dmg)
    {
        if(!immortal_because_invisible)
        {
            gameObject.GetComponent<UnityStandardAssets._2D.UnitParams>().Hitted(dmg);
        }
    }
    public void Update()
    {
        Plane[] planes = GeometryUtility.CalculateFrustumPlanes(Camera.main);
        if (GeometryUtility.TestPlanesAABB(planes, GetComponent<Collider2D>().bounds))
            immortal_because_invisible = false;
        else
            immortal_because_invisible = true;
    }
}
