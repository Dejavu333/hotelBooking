let starCount = 0;
const starsDiv = document.querySelector('#starsDiv')
const stars = starsDiv.children;

for (var i = 0; i < stars.length; i++) {
  	stars[i].addEventListener('click', handleClick);
}

function handleClick(e){
	//ß if the target is the first star and it is already selected, then deselects all stars
	if ((Number(e.target.id) + 1) == starCount)
	{ 
		
		[...stars].forEach((item,i) => {
			item.classList.remove('selected');
		});
		starCount = 0;
		return;
	}
	
	starCount=0;
	[...stars].forEach((item,i) => {
		item.classList.remove('selected');	

		if(i <= e.target.id)
		{
			item.classList.add('selected');
			starCount++;
		}
	});
}
