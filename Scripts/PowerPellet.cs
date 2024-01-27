using UnityEngine;

//PowerPellet inherits from Pellet (A PowerPellet is a type of Pellet)
public class PowerPellet : Pellet {
    public float duration = 8.0f;

    //overriding Eat method from Pellet.cs
    protected override void Eat() {
        //create/finds a reference to the game manager
        FindObjectOfType<GameManager>().PowerPelletEaten(this);
    }

}