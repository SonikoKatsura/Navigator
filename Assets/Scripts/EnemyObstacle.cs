using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyObstacle : MonoBehaviour
{
    [SerializeField] float speed; // Referencia a la velocidad de movimiento
    [SerializeField] Rigidbody rb; // Referencia al Rigidbody2D
    [SerializeField] float distance; // Referencia a la distancia m�xima de movimiento
    Vector3 originalPosition; // Posici�n original del enemigo
    bool rightMotion = true; // Direcci�n del movimiento
                             // Esta funci�n se ejecuta una vez al comienzo
    public void Start()
    {
        // Obtenemos la posici�n original del enemigo
        originalPosition = transform.position;
    }
    // Esta funci�n se ejecuta cada X tiempo
    public void FixedUpdate()
    {
        // Si la posici�n original del enemigo sumada a la distancia m�xima
        // es inferior a la posici�n actual el enemigo sigue movi�ndose a la derecha.
        if ((transform.position.z < (originalPosition.z + distance)) && rightMotion)
        {
            transform.eulerAngles = new Vector3(0f, 0f, 0f);
            rb.velocity = new Vector3(rb.velocity.x, rb.velocity.y, speed);
        }
        else if ((transform.position.z >= (originalPosition.z + distance)) && rightMotion) rightMotion = false;
        // Si la posici�n original del enemigo rest�ndole la distancia m�xima
        // es inferior a la posici�n actual el enemigo sigue movi�ndose a la izquierda.
        if ((transform.position.z > (originalPosition.z - distance)) && !rightMotion)
        {
            transform.eulerAngles = new Vector3 (0f, 180f, 0f);
            rb.velocity = new Vector3(rb.velocity.x, rb.velocity.y, -speed);
        }
        else if ((transform.position.z <= (originalPosition.z - distance)) && !rightMotion) rightMotion = true;
    }
}