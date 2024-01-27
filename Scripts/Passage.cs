
using UnityEngine;

public class Passage : MonoBehaviour  {
    //Either left or right connection
    public Transform connection;


    private void OnTriggerEnter2D(Collider2D other) {
        // get the current position
        Vector3 position = other.transform.position;
       
        // get the new position to the x and y
        position.x = this.connection.position.x;
        position.y = this.connection.position.y;
       
        // set the position
        other.transform.position = position;
    }
}
