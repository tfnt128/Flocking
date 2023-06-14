using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class FlockManager : MonoBehaviour
{
    //Declarando variáveis e configurando-as.
    public GameObject fishPrefab; 
    public int numFishes = 20;
    public GameObject[] allFishes;
    public Vector3 swimLimits = new Vector3(5, 5, 5); 
    public Vector3 goalPos;
    
    [Header("Flock Configuration")]
    [Range(0.0f, 5.0f)]
    public float minSpeed;
    [Range(0.0f, 5.0f)]
    public float maxSpeed;
    [Range(1.0f, 10.0f)]
    public float neighbourDistance;
    [Range(0.0f, 5.0f)]
    public float rotationSpeed; 
    
    void Start()
    {
        //Inicia o array e o armaneza nos peixes.
        allFishes = new GameObject[numFishes]; 
        
        // Intancia em posições aleatória os peixes, porém dentro do limite(swin limits).
        for (int i = 0; i < numFishes; i++)
        {
            Vector3 pos = this.transform.position + new Vector3(Random.Range(-swimLimits.x, swimLimits.x),
                                                                Random.Range(-swimLimits.y, swimLimits.y),
                                                                Random.Range(-swimLimits.z, swimLimits.z));
            allFishes[i] = (GameObject)Instantiate(fishPrefab, pos, Quaternion.identity);
            allFishes[i].GetComponent<Flock>().manager = this; // Set the manager of the fish
        }
        // Atualiza a posição final como a posição do managers position.
        goalPos = this.transform.position; 
    }
    
    // No update atualiza a a posição final para ser a posição do manager position.
    void Update()
    {
        goalPos = this.transform.position; 
        
        //Atualiza aleatoriamente a posição final dentro do limite.
        if (Random.Range(0, 100) < 10)
        {
            goalPos = this.transform.position + new Vector3(Random.Range(-swimLimits.x, swimLimits.x),
                                                            Random.Range(-swimLimits.y, swimLimits.y),
                                                            Random.Range(-swimLimits.z, swimLimits.z));
        }
    }
}