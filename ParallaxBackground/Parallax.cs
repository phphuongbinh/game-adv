using UnityEngine;

public class Parallax : MonoBehaviour
{
    public Transform cam;
    public float parallaxMultiplier = 0.5f;

    private Vector3 lastCamPos;
    private float spriteWidth;

    void Start()
    {
        if (cam == null) cam = Camera.main.transform;

        lastCamPos = cam.position;

        SpriteRenderer sr = GetComponentInChildren<SpriteRenderer>();
        if (sr != null)
        {
            spriteWidth = sr.bounds.size.x;
        }
    }

    void LateUpdate()
    {
        Vector3 delta = cam.position - lastCamPos;

        // Parallax movement
        transform.position += new Vector3(delta.x * parallaxMultiplier, 0, 0);

        lastCamPos = cam.position;

        // Loop background
        float camDistance = cam.position.x - transform.position.x;

        if (camDistance > spriteWidth)
        {
            transform.position += new Vector3(spriteWidth, 0, 0);
        }
        else if (camDistance < -spriteWidth)
        {
            transform.position -= new Vector3(spriteWidth, 0, 0);
        }
    }
}