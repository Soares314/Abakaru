using Godot;
using System;

[GlobalClass]
public partial class Comportamento : Resource
{
	public Personagem personagem;
	public bool Ativo = true;

	public virtual void InicializarComportamento(Personagem personagemControlado)
	{
		personagem = personagemControlado;
		personagem.ChamouAtencao += OnChamouAtencao;
	}
	// Controlador de comportamentos de um NPC, chamando suas ações relativas
	public virtual void AtiveComportamento()
	{
		// GD.Print("Esse é o comportamento padrão");
		return;
	}
	public virtual void OnChamouAtencao()
	{
		Ativo = false;
		personagem.Reparent(personagem.GetTree().CurrentScene);
	}
}