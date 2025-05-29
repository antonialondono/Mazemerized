using UnityEngine;
using UnityEngine.XR;

[RequireComponent(typeof(CharacterController))]
public class XRPlayerGravity : MonoBehaviour
{
    public float gravity = -9.81f;
    public float fallingSpeed = 0f;
    public float additionalHeight = 0.2f;

    public Transform xrCamera; // Arrastra aquí la cámara VR (Main Camera)
    private CharacterController characterController;

    void Start()
    {
        characterController = GetComponent<CharacterController>();
    }

    void Update()
    {
        // Ajustar altura del CharacterController para que siga la cabeza
        UpdateCharacterHeight();

        // Aplicar gravedad
        if (characterController.isGrounded && fallingSpeed < 0)
        {
            fallingSpeed = -1f;
        }
        else
        {
            fallingSpeed += gravity * Time.deltaTime;
        }

        Vector3 move = new Vector3(0, fallingSpeed, 0);
        characterController.Move(move * Time.deltaTime);
    }

    void UpdateCharacterHeight()
    {
        // Calcular altura de la cabeza respecto al XR Origin
        float headHeight = Mathf.Clamp(xrCamera.localPosition.y, 1, 2.5f);
        characterController.height = headHeight + additionalHeight;
        characterController.center = new Vector3(xrCamera.localPosition.x, characterController.height / 2, xrCamera.localPosition.z);
    }
}
