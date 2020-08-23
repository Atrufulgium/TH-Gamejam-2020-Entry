using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Movement : MonoBehaviour {

    public static Movement Player;

    public LayerMask groundMask;

    Rigidbody2D rigidBody;
    Animator anim;
    public Transform t;

    public int CoyoteCooldown = 15;
    public int CoyoteTime = 15;
    public float HDrag = 0.1f;
    public float Speed = 1f;
    public float JumpHeight = 10f;
    public float MaxSpeed = 2f;

    public string currentClip = "";
    int animIndex;
    int downtime = 0; //Ik heb geen downtime want gamejam :(
    bool wasFromSlide = false;

    bool fakePush;
    bool inLevel3;

    bool previousGrounded = false;

    float jumpStart;
    float jumpPeak;

    ComboText OtterC;
    ComboText EagleC;
    int hits = 0;

    // imagine, knowing that unity's animator is a state machine and this effort
    // is *probably* not necessary.
    string[] allowsHorizontal = new[] { "Run", "Slide", "Jump", "Freefall" };
    string[] allowsJump = new[] { "Run" };
    string[] allowsStartSlide = new[] { "Run", "Jump" };
    string[] allowsEndSlide = new[] { "Slide" };
    string[] allowsSafeLanding = new[] { "Jump", "Freefall" };
    string[] allowsUnsafeLanding = new[] { "DiagAttack" };
    string[] allowsDiagAttack = { "Jump" };

    string[] airborneStates = { "Jump", "Freefall", "DiagAttack" };

    void Start() {
        EverythingMoves.MoveSpeed = GameData.DefaultSpeed;
        Score.PlayerScore = 0;
        Player = this;
        OtterC = ComboText.ComboTexts["Otter"];
        EagleC = ComboText.ComboTexts["Eagle"];
        Ghost.OtterCombo = 0;
        Ghost.EagleCombo = 0;
        rigidBody = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        t = GetComponent<Transform>();
        animIndex = anim.GetLayerIndex("Base Layer");

        if (Shards.shards != null)
            Shards.shards.Clear();
        Shards.Generate(200);
        if (AudioManager.manager != null)
            AudioManager.PlayMusic();
        GameData.Paused = true;
        hits = 0;
        inLevel3 = SceneManager.GetActiveScene().name == "Stage 3";
    }

    void Update() {
        if (GameData.Paused) {
            if (anim.speed == 1)
                anim.speed = 0;
            return;
        }
        if (anim.speed == 0)
            anim.speed = 1;
        EverythingMoves.MoveSpeed += 0.0000230f;
        if (anim.GetCurrentAnimatorClipInfoCount(animIndex) != 0)
            currentClip = anim.GetCurrentAnimatorClipInfo(animIndex)[0].clip.name;

        if (Input.GetKeyDown(KeyCode.R))
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);

        // Check from both sides
        bool grounded = CheckGround(0.3f);
        if (grounded)
            CoyoteTime = CoyoteCooldown;
        else if (CoyoteTime > 0)
            CoyoteTime--;

        // ====================
        // JUMPING/ENDING SLIDE
        // ====================
        if (currentClip == "Jump") {
            jumpPeak = Mathf.Max(jumpPeak, t.position.y);
        }
        if (Input.GetKeyDown(GameData.Jump) && CoyoteTime > 0 && allowsJump.Contains(currentClip)) {
            Jump();
        }
        Vector2 vel = rigidBody.velocity;
        if (Input.GetKeyUp(GameData.Jump)) {
            if (vel.y > 0) {
                vel.y = 0;
                rigidBody.velocity = vel;
            }
            downtime = 0;
            wasFromSlide = false;
        }
        if (Input.GetKeyDown(GameData.Jump) && allowsEndSlide.Contains(currentClip))
            anim.SetTrigger("EndSlide");
        if (Input.GetKey(GameData.Jump)) {
            downtime++;
            // It wasn't intended this caused mid-air jumps
            // But it is, now.
            if (downtime == 9 && wasFromSlide && allowsJump.Contains(currentClip))
                Jump();
        }

        // ==================
        // LANDING & FREEFALL
        // ==================
        if ((grounded && !previousGrounded) || (grounded && airborneStates.Contains(currentClip))) {
            if (allowsSafeLanding.Contains(currentClip))
                anim.SetTrigger("SafeLanding");
            else if (allowsUnsafeLanding.Contains(currentClip))
                anim.SetTrigger("UnsafeLanding");
        }

        // ===================
        // HORIZONTAL MOVEMENT
        // ===================
        // Input, otherwise drag in input direction.
        // Not using rigidbody's drag because vertical should have no drag.
        bool dragL = false, dragR = false;
        if (Input.GetKey(GameData.Left) && allowsHorizontal.Contains(currentClip)) {
            rigidBody.AddForce(Vector2.left * Speed, ForceMode2D.Impulse);
        } else
            dragL = true;
        if (Input.GetKey(GameData.Right) && allowsHorizontal.Contains(currentClip)) {
            rigidBody.AddForce(Vector2.right * Speed, ForceMode2D.Impulse);
        } else
            dragR = true;

        vel = rigidBody.velocity;
        if (dragL && vel.x < 0) {
            vel.x *= HDrag;
        } else if (dragR && vel.x > 0) {
            vel.x *= HDrag;
        }
        if (vel.x < -MaxSpeed)
            vel.x = -MaxSpeed;
        else if (vel.x > MaxSpeed)
            vel.x = MaxSpeed;
        rigidBody.velocity = vel;

        Vector3 p = t.position;
        bool hit = p.y < -8.5f;
        hit |= p.x < -10;
        if (p.x > 9.6f)
            p.x = 9.6f;
        t.position = p;
        if (hit)
            GetHit();

        // ==================
        // DIAG ATTACK/SLIDES
        // ==================
        if (Input.GetKeyDown(GameData.Down)) {
            if (grounded || (rigidBody.velocity.y < 0 && CheckGround(3f))) {
                if (allowsStartSlide.Contains(currentClip)) {
                    wasFromSlide = true;
                    anim.SetTrigger("StartSlide");
                }
            } else if (allowsDiagAttack.Contains(currentClip)) {
                anim.SetTrigger("DiagAttack");
            }
        }

        previousGrounded = grounded;
    }

    void Jump() {
        jumpStart = t.position.y;
        rigidBody.AddForce(Vector2.up * JumpHeight, ForceMode2D.Impulse);
        AudioManager.PlaySFX(SFX.Jump);
        anim.SetTrigger("Jump");
        anim.ResetTrigger("UnsafeLanding");
        anim.ResetTrigger("SafeLanding");
        anim.ResetTrigger("DiagAttack");
    }

    bool CheckGround(float dist) {
        return Physics2D.Raycast(t.position, Vector2.down, dist, groundMask).collider != null
            || Physics2D.Raycast((Vector2)t.position - new Vector2(0.4f, 0f), Vector2.down, dist, groundMask).collider != null;
    }

    bool CheckFront(float dist) {
        return Physics2D.Raycast(t.position + Vector3.up * 0.1f, Vector2.right, dist, groundMask).collider != null;
    }

    public void GetHit() {
        Shards.ExplodeAt(t.position, Color.white, 10, 20);
        Shards.ExplodeAt(t.position, Color.red, 10, 20);
        Shards.ExplodeAt(t.position, new Color(0.231f, 0.38f, 0.235f), 80, 10);
        Shards.ExplodeAt(t.position, new Color(0.988f, 0.933f, 0.914f), 50, 8);
        Shards.ExplodeAt(t.position, new Color(0.831f, 0.831f, 0.831f), 30, 7);
        t.position = new Vector3(0f, 11f);
        rigidBody.velocity = Vector2.zero;
        anim.SetTrigger("Freefall");
        EverythingMoves.MoveSpeed = GameData.DefaultSpeed;
        Ghost.EagleCombo = 0;
        Ghost.OtterCombo = 0;
        OtterC.StopCombo();
        EagleC.StopCombo();
        AudioManager.PlaySFX(SFX.Die);
        CameraFixer.Screenshake();
        GameData.PlayerDied = true;
        hits++;
        if (hits == 2) {
            Pause.Die();
            gameObject.SetActive(false);
        }
    }

    private void LateUpdate() {
        return;
        Debug.Log("aaaa");
        fakePush = CheckFront(0.05f);
        if (fakePush && inLevel3) {
            t.position += Vector3.left * EverythingMoves.MoveSpeed;
            fakePush = false;
        }
    }
}
