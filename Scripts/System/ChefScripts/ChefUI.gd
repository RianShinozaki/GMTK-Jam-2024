class_name ChefUI extends Control

@onready var phaseContainer : Node = $NormalLabel
@onready var phaseLabel : Label = $NormalLabel/PhaseLabel
@onready var phaseDescriptionLabel : Label = $NormalLabel/PhaseDescriptionLabel

@onready var bigPhaseContainer : Node = $BigLabel
@onready var bigPhaseLabel : Label = $BigLabel/PhaseLabel
@onready var bigPhaseDescriptionLabel : Label = $BigLabel/PhaseDescriptionLabel

# Called when the node enters the scene tree for the first time.
func _ready():
	pass # Replace with function body.


# Called every frame. 'delta' is the elapsed time since the previous frame.
func _process(delta):
	pass

func _new_phase(phaseName : String, phaseDescription : String):
	phaseLabel.text = phaseName
	phaseDescriptionLabel.text = phaseDescription
	bigPhaseLabel.text = phaseName
	bigPhaseDescriptionLabel.text = phaseDescription
	
	var tween = create_tween().set_ease(Tween.EASE_OUT).set_trans(Tween.TRANS_QUAD)
	tween.tween_property(phaseContainer, "modulate:a", 0, .5)
	tween.set_parallel()
	tween.tween_property(bigPhaseContainer, "modulate:a", 1, .5)
	await get_tree().create_timer(1.5).timeout
	tween = create_tween().set_ease(Tween.EASE_OUT).set_trans(Tween.TRANS_QUAD)
	tween.tween_property(phaseContainer, "modulate:a", 1, .5)
	tween.set_parallel()
	tween.tween_property(bigPhaseContainer, "modulate:a", 0, .5)

	
	
