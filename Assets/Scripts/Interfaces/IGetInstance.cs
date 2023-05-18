using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;

public interface IGetInstance<T> {
    T Instance { get; }
}
