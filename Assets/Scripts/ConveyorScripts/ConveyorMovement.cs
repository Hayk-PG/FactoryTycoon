using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConveyorMovement : MainBehaviour, IMovement {
   
    [Header("Reverse Direction")]
    [SerializeField] bool reverse;
    public bool Reverse => reverse;

    [Header("Direction")]
    [SerializeField] Transform[] directionPoints;

    [Header("Speed")]
    [SerializeField] float speed;

    //IMOVEMENT INTERFACE
    public Vector3 Direction { get => !Reverse ? (directionPoints[1].position - directionPoints[0].position).normalized : (directionPoints[0].position - directionPoints[1].position).normalized; set { } }
    public float Speed { get => speed; set { } }   
    public Rigidbody Rb { get; set; }
    public Transform _Transform { get; set; }
    public bool ShouldStopped { get; set; }


    //BASE TRANSFORM
    public override float ColX { get => 0; set { } }
    public override float ColY { get => Col.bounds.size.y; set { } }
    public override float ColZ { get => 0; set { } }

    
}
