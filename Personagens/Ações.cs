using Godot;
using System.Linq;

public partial class Ações : Personagem
{
	// Ação de andar até em direção a um ponto
	public void AndarAte(float delta, Vector2 posicaoAlvo)
	{
		Vector2 velocidade = Vector2.Zero;

		if (posicaoAlvo != Vector2.Zero)
		{
			AlcancePersonagem.TargetPosition = posicaoAlvo - Position;
			velocidade = AlcancePersonagem.TargetPosition.Normalized() * Status["Velocidade"];
		}
		else
        {
            velocidade.X = Mathf.MoveToward(velocidade.X * 0.3f, 0, Status["Velocidade"] * delta);
            velocidade.Y = Mathf.MoveToward(velocidade.Y * 0.3f, 0, Status["Velocidade"] * delta);
        }

		Velocity = velocidade;
		MoveAndSlide();

	}

	// Verifica se algo está ao alcance do personagem
	public bool EstarAoAlcance(float alcance)
	{
		AlcancePersonagem.TargetPosition *= alcance;
		if (AlcancePersonagem.IsColliding())
		{
			AlcancePersonagem.TargetPosition = AlcancePersonagem.TargetPosition.Normalized();
			return true;
		}
		else
		{
			AlcancePersonagem.TargetPosition = AlcancePersonagem.TargetPosition.Normalized();
			return false;
		}
	}


	// Cálculo para qual membro será ferido e tranferência do ataque
	public void Atacar(Godot.Collections.Dictionary<string, float> precisaoAtaque, Personagem personagemAlvo, int dano)
	{
		float TotalProp = 0.0f;

		foreach (float item in precisaoAtaque.Values)
		{
			TotalProp += item;
		}

		if(TotalProp == 1f)
		{
			float numeroAleatorio = GD.Randf();

			foreach (string membro in precisaoAtaque.Keys)
			{
				if(numeroAleatorio < precisaoAtaque[membro])
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
	public void AtaqueCurto(float alcanceAtaque, Personagem personagemAlvo, int dano)
	{
		if (EstarAoAlcance(alcanceAtaque))
		{
			AnimePersonagem("Ataque Curto");
			return ;
		}
	}

}
