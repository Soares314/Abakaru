using Godot;

[GlobalClass]
public partial class Aspecto : Resource
{
	[Export]
	public float Amplificador = 1f;
    public Jogador jogador;
	public HBoxContainer Cabecas;
	// Vai rodar sempre que o jogador devorar o aspecto
    public virtual void InicializarAspecto(Jogador jogadorQueComeu)
	{
		jogador = jogadorQueComeu;

		jogador.Amplificadores["Cabeça"] += 1;

		// HUD da cabeça aparecendo indicando a vida
		HBoxContainer Cabecas = jogadorQueComeu.HUDjogador.GetNode<HBoxContainer>("Cabecas");

        TextureRect cabecaNova = new TextureRect
        {
            Texture = GD.Load<Texture2D>("res://Sprites/HUDJogador/CabeçaHUD.png"),
            ExpandMode = TextureRect.ExpandModeEnum.FitWidthProportional
        };

        Cabecas.AddChild(cabecaNova);

	}

	// Controlador de comportamentos de um NPC, chamado todo frame
	public virtual void RodarAspecto()
	{
		GD.Print("Esse é o aspecto padrão com aplificação", Amplificador);
		return;
	}

	// Perde a vida caso vomite
    protected override void Dispose(bool disposing)
    {
		if (disposing)
		{
			jogador.Amplificadores["Cabeça"] += 1;
			Cabecas.GetChild(Cabecas.GetChildCount() - 1).QueueFree();
		}
    }
}
