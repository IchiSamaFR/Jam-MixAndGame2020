using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundSpell : MonoBehaviour
{

    public string id;
    private GameObject obj;

    private void OnTriggerEnter2D(Collider2D other) {
        if (other.CompareTag("Player")) {
            obj = other.gameObject;
        }
    }

    private void OnTriggerExit2D(Collider2D other) {
        if (other.CompareTag("Player")) {
            obj = null;
        }
    }


    private void Update() {
        if (obj != null && Input.GetKeyDown(KeyCollection.instance.interact) && obj.GetComponent<PlayerMovement>().canMove) {
            bool can = obj.GetComponent<SpellToolbar>().AddSpell(SpellCollection.instance.GetSpell(id));

            if (can)
            {
                Destroy(this.gameObject);
            }
        }
    }
}
