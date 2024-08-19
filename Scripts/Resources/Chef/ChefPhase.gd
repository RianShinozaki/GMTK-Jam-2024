class_name ChefPhase extends Resource

@export var phaseName : String
@export var phaseDescription : String

@export_enum("Base:0", "Melody:1", "Percussion:2") var trackType : int
@export_enum("Intro","LoopA","LoopB","Chorus","Workshop","LoopB2") var trackPart : int
@export var trackVolume : float
@export var phaseLength : float

@export var taskList : Array[ChefTask]
