function mouse_click(obj) {
	if (obj.classList.contains("itemblue"))
	{
		obj.classList.remove("itemblue");
		obj.classList.add("itemred");
	}
	else if (obj.classList.contains("itemred"))
	{
		obj.classList.remove("itemred");
		obj.classList.add("itemblue");
	}
  
  obj.innerHTML = "X";
  document.getElementById("debug_output").innerHTML = "mouse_click " + obj.id;
}