using System.ComponentModel.DataAnnotations;
using Godot;
using Godot.NativeInterop;
using System;

public partial class CaminhoAutomatizado : Path2D
{
	public enum EstadoPersonagem {FRENTE, TRAS}
	public PathFollow2D RotaPersonagem;
	public PhysicsBody2D ObjetoMovido;
	private Area2D AreaColisao;
	public bool DetectarColisao = false;
	[Signal]
    public delegate void ColidiuComTileEventHandler();
	private float VelocidadeObjeto = 0.001f;
	public EstadoPersonagem FrenteOuTras = EstadoPersonagem.FRENTE;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		RotaPersonagem = GetNode<PathFollow2D>("Rota Personagem");
		AreaColisao = GetNode<Area2D>("Detector Colisão");

		// Define a velocidade que move o objeto
		if(ObjetoMovido is Personagem)
		{
			Personagem objetoMovidoAsPersonagem = ObjetoMovido as Personagem;
			VelocidadeObjeto = objetoMovidoAsPersonagem.Status["Velocidade"] * 0.00001f;
		}

		if (DetectarColisao)
		{
			AtivaDetectorColisao();
		}
	}

	public override void _PhysicsProcess(double delta)
	{
	
		// Controla o fluxo do caminho que o NPC vai andar, sempre fazendo ir e voltar
		switch (FrenteOuTras)
		{
		case EstadoPersonagem.TRAS:
			RotaPersonagem.ProgressRatio -= VelocidadeObjeto;
			break;

		case EstadoPersonagem.FRENTE:
			RotaPersonagem.ProgressRatio += VelocidadeObjeto;
			break;
		}

		if(RotaPersonagem.ProgressRatio == 0)
		{
			FrenteOuTras = EstadoPersonagem.FRENTE;
		}
		else if(RotaPersonagem.ProgressRatio == 1)
		{
			FrenteOuTras = EstadoPersonagem.TRAS;
		}

		/*
		if (HaColisao())
		{
			GD.Print("Inverteu");
			InverterOrdem();
		}
		*/

	}

	// Detecta se há colisão com algum objeto no caminho
	private bool HaColisao()
	{
		if(ObjetoMovido == null)
		{
			GD.Print("Não há objeto anexado");
			return false;
		}
		
		Vector2 direcao = RotaPersonagem.GlobalTransform.X; 
		Vector2 proxDirecao = direcao + new Vector2(VelocidadeObjeto, VelocidadeObjeto);

		return ObjetoMovido.TestMove(ObjetoMovido.GlobalTransform, proxDirecao);
	}

	public void InverterOrdem()
	{
		if(FrenteOuTras == EstadoPersonagem.FRENTE)
		{
			FrenteOuTras = EstadoPersonagem.TRAS;
		} 
		else
		{
			FrenteOuTras = EstadoPersonagem.FRENTE;
		}
	}

	// Utilizado para detectar
	public void AtivaDetectorColisao()
	{
		CollisionPolygon2D areaColisao = new CollisionPolygon2D();


		Vector2[] pontosCurva = Curve.GetBakedPoints();

		Array.Copy(pontosCurva, areaColisao.Polygon, pontosCurva.Length);

    	AreaColisao.AddChild(areaColisao);
	}

	private void OnDetectorColisãoBodyEntered(Node2D colisor)
	{
		if(colisor is TileMapLayer)
		{
			EmitSignal(SignalName.ColidiuComTile);
		}
	}
}
