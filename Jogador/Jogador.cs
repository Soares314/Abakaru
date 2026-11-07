using Godot;
using Godot.Collections;

public partial class Jogador : Personagem
{
	[Signal]
	public delegate void PlayerInteractedEventHandler(GodotObject interactedObject);
	private GodotObject CollidingObject;
	public Resource NaZonaDe = null;

	public Array<Aspecto> AspectosJogador;

	public void GetInput(float delta)
	{
		// Detectar inputs.
		Vector2 direcao = Input.GetVector("ui_left", "ui_right", "ui_up", "ui_down");
		MoverAte(delta, direcao);

		if(Input.IsActionJustPressed("UsarEsquerda"))
			Armas[0].Usar(this, null);

		if(Input.IsActionJustPressed("UsarDireita"))
			Armas[1].Usar(this, null);

		if (Input.IsActionJustPressed("Interagir"))
			ColetarItem();
	}

	// Called every frame during physics processing.
	public override void _PhysicsProcess(double delta)
	{
		
		GetInput((float)delta);

		if (Velocity != Vector2.Zero)
		{
			if (GetLastSlideCollision() != null)
				CollidingObject = GetLastSlideCollision().GetCollider();
			else
				CollidingObject = null;
		}

		base._PhysicsProcess(delta);

		// GD.Print(CollidingObject);
	}

	//  Mover após apertar
	public void MoverAte(float delta, Vector2 direcao)
	{
		// Handles movement.
		Vector2 velocity = Velocity;

		if (direcao != Vector2.Zero)
		{
			velocity = direcao * Status["Velocidade"];
		}
		else
        {
            velocity.X = Mathf.MoveToward(velocity.X * 0.3f, 0, Status["Velocidade"] * delta);
            velocity.Y = Mathf.MoveToward(velocity.Y * 0.3f, 0, Status["Velocidade"] * delta);
        }

		Velocity = velocity;
		MoveAndSlide();
	}

	// Comer corpo ou pegar objeto.
	public void ColetarItem()
	{
		if(NaZonaDe != null)
		{
			// Comer
			if(NaZonaDe is Aspecto)
			{
				EmitSignal(SignalName.PlayerInteracted, CollidingObject);
				if (AspectosJogador.Contains(NaZonaDe as Aspecto))
				{
					AspectosJogador[AspectosJogador.IndexOf(NaZonaDe as Aspecto)].Amplificador += 1f;
				}
				else
				{
					AspectosJogador.Add(NaZonaDe as Aspecto);
				}
			}
			// Pegar objeto
			else if(NaZonaDe is Arma)
			{
				if(Armas.Count < 2)
				{
					Armas.Add(NaZonaDe as Arma);
				}
			}
		}
	}

	public override void Morrer()
	{
		base.Morrer();

		// Mostrar tela de morte
		var cenaMorte = GD.Load<PackedScene>("res://Telas/tela_de_morte.tscn");

		Node telaMorte = cenaMorte.Instantiate();

		GetTree().CurrentScene.AddChild(telaMorte);
	}
}