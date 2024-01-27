using UnityEngine;

//to move an object, we require a RigidBody
[RequireComponent(typeof(Rigidbody2D))]

public class Movement : MonoBehaviour {   //variables for customization (think of it as movement settings)
    public float speed = 8.0f;
    public float speedMultiplier = 1.0f;
    public Vector2 initialDirection;
    // raycast a layer (only checking this layer for raycast); this LayerMask to check obstacle layer when checking for collisions. raycast a layer (only checking this layer for raycast)
    public LayerMask obstacleLayer;
    
    // reference to the rigid body
    public new Rigidbody2D rigidbody { get; private set; }
    
    // direction currently on
    public Vector2 direction { get; private set; }
    // the next direction player is taking (queueing the direction)
    public Vector2 nextDirection { get; private set; }
    public Vector3 startingPosition { get; private set; }

    // note: will almost always establish refernces to other objects in awake
    private void Awake() {
        this.rigidbody = GetComponent<Rigidbody2D>();
        this.startingPosition = this.transform.position;
    }

    private void Start() {
        ResetState();
    }

     //reset everything back to their orginal values
    public void ResetState() {
        //setting inital values
        this.speedMultiplier = 1.0f;
        this.direction = this.initialDirection;
        //clear direction
        this.nextDirection = Vector2.zero;
        this.transform.position = this.startingPosition;
        // for ghost so they can stop passing through walls
        this.rigidbody.isKinematic = false;
        this.enabled = true;
    }

    private void Update() {
        // Try to move in the next direction while it's queued to make movements
        if (this.nextDirection != Vector2.zero) {
            SetDirection(this.nextDirection);
        }
    }


    // like update, but called at a fixed time interval
   //Called automatically by Unity and where we actually apply movement physics
    private void FixedUpdate() {
        //get current position
        Vector2 position = this.rigidbody.position;
        //how much its moving/translating
        Vector2 translation = this.direction * this.speed * this.speedMultiplier * Time.fixedDeltaTime;

        this.rigidbody.MovePosition(position + translation);
    }

    // Set the direction
    public void SetDirection(Vector2 direction, bool forced = false) {
        // Only set the direction if the tile in that direction is available
        // otherwise we set it as the next direction so it'll automatically be
        // set when it does become available
        if (forced || !Occupied(direction))
        {
            // move that direction
            this.direction = direction;
            //clear out queued up direction
            this.nextDirection = Vector2.zero;
        }
        else
        {
            this.nextDirection = direction;
        }
    }

    //check to see if Tile in the given direction is occupied
    public bool Occupied(Vector2 direction) {
        //  hit takes (the position, a size, a angle, direction cast is going, this distance it is checking, and a layermask/layer its being applied to)
        RaycastHit2D hit = Physics2D.BoxCast(this.transform.position, Vector2.one * 0.75f, 0.0f, direction, 1.5f, this.obstacleLayer);
        //if it hits will return the colider, if it doesn't returns null
        return hit.collider != null;


    }
}
