using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Entity : MonoBehaviour
{
    [SerializeField]
    protected Rigidbody rigidbody;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.W))
        {
            LaunchInDirection(transform.up, 5f);
        }
    }

    public IEnumerator MoveInDirection(Vector3 direction, float time, float speedPerSecond)
    {
        float startTime = Time.time;
        yield return new WaitForFixedUpdate();


        while (Time.time < startTime + time)
        {
            rigidbody.MovePosition(transform.position + (direction * speedPerSecond / 50f));
            yield return new WaitForFixedUpdate();
        }

        yield return null;
    }

    public IEnumerator MoveToPosition(Vector3 position, float time)
    {
        float distance = Vector3.Distance(position, transform.position);

        float speed = distance / time;

        float startTime = Time.time;

        while (Time.time < startTime + time)
        {
            rigidbody.MovePosition(Vector3.MoveTowards(transform.position, position, speed / 50f));
            yield return new WaitForFixedUpdate();
        }
    }

    public void LaunchInDirection(Vector3 direction, float force)
    {
        rigidbody.AddForce(direction * force, ForceMode.Impulse);
    }
}