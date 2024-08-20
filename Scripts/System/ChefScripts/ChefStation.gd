class_name ChefStation extends Node2D

#for anim
enum ChefStationType{
	Grab,
	Stir,
	Cook,
	Cut
}

@export var stationType : ChefStationType

@export var actions : Array[ChefStationActions]
@export var chefState : Chef.ChefStates
@export var ladderLevel : int

# Called when the node enters the scene tree for the first time.
func _ready():
	#print(get_node("."))
	match chefState:
		Chef.ChefStates.GetPot:
			ChefManager.potLocation = get_node(".")
		Chef.ChefStates.GetAlcohol:
			ChefManager.alcoholLocation = get_node(".")
		Chef.ChefStates.GetSeasoning:
			ChefManager.seasoningLocation = get_node(".")
		Chef.ChefStates.GetVegetables:
			ChefManager.vegetableLocation = get_node(".")
		Chef.ChefStates.GetMeat:
			ChefManager.meatLocation = get_node(".")
		Chef.ChefStates.Chop:
			ChefManager.chopLocation = get_node(".")
		Chef.ChefStates.Stove:
			ChefManager.stoveLocation = get_node(".")
		Chef.ChefStates.Serve:
			ChefManager.serveLocation = get_node(".")
	
	pass # Replace with function body.

func _chef_use_station(chef : Chef, taskName : String):
	for a in actions:
		if a.itemIn == taskName:
			chef._start_active_task(a.actionTime, stationType, a.itemOut)
