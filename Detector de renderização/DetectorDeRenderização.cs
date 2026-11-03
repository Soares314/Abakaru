using Godot;
using System;

public partial class DetectorDeRenderização : VisibleOnScreenEnabler2D
{
	// Captura o nó da sala
	Node2D ObjetoRenderizar;

	// Renderizar o objeto apenas ele estiver na tela
	public void OnScreenEntered()
	{
		ObjetoRenderizar.Visible = true;
	}

	public void OnScreenExited()
	{
		ObjetoRenderizar.Visible = false;
	}
}
