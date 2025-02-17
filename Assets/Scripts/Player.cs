using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public float speed,//Velocidad
                 distRaycast;//Vamos a tener un raycast para tener en cuenta si se choca o no con las cosas

    float h, //Horizontal y vertical
          v;

    Vector2 destination;
    Animator anim;

    public LayerMask layerNotWalkable;//Vamos a establecer para decirle que contra esos objetos no los traspase, que se choque

    bool walking;//Para saber si estamos caminando o no

    private void Awake()
    {
        anim = GetComponent<Animator>();
    }

    void Start()
    {
        //Inicialiazo destination a la posición inicial del Player
        destination = transform.position;
    }

   
    void Update()
    {
        PlayerMovement();
        PlayerAnimation();
    }

    public void PlayerMovement()
    {
        //Si walking no es igual a true, es decir si no estamos caminando
        if(!walking)
        {
            //Walking lo ponemos a true, que camine
            walking = true;
            
            //Mapeo de teclas, (-1,0,1)
            h = Input.GetAxisRaw("Horizontal");
            v = Input.GetAxisRaw("Vertical");

            //Siempre h no sea igual a 0 y además con todo aquello con lo que choque mi raycast no contenga
            //el layer que nosotros hemos creado, que se mueva
            if (h != 0 && !Physics.Raycast(transform.position, h * Vector2.right, distRaycast, layerNotWalkable))
            {
                destination = (Vector2)transform.position + (h * Vector2.right);
            }
            //Si el valor de v no es 0 nos estamos moviendo en vertical
            else if (v != 0 && !Physics.Raycast(transform.position, v * Vector2.up, distRaycast, layerNotWalkable))
            {
                destination = (Vector2)transform.position + (v * Vector2.up);
            }
        }
        //Walkin es igual a true, estamos en movimiento
        else
        {
            transform.position = Vector2.MoveTowards(transform.position, destination, speed * Time.deltaTime);
            
            //Estamos haciendo que el personaje se mueva de cuadricula en cuadricula
            if(Vector2.Distance(transform.position, destination) < 0.01f)
            {
                //Si no le indicamos la nueva posición del personaje se generan imprecisiones
                transform.position = destination;
                walking = false;
            }
        }
    }

    private void PlayerAnimation()
    {
        //Estamos modificando con anim.SetFloat el valor que tiene VelocityX y VelocityY. Cuano pasemos a pulsar
        //la tecla pasara de 0 a 1 por ejemplo
                       //Nombre del parametro y le metemos el mapeo de las teclas (h)
        anim.SetFloat("VelocityX", h);
        anim.SetFloat("VelocityY", v);
    }
}
