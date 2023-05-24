using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemManager : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private Rigidbody _rigidbody;

    [SerializeField] private bool _push;
    private bool _isPushed;

    private List<Vector3> _queuedPushes = new List<Vector3>();




    private void Update()
    {
        if (_push)
        {
            PushItem(Vector3.left);
            _push = false;
        }
    }

    public void PushItem(Vector3 direction)
    {
        _queuedPushes.Add(direction);

        if (_isPushed)
        {
            return;
        }

        StartCoroutine(Test(direction, 2));
    }

    private IEnumerator Test(Vector3 direction, float time)
    {
        print("aa");

        _isPushed = true;

        Vector3 initialPosition = _rigidbody.position;
        Vector3 targetPosition = initialPosition + direction;
        Vector3 currentPosition = Vector3.zero;

        float currentDistance = Vector3.Distance(initialPosition, targetPosition);
      
        while (currentDistance > 0.05f)
        {         
            currentPosition = Vector3.Lerp(_rigidbody.position, targetPosition, time * Time.fixedDeltaTime);
            currentDistance = Vector3.Distance(_rigidbody.position, targetPosition);
            _rigidbody.MovePosition(currentPosition);

            yield return new WaitForSeconds(Time.fixedDeltaTime);
        }

        _rigidbody.position = targetPosition;
        _queuedPushes.RemoveAt(0);

        if(_queuedPushes.Count > 0)
        {
            StartCoroutine(Test(_queuedPushes[0], 2));
            yield break;
        }

        _isPushed = false;
    }
}
