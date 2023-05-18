using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LayerMasks : MonoBehaviour
{
    [SerializeField] LayerMask tileMask; public LayerMask TileMask => tileMask;
    [SerializeField] LayerMask conveyorMask; public LayerMask ConveyorMask => conveyorMask;
    [SerializeField] LayerMask itemMask; public LayerMask ItemMask => itemMask;
    [SerializeField] LayerMask worldSpaceUI; public LayerMask WorldSpaceUI => worldSpaceUI;
    [SerializeField] LayerMask groundTiles; public LayerMask GroundTiles => groundTiles;
}
