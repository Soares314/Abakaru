using System.Threading.Tasks;
using Godot;
using Godot.Collections;

public partial class Personagem : CharacterBody2D
{
	public RayCast2D RaioCalculo;

	private CaminhoAutomatizado RotaPersonagem;

	[Export]
	public Dictionary<string, int> Status = new Dictionary<string, int>()
	{
		{ "DanoNormal", 2},
		{ "Velocidade", 300},
		{ "AlcanceAtaque", 100 },
		{ "Cabeça", 10 },
		{ "Pernas", 10 },
		{ "Braços", 10 },
		{ "Peitoral", 10}
	};

	[Export]
	public Dictionary<string, float> Precisao = new Dictionary<string, float>()
	{
		{ "Cabeça", 0.15f },
		{ "Pernas", 0.2f },
		{ "Braços", 0.3f },
		{ "Peitoral", 0.35f }
	};

	[Export]
	public Array<Comportamento> SerieComportamentos;
	[Signal]
	public delegate void ChamouAtencaoEventHandler();
	private bool VisaoColidiuComTile = false;

    public override void _Ready()
    {
        RaioCalculo = GetNodeOrNull<RayCast2D>("Alcance Personagem");

		if(GetParent() is CaminhoAutomatizado)
		{
			RotaPersonagem = GetParent<CaminhoAutomatizado>();
		}

		foreach (Comportamento comportamento in SerieComportamentos)
		{
			comportamento.InicializarComportamento(this);
		}
    }

    public override void _PhysicsProcess(double delta)
    {

        foreach (Comportamento comportamento in SerieComportamentos)
		{
			comportamento.AtiveComportamento();
		}
    }

	public void AnimePersonagem(string tipoAnimacao)
	{
		
	}

	/*
	
	Verificações
	
	*/

	// Verifica se o ataque está ao alcance do personagem
	public bool EstarAoAlcance()
	{
		RaioCalculo.TargetPosition *= Status["AlcanceAtaque"];

		if (RaioCalculo.IsColliding())
		{
			RaioCalculo.TargetPosition = RaioCalculo.TargetPosition.Normalized();
			return true;
		}
		else
		{
			RaioCalculo.TargetPosition = RaioCalculo.TargetPosition.Normalized();
			return false;
		}
	}

	public async Task EstabelecerRota(CaminhoAutomatizado rota)
	{
		// Jogador se move até o ponto do caminho
		Vector2 local = rota.Curve.GetPointPosition(0);  
		Vector2 posicaoPonto = rota.ToGlobal(local);

		await AndarAte(posicaoPonto);

		Reparent(rota.RotaPersonagem);

		GetTree().CurrentScene.AddChild(rota);
	}

	// Verifica o perimetro ao redor da sala que estar para instanciar a curva de andar circulos
	public CaminhoAutomatizado CacularPerimetro()
	{
		Curve2D curvaCircular = GD.Load<Curve2D>("res://ComportamentosIA/Formas de Andar/Circulo.tres");

		var cenaCaminho = GD.Load<PackedScene>("res://Personagens/Caminho Automatizado/caminho_automatizado.tscn");
		CaminhoAutomatizado caminhoAuto = cenaCaminho.Instantiate<CaminhoAutomatizado>();

		caminhoAuto.Curve = curvaCircular;
		caminhoAuto.DetectarColisao = true;
		caminhoAuto.GlobalPosition = GlobalPosition;

		AddChild(caminhoAuto);

		caminhoAuto.ColidiuComTile += OnCaminhoAutomatizadoColidiuComTile;
		while (VisaoColidiuComTile == false)
		{
			caminhoAuto.Scale = new Vector2(Scale.X + 0.2f, Scale.Y + 0.2f);
		}

		caminhoAuto.Scale = new Vector2(Scale.X - 0.2f, Scale.Y - 0.2f);

		caminhoAuto.ColidiuComTile -= OnCaminhoAutomatizadoColidiuComTile;

		RemoveChild(caminhoAuto);

		return caminhoAuto;
	}

	private void OnCaminhoAutomatizadoColidiuComTile()
	{
		VisaoColidiuComTile = true;
	}

	/*
	
	Ações
	
	*/


	// Ação de tentar andar até em direção a um ponto
	public async Task AndarAte(Vector2 posicaoAlvo)
	{
		Vector2 velocidade = Vector2.Zero;

		float delta = (float)GetPhysicsProcessDeltaTime();

		if (posicaoAlvo != Vector2.Zero)
		{
			RaioCalculo.TargetPosition = posicaoAlvo - Position;
			velocidade = RaioCalculo.TargetPosition.Normalized() * Status["Velocidade"];
		}
		else
        {
            velocidade.X = Mathf.MoveToward(velocidade.X * 0.3f, 0, Status["Velocidade"] * delta);
            velocidade.Y = Mathf.MoveToward(velocidade.Y * 0.3f, 0, Status["Velocidade"] * delta);
        }

		Velocity = velocidade;
		MoveAndSlide();

	}

	// Tomar dano
	public void TomarDano(int dano, string membro)
	{
		Status[membro] -= dano;
	}

	// Cálculo para qual membro será ferido e tranferência do ataque
	public void Atacar(Personagem personagemAlvo, int dano)
	{
		float TotalProp = 0.0f;

		foreach (float item in Precisao.Values)
		{
			TotalProp += item;
		}

		if(TotalProp == 1f)
		{
			float numeroAleatorio = GD.Randf();

			foreach (string membro in Precisao.Keys)
			{
				if(numeroAleatorio < Precisao[membro])
				{
					personagemAlvo.TomarDano(dano, membro);
				}
			}
		}
		else
		{
			GD.PushError($"Formato de descrição da precissão de {this.Name} inválidos");
			return;
		}

	}

	// Ataque fisico básico
	public void PrepararAtaqueCurto(Personagem personagemAlvo, int dano)
	{
		if (EstarAoAlcance())
		{
			Atacar(personagemAlvo, dano);
			AnimePersonagem("Ataque Curto");
			return ;
		}
	}

}
