using Unity.Cinemachine;
using UnityEngine;

[RequireComponent(typeof(CinemachineCamera))]
public class CameraLook : MonoBehaviour
{
    [Header("References")]
    CinemachineCamera cinemachine;
    PlayerInputs playerInputs;
    [SerializeField] float looksSpeed = 5;

    private void OnEnable()
    {
        
    }

    private void Awake()
    {
        cinemachine = GetComponent<CinemachineCamera>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
