using Godot;
using System;

public partial class Jogador : Personagem
{
	[Signal]
	public delegate void PlayerInteractedEventHandler(GodotObject interactedObject);
	private GodotObject CollidingObject;

	// Called every frame during physics processing.
	public override void _PhysicsProcess(double delta)
	{
		Vector2 velocity = Velocity;

		// Get inputs arguments.
		Vector2 direction = Input.GetVector("ui_left", "ui_right", "ui_up", "ui_down");

		if (Input.IsActionJustPressed("Interagir") && CollidingObject != null)
			EmitSignal(SignalName.PlayerInteracted, CollidingObject);
		        						
		// Handles movement.
		if (direction != Vector2.Zero)
		{
			velocity = direction * Status["Velocidade"];
		}
		else
        {
            velocity.X = Mathf.MoveToward(velocity.X * 0.3f, 0, Status["Velocidade"] * (float)delta);
            velocity.Y = Mathf.MoveToward(velocity.Y * 0.3f, 0, Status["Velocidade"] * (float)delta);
        }	

		Velocity = velocity;
		MoveAndSlide();

		if (Velocity != Vector2.Zero)
		{
			if (GetLastSlideCollision() != null)
				CollidingObject = GetLastSlideCollision().GetCollider();
			else
				CollidingObject = null;
		}

		// GD.Print(CollidingObject);
	}
}