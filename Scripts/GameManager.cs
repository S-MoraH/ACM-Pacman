using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour {
    // Array that holds the ghosts
    public Ghost[] ghosts;
    // Player object
    public Pacman pacman;
    
    // pellets is a Transform so that we can access all its children/instances
    public Transform pellets;

    public int ghostMultiplier { get; private set; } = 1;

    // int to hold the score with a public getter and a private setter
    public int score { get; private set; }

    // int to hold the lives with a public getter and a private setter
    public int lives { get; private set; }

    // UI Elements
    public Text gameOverText;
    public Text livesText;
    public Text scoreText;


    //First method called when the game starts
    private void Start() {
        NewGame();
    }

    //checked every frame of game
    private void Update() {
        // Checks if a player can restart a game.
        if (Input.anyKeyDown && this.lives <= 0) {
            NewGame();
        }
    }

    // Sets up pacman game.
    private void NewGame() {
        SetScore(0);
        SetLives(3);
        NewRound();
    }

    // Starting a new round of the game/resetting the game
    private void NewRound() {
        
        this.gameOverText.enabled = false;

        // Refreshes the pellets on the game map for a new round.
        foreach (Transform pellet in this.pellets) {
            pellet.gameObject.SetActive(true);
        }
        ResetState();
    }

    // Resets the entire game for pacman.
   private void ResetState() {
        //Reset ghost multiplier
        ResetGhostMultiplier();

        // Refreshes ghosts to their places.
        for (int i = 0; i < this.ghosts.Length; i++) {
            this.ghosts[i].ResetState();
        }

        this.pacman.ResetState();
    }

     // Handles when player loses the game.
    private void GameOver() {
        this.gameOverText.enabled = true;

        for (int i = 0; i < this.ghosts.Length; i++) {
            // deactiveate the ghosts
            this.ghosts[i].gameObject.SetActive(false);
        }
        //deactivate the player/pacman
        this.pacman.gameObject.SetActive(false);
    }

    // set the game score
    private void SetScore(int score) {
        this.score = score;
        this.scoreText.text = score.ToString().PadLeft(2, '0');
    }
    
    // set number of lives
    private void SetLives(int lives) {
        this.lives = lives;
        string count = "";

        for (int i = 0; i < lives; i++) {
            count += "❤︎ ";
        }

        this.livesText.text = count;


    }

    // method that handles when a ghost is eatened
    public void GhostEaten(Ghost ghost) {
        int points = ghost.points * this.ghostMultiplier;
        SetScore(this.score + ghost.points);
        this.ghostMultiplier++;
    }


   // method that handles a player being eaten by ghosts.
    public void PacmanEaten() {

        //start death animation
        this.pacman.DeathSequence();

        // subtract one of the lives
        SetLives(this.lives - 1);

        // Check if player has lives remaining.
        if (this.lives > 0) {
            // ResetState();     This would instantly reset the game
            // Need a grace period, so we invoke it after 4s
            Invoke(nameof(ResetState), 3.0f);       
        } 
        else {
            GameOver();
        }
    }

    public void PelletEaten(Pellet pellet) {
        //eat the pellet
        pellet.gameObject.SetActive(false);

        SetScore(this.score + pellet.points);  

        if (!HasRemainingPellets()){
            this.pacman.gameObject.SetActive(false);
            Invoke(nameof(NewRound), 4.0f);
        }
    }


  public void PowerPelletEaten(PowerPellet pellet) {
        //tell all our ghost to become frightened
        for (int i = 0; i < this.ghosts.Length; i++)
        {
            this.ghosts[i].frightened.Enable(pellet.duration);
        }

        PelletEaten(pellet);
        
        //if one is running cancel it and reinvoke (lets us stack ghosts)
        CancelInvoke();
        Invoke(nameof(ResetGhostMultiplier), pellet.duration);
     
    }

    // Checks if the map contains any more pellets (game over).
  private bool HasRemainingPellets() {
        foreach (Transform pellet in this.pellets) {
            // Check if there is a pellet still active.
            if (pellet.gameObject.activeSelf) {
                return true;
            }
        }

        // All pellets gone.
        return false;
    }

    // resets the multiplier
    private void ResetGhostMultiplier () {
        this.ghostMultiplier = 1;
    }
}
