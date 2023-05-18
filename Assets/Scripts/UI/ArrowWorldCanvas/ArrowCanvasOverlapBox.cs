using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowCanvasOverlapBox : MonoBehaviour
{
    /// <summary>
    /// Gets TilesContainer interface
    /// </summary>
    public ISideTile TilesContainer { get; set; }

    void Update() {

        if(Physics.BoxCast(transform.position, new Vector3(0.5f, 0.5f, 0.5f),transform.position, transform.rotation, 0.5f, ObjectsHolder.instance.LayerMasks.TileMask)) {

            TilesContainer.ArrowButtonIsDestroyed = true;
            Destroy(gameObject);
        }
    }
}
