using System.Collections;
using UnityEngine;

/// <summary>
/// Subscribe To Events In SafeMode
/// </summary>
public class SubscribeToEventsInSafeMode : MonoBehaviour
{
    public static SubscribeToEventsInSafeMode instance;

    void Awake() {

        instance = this;
    }

    /// <summary>
    /// Wait until obj is assigned, check null reference: Subscribe to event
    /// </summary>
    /// <param name="waitUntil"></param>
    /// <param name="Subscribe"></param>
    public void Subscribe(bool waitUntil, System.Action Subscribe) {

        StartCoroutine(SubscribeToEventsCoroutine(waitUntil, Subscribe));
    }

    IEnumerator SubscribeToEventsCoroutine(bool waitUntil, System.Action Subscribe) {

        yield return new WaitUntil(() => waitUntil);

        Subscribe();
    }

 










}
