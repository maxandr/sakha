using UnityEngine;
using System.Collections;

public class Hit : MonoBehaviour
{
    private bool immortal_because_invisible;
    private UnityStandardAssets._2D.PlatformerCharacter2D player;
    void Awake() {
        if (gameObject.tag == "Player")
        {
            player = gameObject.GetComponent<UnityStandardAssets._2D.PlatformerCharacter2D>();
        }
    }

    public bool Hitted(int dmg)
    {
        if(!immortal_because_invisible && ((player && !player.teleporting)||!player))
        {
            gameObject.GetComponent<UnityStandardAssets._2D.UnitParams>().Hitted(dmg);
            return true;
        }
        return false;
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
