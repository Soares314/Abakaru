using System.Threading.Tasks;
using Godot;
using Godot.Collections;

public partial class Personagem : CharacterBody2D
{
	private enum Estado
	{
		EM_ACAO,
		PARADO,
	}
	public Vector2 PosicaoAnterior;
	public RayCast2D RaioDirecao;

	private Area2D AreaVisao;

	[Export]
	public Dictionary<string, int> Status = new Dictionary<string, int>()
	{
		{ "Força", 2},
		{ "Velocidade", 300},
		{ "Cabeça", 10 },
		{ "Pernas", 10 },
		{ "Braços", 10 },
		{ "Peitoral", 10}
	};

	public Dictionary<string, int> StatusAtual;

	public Dictionary<string, int> Amplificadores = new Dictionary<string, int>()
	{
		{ "Força", 1 },
		{ "Velocidade", 1 },
		{ "Cabeça", 1 },
		{ "Pernas", 1 },
		{ "Braços", 1 },
		{ "Peitoral", 1 }
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
	public Array<Arma> Armas = [new Arma(), new Arma()];
	[Export]
	public Dictionary<string, int> Municao;
	Estado EstadoPersonagem = Estado.EM_ACAO;

	[Export]
	public Array<Comportamento> SerieComportamentos = [new MorteHumana()];
	[Export]	
	public Aspecto AspectoPersonagem = new Aspecto();
	[Export]
	public bool Especial = false;
	[Signal]
	public delegate void ChamouAtencaoEventHandler();
	private bool VisaoColidiuComTile = false;

	public float ErroCritico = 0.005f;

    public override void _Ready()
    {
        RaioDirecao = GetNodeOrNull<RayCast2D>("Alcance Personagem");

		AreaVisao = GetNodeOrNull<Area2D>("Visão");
 
		StatusAtual = new Dictionary<string, int>()
		{
			{ "Força", 2},
			{ "Velocidade", 300},
			{ "Cabeça", 10 },
			{ "Pernas", 10 },
			{ "Braços", 10 },
			{ "Peitoral", 10}
		};

		StatusAtual = Status.Duplicate();

		foreach (Comportamento comportamento in SerieComportamentos)
		{
			comportamento.InicializarComportamento(this);
		}
		GD.Print("Chaves: ", string.Join(", ", Status.Keys));

    }

    public override void _PhysicsProcess(double delta)
    {
        foreach (Comportamento comportamento in SerieComportamentos)
		{
			comportamento.AtiveComportamento();
		}

		// Atualiza RaioDirecao para indicar a direção que está e e sua área de visão
		RaioDirecao.TargetPosition = (GlobalPosition - PosicaoAnterior).Normalized();

		/*if(!(this is Jogador))
		{
			GD.Print("Alcance antes antes:", RaioDirecao.TargetPosition);
		}*/

		if (RaioDirecao.TargetPosition.LengthSquared() > 0.01f)
		{
			AreaVisao.Rotation = RaioDirecao.TargetPosition.Angle() - Mathf.Pi / 2f;
		}

		PosicaoAnterior = GlobalPosition;

    }

	public void AnimePersonagem(string tipoAnimacao)
	{
		
	}

	/*
	
	Verificações
	
	*/

	// Para personagem olhar e retornar os personagens que está olhando
	public bool Olhou(Node objeto)
	{
		// GD.Print("Eles está olhando? ", AreaVisao.OverlapsBody(objeto));
		return AreaVisao.OverlapsBody(objeto);
	}

	// Verifica se o ataque está ao alcance do personagem
	public bool EstarAoAlcance(int alcance, Node2D alvo = null)
	{
		RaioDirecao.TargetPosition *= alcance;
		RaioDirecao.ForceRaycastUpdate();

		if (RaioDirecao.IsColliding())
		{
			if(RaioDirecao.GetCollider() == alvo)
			{
				RaioDirecao.TargetPosition = RaioDirecao.TargetPosition.Normalized();
				return true;
			}
		}

		RaioDirecao.TargetPosition = RaioDirecao.TargetPosition.Normalized();
		return false;
	}

	// Começa a seguir uma rota indicada
	public async Task EstabelecerRota(CaminhoAutomatizado rota)
	{
		// Jogador se move até o ponto do caminho
		/*Vector2 local = rota.Curve.GetPointPosition(0);  
		Vector2 posicaoPonto = rota.ToGlobal(local);

		await AndarAte(posicaoPonto);
		*/
		rota.ObjetoMovido = this;
		rota.Position = Position;

		GetTree().CurrentScene.AddChild(rota);

		//Scale = new Vector2(Scale.X/rota.Scale.X, Scale.Y/rota.Scale.Y);

		Reparent(rota.RotaPersonagem);
	}

	public CaminhoAutomatizado CalcularReto(Vector2 direcao, float tamanho = 300)
	{

		Curve2D curvaReta = GD.Load<Curve2D>("res://ComportamentosIA/Formas de Andar/Reto.tres");

		var cenaCaminho = GD.Load<PackedScene>("res://Personagens/Caminho Automatizado/caminho_automatizado.tscn");
		CaminhoAutomatizado caminhoAuto = cenaCaminho.Instantiate<CaminhoAutomatizado>();

		caminhoAuto.Curve = curvaReta;
		caminhoAuto.Scale *= tamanho;
		caminhoAuto.Rotation = direcao.Angle();


		return caminhoAuto;
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

	// Ação para parar, o personagem não pode ser movido
	public void Parar()
	{
		EstadoPersonagem = Estado.PARADO;
	}

	// Ação de tentar andar até em direção a um ponto
	public async Task AndarAte(Vector2 posicaoAlvo)
	{
		if(EstadoPersonagem != Estado.PARADO)
		{
			Vector2 velocidade = Vector2.Zero;

			float delta = (float)GetPhysicsProcessDeltaTime();

			if (posicaoAlvo != Vector2.Zero)
			{
				RaioDirecao.TargetPosition = (posicaoAlvo - Position).Normalized();
				// GD.Print("Posição que está indo: ", RaioDirecao.TargetPosition);
				velocidade = RaioDirecao.TargetPosition * StatusAtual["Velocidade"];
			}
			else
			{
				velocidade.X = Mathf.MoveToward(velocidade.X * 0.3f, 0, StatusAtual["Velocidade"] * delta);
				velocidade.Y = Mathf.MoveToward(velocidade.Y * 0.3f, 0, StatusAtual["Velocidade"] * delta);
			}

			Velocity = velocidade;
			MoveAndSlide();
		}

	}

	// Tomar dano ou diminuir status
	public async void TomarDano(int dano, string membro)
	{
		StatusAtual[membro] -= dano;
		GD.Print("Membro:", membro);
		GD.Print("Vida:", StatusAtual[membro]);

		if(StatusAtual[membro] < 0 && Amplificadores[membro] > 1)
		{
			Amplificadores[membro]--;
			StatusAtual[membro] = Status[membro];
		}
	}

	public virtual void Morrer()
	{
		if((GD.Randi() % 100) < 5 || Especial)
		{
			Node sala = GetTree().CurrentScene;
			var cenaMorto = GD.Load<PackedScene>("res://Item/item.tscn");

			Item areaMorto = cenaMorto.Instantiate<Item>();

			areaMorto.Position = Position;
			areaMorto.ItemNoChao = AspectoPersonagem;

			sala.AddChild(areaMorto);
		}

		QueueFree();
	}

	// Cálculo para qual membro será ferido e tranferência do ataque
	public void Atacar(Personagem personagemAlvo, int dano)
	{
		float TotalProp = 0.0f;

		foreach (float item in Precisao.Values)
		{
			TotalProp += item;
		}
		//GD.Print("Probalidade Total:", TotalProp);

		if(TotalProp == 1f)
		{
			float numeroAleatorio = GD.Randf();
			float acumulativo = 0.0f;

			foreach (string membro in Precisao.Keys)
			{
				if(numeroAleatorio < Precisao[membro])
				{
					personagemAlvo.TomarDano(dano, membro);
					break;
				}
				acumulativo += Precisao[membro]; 
			}
		}
		else
		{
			GD.Print($"Formato de descrição da precissão de {Name} inválidos");
			return;
		}

	}

	// Ataque fisico básico
	public void PrepararAtaqueCurto(Node2D personagemAlvo, int dano, int alcance, string animacao = "Soco")
	{
		if (personagemAlvo == null || EstarAoAlcance(alcance, personagemAlvo))
		{
			//GD.Print("Estar no alcance");
			if(personagemAlvo is Personagem)
			{
				if(GD.Randf() < ErroCritico)
				{
					Atacar(this, dano);
				}
				else
				{
					Atacar((Personagem)personagemAlvo, dano);
				}
				
				AnimePersonagem(animacao);
			}
		}
	}

}
