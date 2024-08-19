class_name Ladder extends Node2D

@export var ladderLevel : int
@export var travelTime : float

@onready var bottomTrigger = $BottomTrigger
@onready var topTrigger = $TopTrigger

func _ready():
	ChefManager.ladderLocations[ladderLevel] = self

func _request_climb_ladder(chef : Chef, level : int):
	chef._climb_ladder(level,
	topTrigger.global_position if level == 0 else bottomTrigger.global_position,
	bottomTrigger.global_position if level == 0 else topTrigger.global_position, travelTime)
