using UnityEngine;

public class ParallaxController : MonoBehaviour
{
    private Transform cam;
    private Vector3 camStartPos;
    private float distanceX;

    private GameObject[] backgrounds;
    private Material[] mats;
    private float[] backSpeed;

    private float farthestBack = 0.0001f; // tránh chia 0

    [Range(0.01f, 1f)]
    public float parallaxSpeed = 0.05f;

    private string textureProperty = "_MainTex"; // default Built-in

    void Start()
    {
        cam = Camera.main.transform;
        camStartPos = cam.position;

        int backCount = transform.childCount;

        backgrounds = new GameObject[backCount];
        mats = new Material[backCount];
        backSpeed = new float[backCount];

        // Detect URP shader property
        // URP thường dùng "_BaseMap"
        textureProperty = "_MainTex";

        for (int i = 0; i < backCount; i++)
        {
            backgrounds[i] = transform.GetChild(i).gameObject;

            Renderer r = backgrounds[i].GetComponent<Renderer>();
            if (r == null)
            {
                Debug.LogWarning("Background " + backgrounds[i].name + " không có Renderer!");
                continue;
            }

            mats[i] = r.material;

            if (mats[i].HasProperty("_BaseMap"))
                textureProperty = "_BaseMap";
        }

        CalculateBackSpeed(backCount);
    }

    void CalculateBackSpeed(int backCount)
    {
        farthestBack = 0.0001f;

        // tìm layer xa nhất
        for (int i = 0; i < backCount; i++)
        {
            if (backgrounds[i] == null) continue;

            float zDist = backgrounds[i].transform.position.z - cam.position.z;

            if (zDist > farthestBack)
                farthestBack = zDist;
        }

        // tính speed cho từng layer
        for (int i = 0; i < backCount; i++)
        {
            if (backgrounds[i] == null) continue;

            float zDist = backgrounds[i].transform.position.z - cam.position.z;
            backSpeed[i] = 1 - (zDist / farthestBack);
        }
    }

    void LateUpdate()
    {

        distanceX = cam.position.x - camStartPos.x;
        transform.position = new Vector3(cam.position.x, transform.position.y, 0);

        for (int i = 0; i < backgrounds.Length; i++)
        {
            Debug.Log("Test");

            if (mats[i] == null) continue;

            float speed = backSpeed[i] * parallaxSpeed;

            // chia nhỏ distance để tránh bị trượt quá nhanh
            float offsetX = distanceX * speed;

            mats[i].SetTextureOffset(textureProperty, new Vector2(offsetX, 0));
        }
    }
}