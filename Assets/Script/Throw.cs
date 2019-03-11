using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Throw : MonoBehaviour
{
    [SerializeField]
    private GameObject effect;

    public Collider Enemy;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Enemy == null)
            Destroy(gameObject);
        transform.Translate((Enemy.transform.position - transform.position).normalized * 5 * Time.deltaTime);
    }
    

    private void OnTriggerEnter(Collider other)
    {
        if(other == Enemy)
        {
            Instantiate(effect, transform.position, Quaternion.identity);
            Destroy(gameObject);
        }
    }
}
