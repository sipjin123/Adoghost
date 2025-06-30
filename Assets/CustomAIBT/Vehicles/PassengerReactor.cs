using UnityEngine;

public class PassengerReactor : MonoBehaviour
{
    [Header("Reaction Settings")]
    public float wobbleAmount = 0.05f;
    public float wobbleSpeed = 3f;

    private Vector3 baseLocalPosition;
    private float seedOffset;

    void Start()
    {
        baseLocalPosition = transform.localPosition;
        seedOffset = Random.Range(0f, 100f); // Make each passenger unique
    }

    public void ReactToBump(float time)
    {
        float wobble = Mathf.PerlinNoise(time * wobbleSpeed, seedOffset);
        float offsetY = (wobble - 0.5f) * 2f * wobbleAmount;

        Vector3 targetPos = baseLocalPosition + new Vector3(0f, offsetY, 0f);
        transform.localPosition = Vector3.Lerp(transform.localPosition, targetPos, Time.deltaTime * 5f);
    }
}
