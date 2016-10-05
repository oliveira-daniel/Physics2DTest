using UnityEngine;
using System.Collections;

public class WaterDetector : MonoBehaviour {

    // Mais detalhes em:
    // https://gamedevelopment.tutsplus.com/tutorials/creating-dynamic-2d-water-effects-in-unity--gamedev-14143

    void OnTriggerEnter2D(Collider2D hit)
    {
        Rigidbody2D rbFromHit = hit.GetComponent<Rigidbody2D>();
        if (rbFromHit != null)
        {
            transform.parent.GetComponent<WaterLine>().Splash(transform.position.x, rbFromHit.velocity.y * rbFromHit.mass / 40f);
        }
        
    }

}
