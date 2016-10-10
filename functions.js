var team = [
	{
		name: "Rickard Bergeling",
		photo: "img/members/Rickard.png",
		github: "herrr"
	}, 
	{
		name: "Erik Forsberg",
		photo: "img/members/Erik.jpg",
		github: "ankhuve"
	}, 
	{
		name: "Arvid Sätterkvist", 
		photo: "img/members/Arvid.png",
		github: "arvidsat"
	}, 
	{
		name: "Calle Sténson", 
		photo: "img/members/Calle.jpg",
		github: "cstensonpv"
	}, 
	{
		name: "Anton Sivertsson", 
		photo: "img/members/Anton.jpg",
		github: "herrr"
	}, 
	{
		name: "Emil Westin",
		photo: "img/members/Emil.jpg",
		github: "emiwes"
	}
];

var generateTeamContent = function(){
	var innerHTML = "";
	for(var member in team){
		innerHTML += '<div class="member">';
		innerHTML += '<a href="https://github.com/'+team[member].github+'">';
		innerHTML += '<img src="'+team[member].photo+'"/>';
		innerHTML += '<div class="fname">'+team[member].name.split(" ")[0]+'</div>';
		innerHTML += '<div class="lname">'+team[member].name.split(" ")[1]+'</div>';
		innerHTML += "</a>";
		innerHTML += '</div>';
	}
	document.getElementById("theTeam").innerHTML = innerHTML;
}

generateTeamContent();