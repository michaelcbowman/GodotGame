using Godot;
using System;

public class PlayerController : KinematicBody2D
{
	private int speed = 400;
	private int gravity = 9000;
	private float friction = .9f;
	private float acceleration = .25f;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		
	}

//  // Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(float delta)
	{
		Vector2 velocity = new Vector2();
		int direction = 0;

		if (Input.IsActionPressed("ui_left")) direction -= 1;

		if (Input.IsActionPressed("ui_right")) direction += 1;

		if (direction != 0) velocity.x = Mathf.Lerp(velocity.x, direction * speed, acceleration);
		else velocity.x = Mathf.Lerp(velocity.x, 0, friction);


		if(Input.IsActionJustPressed("jump")){
			if(IsOnFloor()){
				velocity.y -= gravity / 2;
			}
		}

		velocity.y += gravity * delta;

		MoveAndSlide(velocity, Vector2.Up);
	}
}
