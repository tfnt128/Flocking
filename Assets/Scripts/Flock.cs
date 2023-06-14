using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flock : MonoBehaviour
{
    //Declarando variáveis.
    public FlockManager manager;
    float speed;
    bool turning = false;
    
    //Setando a velocidade no start com uma margem de minimo e máximo.
    void Start()
    {
        speed = Random.Range(manager.minSpeed, manager.maxSpeed);
    }
    
    //Pegar os limites no update.
    void Update()
    {
        
        Bounds b = new Bounds(manager.transform.position, manager.swimLimits * 2);
        RaycastHit hit = new RaycastHit();
        Vector3 direction = manager.transform.position - transform.position;
        //Se dentro dos limites turning fica verdadeira e muda para uma nova direção.
        if (!b.Contains(transform.position))
        {
            turning = true;
            direction = manager.transform.position - transform.position;
        }
        //Se não a direção continua a mesma.
        else if (Physics.Raycast(transform.position, this.transform.forward * 50, out hit))
        {
            turning = true;
            direction = Vector3.Reflect(this.transform.forward, hit.normal);
        }
        else
            turning = false;
        //Se turning é verdadeiro, rotaciona na direção desejada
        if (turning)
        {
            transform.rotation = Quaternion.Slerp(transform.rotation,
            Quaternion.LookRotation(direction),
            manager.rotationSpeed * Time.deltaTime);
        }
        //Se não ele chama o ApplyRules
        else
        {
            if (Random.Range(0, 100) < 10)
                speed = Random.Range(manager.minSpeed,
                manager.maxSpeed);
            if (Random.Range(0, 100) < 20)
                ApplyRules();
        }
        transform.Translate(0, 0, Time.deltaTime * speed);
    }
        void ApplyRules()
        {
            GameObject[] gos;
            gos = manager.allFishes;

            Vector3 vcentre = Vector3.zero;
            Vector3 vavoid = Vector3.zero;
            float gspeed = 0.01f;
            float nDistance;
            int groupSize = 0;

            
            
            //Na lógica do flocking ele encontra o centro médio e a velocidade de um grupo de objetos ("gos")
            // em relação ao objeto atual.
            foreach (GameObject g in gos)
            {
                if (g != this.gameObject)
                {
                    nDistance = Vector3.Distance(g.transform.position, this.transform.position);
                    if (nDistance <= manager.neighbourDistance)
                    {
                        vcentre = g.transform.position;
                        groupSize++;

                        if (nDistance < 1f)
                        {
                            vavoid = vavoid + (this.transform.position - g.transform.position);
                        }

                        Flock anotherFlock = g.GetComponent<Flock>();
                        gspeed = gspeed + anotherFlock.speed;
                    }
                }
            }
            //Ajusta a rotação do objeto atual em direção ao centro do grupo,
            //levando em consideração uma posição para evitar.
            if (groupSize > 0)
            {
                vcentre = vcentre / groupSize;
                speed = gspeed / groupSize;

                Vector3 direction = (vcentre - vavoid) - transform.position;
                if (direction != Vector3.zero)
                {
                    transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(direction), manager.rotationSpeed * Time.deltaTime);
                }
            }
        }
    } 
