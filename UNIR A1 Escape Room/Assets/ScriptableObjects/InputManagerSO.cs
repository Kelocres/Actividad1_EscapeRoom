using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[CreateAssetMenu(menuName = "InputManager")]
public class InputManagerSO : ScriptableObject {

    // Para vincularlo con el InputManager
    Controls misControles;

    // Para llamar al Player cuando se pulsen
    // los botones
    public event Action OnSaltar;
    public event Action<Vector2> OnMover;

    // Esto se ejecutará cuando se cree una instancia
    private void OnEnable()
    {
        misControles = new Controls();
        misControles.Gameplay.Enable();
        misControles.Gameplay.Saltar.started += Saltar;

        //Los valores started son demasiado pequeños para
        // para el input de movimiento
        misControles.Gameplay.Mover.performed += Mover;

        // Para detectar que el joystick está en punto muerto
        // y detener al personaje
        misControles.Gameplay.Mover.canceled += Mover;
    }


    private void Saltar(InputAction.CallbackContext contexto)
    {
        //Debug.Log("Salto");
        OnSaltar?.Invoke();
    }

    // Sólo se va a ejecutar cuando SE ACTUALICE el input de movimiento
    private void Mover(InputAction.CallbackContext contexto)
    {
        //Para obtener la información Vector 2 del Input
        //Debug.Log(contexto.ReadValue<Vector2>());

        OnMover?.Invoke(contexto.ReadValue<Vector2>());
    }
}
