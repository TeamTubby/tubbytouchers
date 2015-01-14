var isQuit=false;
var isBack=false;
var isHelp=false;

function OnMouseEnter(){
	//change text color
	renderer.material.color=Color.red;
}

function OnMouseExit(){
	//change text color
	renderer.material.color=Color.white;
}

function OnMouseUp(){
	//is this quit
	if (isQuit==true) {
		//quit the game
		Application.Quit();
	}
	else if (isBack==true) {
		//quit the game
		Application.LoadLevel(0);
	}
	else if (isHelp==true) {
		//quit the game
		Application.LoadLevel(2);
	}
	else {
		//load level
		Application.LoadLevel(1);
	}
}

function Update(){
	//quit game if escape key is pressed
	if (Input.GetKey(KeyCode.Escape)) { Application.Quit();
	}
}