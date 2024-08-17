extends Node2D

@export var testState : Chef.ChefStates

# Called when the node enters the scene tree for the first time.
func _ready():
	print(get_node("."))
	match testState:
		Chef.ChefStates.GetPot:
			
			ChefManager.potLocation = get_node(".")
		Chef.ChefStates.GetWater:
			ChefManager.waterLocation = get_node(".")
		Chef.ChefStates.GetVegetables:
			ChefManager.vegetableLocation = get_node(".")
		Chef.ChefStates.GetMeat:
			ChefManager.meatLocation = get_node(".")
		Chef.ChefStates.Cook:
			ChefManager.cookLocation = get_node(".")
	
	pass # Replace with function body.


# Called every frame. 'delta' is the elapsed time since the previous frame.
func _process(delta):
	pass
