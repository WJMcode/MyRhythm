using UnityEngine;
using UnityEngine.InputSystem;

public class DJInputManager : MonoBehaviour
{
    private PlayerInput playerInput;

    void Awake()
    {
        playerInput = GetComponent<PlayerInput>();
    }

    public void OnDJing(InputValue value)
    {
        Debug.Log("DJing input received");
    }
}
