using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class touchCoin : MonoBehaviour
{

    [SerializeField]
    private DataManager data;

    [SerializeField]
    private AudioSource source;

    private coinManager cm;
    // Start is called before the first frame update
    void Start()
    {
        data = FindObjectOfType<DataManager>().GetComponent<DataManager>();
        cm = FindObjectOfType<coinManager>().GetComponent<coinManager>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && this.enabled)
        {
            this.enabled = false;
            source.Play();
            data.AddCoin();
            cm.ShowCoin();
            Destroy(this.gameObject,.2f);
        }
    }
}
