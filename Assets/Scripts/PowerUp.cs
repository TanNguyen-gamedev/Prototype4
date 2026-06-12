using Unity.VisualScripting;
using UnityEngine;

public class PowerUp : MonoBehaviour
{
    [SerializeField] private float _rotationRate = 30f;
    [SerializeField] private float _bobHeight = 0.5f;
    [SerializeField] private float _bobSpeed = 2f;
    private Vector3 startPosition;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        startPosition = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(0, _rotationRate * Time.deltaTime, 0);
        float deltaBob = startPosition.y + Mathf.Sin(Time.time * _bobSpeed) * _bobHeight;
        transform.position = new Vector3(startPosition.x,Mathf.Abs(deltaBob) ,startPosition.z);
    }
}
