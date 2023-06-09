﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IMovement 
{
    Vector3 Direction { get; set; }
    float Speed { get; set; }
    Rigidbody Rb { get; set; }
    Transform _Transform { get; set; }
    bool ShouldStopped { get; set; }




}
