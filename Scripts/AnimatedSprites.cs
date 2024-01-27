
using UnityEngine;

// Require an object to have a sprite renderer
[RequireComponent(typeof(SpriteRenderer))]
public class AnimatedSprites : MonoBehaviour
{
    // hold the reference to the sprite renderer
    public SpriteRenderer spriteRenderer { get; private set;}

    // store each sprites that creates the animation
    public Sprite [] sprites;

    // time between switching a sprite
    public float animationTime = 0.25f;

    // keep the index of sprite that is currently in use
    public int animationFrame {get; private set;}

    // bool to know if anitmation loops
    public bool loop = true;



    //method that called before the game starts
    private void Awake(){

        this.spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void Start(){
        // repeatedly invoke an aciton/method/  takes method, inital time, time after the first
        InvokeRepeating(nameof(Advance), this.animationTime, this.animationTime);
    }


    // Animates the object based on the frames.
    private void Advance() {
        if (!this.spriteRenderer.enabled)
        {
            return;
        }

        this.animationFrame++;

        if (this.animationFrame >= this.sprites.Length && this.loop)
        {
            this.animationFrame = 0;
        }

        if (animationFrame >= 0 && this.animationFrame < this.sprites.Length)
        {
            this.spriteRenderer.sprite = this.sprites[this.animationFrame];
        }
    }

    // Restarts the animation.
    public void Restart() {
        this.animationFrame = -1;

        Advance();
    }


}
