using UnityEngine;

public class camera_script : MonoBehaviour
{
    [SerializeField] private Transform target;           // Referencia al transform del jugador
    [SerializeField] private float smoothSpeed = 0.125f; // Velocidad suave de desplazamiento

    private Vector3 offset;            // Distancia inicial entre la c�mara y el jugador

    void Start()
    {
        // Calculamos la distancia inicial entre la c�mara y el jugador
        offset = transform.position - target.position;
    }

    void LateUpdate()
    {
        // Calculamos la posici�n objetivo de la c�mara
        Vector3 desiredPosition = target.position + offset;

        // Utilizamos Lerp para mover suavemente la c�mara hacia la posici�n objetivo
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);

        // Actualizamos la posici�n de la c�mara
        transform.position = smoothedPosition;

        
        //private Vector3 velocity = Vector3.zero; 
        // Utilizamos SmoothDamp para mover suavemente la c�mara hacia la posici�n objetivo
        //transform.position = Vector3.SmoothDamp(transform.position, desiredPosition, ref velocity, smoothTime);

    }
}

