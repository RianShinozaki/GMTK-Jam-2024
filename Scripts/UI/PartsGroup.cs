using Godot;
using System;

public partial class PartsGroup : HBoxContainer
{
	[Export] Godot.Collections.Array<AndroidPiece> pieces;
	PieceOption pieceTemplate;
	[Export] AndroidPart androidPart;
	
	public override void _Ready()
	{
		pieceTemplate = GetChild<PieceOption>(0);
		for(int i = 0; i < pieces.Count; i++) {
			PieceOption newPiece = (PieceOption)pieceTemplate.Duplicate();
			newPiece.myPiece = pieces[i];
			newPiece.myPart = androidPart;
			newPiece.Init();
			AddChild(newPiece);
		}
		pieceTemplate.QueueFree();
	}
	public override void _Process(double delta)
	{
	}
}
