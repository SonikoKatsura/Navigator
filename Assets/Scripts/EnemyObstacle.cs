using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyObstacle : MonoBehaviour
{
    [SerializeField] float speed; // Referencia a la velocidad de movimiento
    [SerializeField] Rigidbody rb; // Referencia al Rigidbody2D
    [SerializeField] float distance; // Referencia a la distancia máxima de movimiento
    Vector3 originalPosition; // Posición original del enemigo
    bool rightMotion = true; // Dirección del movimiento
                             // Esta función se ejecuta una vez al comienzo
    public void Start()
    {
        // Obtenemos la posición original del enemigo
        originalPosition = transform.position;
    }
    // Esta función se ejecuta cada X tiempo
    public void FixedUpdate()
    {
        // Si la posición original del enemigo sumada a la distancia máxima
        // es inferior a la posición actual el enemigo sigue moviéndose a la derecha.
        if ((transform.position.z < (originalPosition.z + distance)) && rightMotion)
        {
            transform.eulerAngles = new Vector3(0f, 0f, 0f);
            rb.velocity = new Vector3(rb.velocity.x, rb.velocity.y, speed);
        }
        else if ((transform.position.z >= (originalPosition.z + distance)) && rightMotion) rightMotion = false;
        // Si la posición original del enemigo restándole la distancia máxima
        // es inferior a la posición actual el enemigo sigue moviéndose a la izquierda.
        if ((transform.position.z > (originalPosition.z - distance)) && !rightMotion)
        {
            transform.eulerAngles = new Vector3 (0f, 180f, 0f);
            rb.velocity = new Vector3(rb.velocity.x, rb.velocity.y, -speed);
        }
        else if ((transform.position.z <= (originalPosition.z - distance)) && !rightMotion) rightMotion = true;
    }
}