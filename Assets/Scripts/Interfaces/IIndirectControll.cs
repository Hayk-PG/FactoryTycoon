using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IIndirectControll 
{
    /// <summary>
    /// Stops the gameobject's work (IIndirect)
    /// </summary>
    void Stop();

    /// <summary>
    /// Starts the gameobject's work (IIndirect)
    /// </summary>
    void Run();
}
