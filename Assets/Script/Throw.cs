using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Throw : MonoBehaviour
{
    [SerializeField]
    private GameObject effect;

    public Collider Enemy;
    public GameObject FindedEnemy;
    public int speed = 0;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Enemy == null && FindedEnemy == null)
        {
            Destroy(gameObject);
            return;
        }
        if (Enemy != null)
        {
            transform.Translate((Enemy.transform.position - transform.position).normalized * speed * Time.deltaTime);
        }
        else
        {
            transform.Translate((FindedEnemy.transform.position - transform.position).normalized * speed * Time.deltaTime);
        }
    }
    

    private void OnTriggerEnter(Collider other)
    {
        if(other == Enemy || other.gameObject == FindedEnemy)
        {
            Instantiate(effect, transform.position, Quaternion.identity);
            Destroy(gameObject);
        }
    }
}
