using System.Collections;
using UnityEngine;

public class GrassSway : MonoBehaviour
{
    [Header("Sway Settings")]
    public float swayAngle = 15f;
    public float swaySpeed = 10f;
    public float returnSpeed = 5f;

    private Quaternion originalRotation;
    private Coroutine swayCoroutine;

    void Start()
    {
        originalRotation = transform.rotation;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            float dir = other.transform.position.x > transform.position.x ? -1f : 1f;

            if (swayCoroutine != null)
                StopCoroutine(swayCoroutine);

            swayCoroutine = StartCoroutine(Sway(dir));
        }
    }

    IEnumerator Sway(float dir)
    {
        Quaternion targetRotation = Quaternion.Euler(0, 0, swayAngle * dir);

        float t = 0;
        while (t < 1)
        {
            t += Time.deltaTime * swaySpeed;
            transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, t);
            yield return null;
        }

        t = 0;
        while (t < 1)
        {
            t += Time.deltaTime * returnSpeed;
            transform.rotation = Quaternion.Lerp(transform.rotation, originalRotation, t);
            yield return null;
        }

        transform.rotation = originalRotation;
    }
}