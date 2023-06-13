using Godot;
using System;

public class PlayerController : KinematicBody2D
{
	private int speed = 200;
	private int dashSpeed = 300;
	private int jumpHeight = 100;
	private int gravity = 175;
	private float friction = .09f;
	private float acceleration = .25f;
	private float dashTimer = .5f;
	private float dashTimerReset = .5f;
	private bool isDashReady = true;
	private Vector2 velocity = new Vector2();
	private bool isDashing = false;
	private bool isWallJumping = false;
	private float wallJumpTimer = .5f;
	private float wallJumpTimerReset = .5f;
	

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		
	}

//  // Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(float delta)
	{
		if (!isDashing && !isWallJumping)
		{
			int direction = 0;

			if (Input.IsActionPressed("ui_left")) direction -= 1;

			if (Input.IsActionPressed("ui_right")) direction += 1;

			if (direction != 0) velocity.x = Mathf.Lerp(velocity.x, direction * speed, acceleration);
			else velocity.x = Mathf.Lerp(velocity.x, 0, friction);

		}
		
		if (IsOnFloor()){
			if (Input.IsActionJustPressed("jump")) velocity.y = -jumpHeight;
			isDashReady = true;
		}

		if (Input.IsActionJustPressed("jump") && GetNode<RayCast2D>("RayCastLeft").IsColliding())
		{
			velocity.y = -jumpHeight;
			velocity.x = jumpHeight;
			isWallJumping = true;
		}
		else if (Input.IsActionJustPressed("jump") && GetNode<RayCast2D>("RayCastRight").IsColliding())
		{
			velocity.y = -jumpHeight;
			velocity.x = -jumpHeight;
			isWallJumping = true;
		}

		if (isWallJumping)
		{
			wallJumpTimer -= delta;
			if (wallJumpTimer <= 0) 
			{
				isWallJumping = false;
				wallJumpTimer = wallJumpTimerReset;
			}
		}

		if (isDashReady)
		{
			if (Input.IsActionJustPressed("dash"))
			{
				if (Input.IsActionPressed("ui_left")) 
				{
					velocity.x = -dashSpeed;
					isDashing = true;
				}
				if (Input.IsActionPressed("ui_right")) 
				{
					velocity.x = dashSpeed;
					isDashing = true;
				}
				if (Input.IsActionPressed("ui_up")) 
				{
					velocity.y = -dashSpeed;
					isDashing = true;
				}
				if (Input.IsActionPressed("ui_right") && Input.IsActionPressed("ui_up")) 
				{
					velocity.x = dashSpeed;
					velocity.y = -dashSpeed;
					isDashing = true;
				}
				if (Input.IsActionPressed("ui_left") && Input.IsActionPressed("ui_up")) 
				{
					velocity.x = -dashSpeed;
					velocity.y = -dashSpeed;
					isDashing = true;
				}

				dashTimer = dashTimerReset;
				isDashReady = false;
			}
		}

		if (isDashing)
		{
			dashTimer -= delta;
			if (dashTimer <= 0) 
			{
				isDashing = false;
				velocity = new Vector2(0, 0);
			}
		}
		else velocity.y += gravity * delta;

		MoveAndSlide(velocity, Vector2.Up);
	}
}
