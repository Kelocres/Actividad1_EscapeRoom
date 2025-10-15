using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    //private Rigidbody rb;
    private CharacterController controller;

    // Al usar Input Manager, estas variables
    // no son necesarias
    //private float inputH;
    //private float inputV;

    //Para moverse según el eje de la cámara
    [SerializeField] private Transform camara;
    private Vector3 direccionMovimiento;
    private Vector3 direccionInput;

    // Para la gravedad
    [Header("Deteccion suelo")]
    [SerializeField] private Transform pies;
    [SerializeField] private float radioDeteccion;
    [SerializeField] private LayerMask queEsSuelo;
    private Vector3 velocidadVertical;
    [SerializeField] private float factorGravedad = -9.8f;

    [Header("Para los saltos")]
    [SerializeField] 
    private float alturaDeSalto = 5f;

    //Para los controles
    [SerializeField]
    private InputManagerSO inputManager;
    [SerializeField]
    private float velocidadMovimiento = 10f;

    private void OnEnable()
    {
        inputManager.OnSaltar += Saltar;
        inputManager.OnMover += Mover;
    }

    void Start()
    {
        //rb = GetComponent<Rigidbody>();
        controller = GetComponent<CharacterController>();

        //Para esconder el cursor del ratón
        Cursor.lockState = CursorLockMode.Locked;
    }

    // Update is called once per frame
    void Update()
    {
        //Movimiento
        direccionMovimiento = 
            camara.forward * direccionInput.z + camara.right * direccionInput.x;
        
        //Para evitar que la dirección sea hacia arriba o abajo
        direccionMovimiento.y = 0f;
        controller.Move(direccionMovimiento * velocidadMovimiento * Time.deltaTime);

        if (direccionMovimiento.sqrMagnitude > 0)
            RotarHaciaDestino();

        //Si hemos aterrizado...
        if (EstoyEnSuelo() && velocidadVertical.y < 0)
        {
            //Entonces reseteo mi velocidad vertical
            velocidadVertical.y = 0f;
        }

        AplicarGravedad();
    }

    private void Mover(Vector2 contexto)
    {
        //No hace falta normalizarlo, porque ya está normalizado
        direccionInput = new Vector3(contexto.x, 0, contexto.y);
    }

    private void Saltar()
    {
        //Debug.Log("Saltar");
        if (EstoyEnSuelo())
        {
            velocidadVertical.y = Mathf.Sqrt(-2 * factorGravedad * alturaDeSalto);
        }
    }

    private void RotarHaciaDestino()
    {
        Quaternion rotacionObjetivo = Quaternion.LookRotation(direccionMovimiento);
        transform.rotation = rotacionObjetivo;
    }


    // Para detectar si estás tocando el suelo
    private bool EstoyEnSuelo()
    {
        return Physics.CheckSphere(pies.position, radioDeteccion, queEsSuelo);
    }

    private void AplicarGravedad()
    {
        velocidadVertical.y += factorGravedad * Time.deltaTime;
        controller.Move(velocidadVertical * Time.deltaTime);
    }

    // Para ver en el Editor dónde está el detector
    private void OnDrawGizmos()
    {
        Gizmos.DrawSphere(pies.position, radioDeteccion);
    }
}
