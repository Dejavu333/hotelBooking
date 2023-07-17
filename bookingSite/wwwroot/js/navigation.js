//!Globals
function $(selector) 
{
  return document.querySelector(selector);
};

let middleLayerLevel = 1; //menuLayer, hotelsLayer, roomsLayer, favoritesLayer, myBookingsLayer


//!Functions
//?horizontal navigation via swipes
function next()   //leftmost, prev, middle, prev, rightmost
{
  let  leftmost, prev, middle, next, rightmost;
  if($(".middleLayer>.leftmost")) { leftmost = $(".middleLayer>.leftmost");}
  if($(".middleLayer>.prev")) { prev = $(".middleLayer>.prev");}
  if($(".middleLayer>.middle")) { middle = $(".middleLayer>.middle");}
  if($(".middleLayer>.next")) { next = $(".middleLayer>.next");}
  if($(".middleLayer>.rightmost")) { rightmost = $(".middleLayer>.rightmost");}

  //*unable to slide if there is no next card
  if(!next) return;

  //*removes leftmost from DOM
  if(leftmost)
  {
    leftmost.remove();
  }
  //*prev to leftmost
  if(prev)
  {
    $(".middleLayer>.prev").classList.add("leftmost");
    $(".middleLayer>.prev").classList.remove("prev");
  }
  //*middle to prev
  if(middle)
  {
    $(".middleLayer>.middle").classList.add("prev");
    $(".middleLayer>.middle").classList.remove("middle");
  }
  //*next to middle
  if(next)
  {
    $(".middleLayer>.next").classList.add("middle");
    $(".middleLayer>.next").classList.remove("next");
  }
  //*rightmost to next
  if(rightmost)
  {
    $(".middleLayer>.rightmost").classList.add("next");
    $(".middleLayer>.rightmost").classList.remove("rightmost");
  }
  //*leftmost to rightmost
  if(leftmost)
  {
    $(".middleLayer").appendChild(leftmost);
    leftmost.classList.remove("leftmost");
    leftmost.classList.add("rightmost");
  }
}//next function ends

function prev() 
{
  let  leftmost, prev, middle, next, rightmost;
  if($(".middleLayer>.leftmost")) { leftmost = $(".middleLayer>.leftmost");}
  if($(".middleLayer>.prev")) { prev = $(".middleLayer>.prev");}
  if($(".middleLayer>.middle")) { middle = $(".middleLayer>.middle");}
  if($(".middleLayer>.next")) { next = $(".middleLayer>.next");}
  if($(".middleLayer>.rightmost")) { rightmost = $(".middleLayer>.rightmost");}

  //*unable to slide if there is no prev card
  if(!prev) return;

  //*removes rightmost from DOM
  if(rightmost)
  {
    rightmost.remove();
  }
  //*next to rigtmost
  if(next)
  {
    $(".middleLayer>.next").classList.add("rightmost");
    $(".middleLayer>.next").classList.remove("next"); 
  }
  //*middle to next
  if(middle)
  {
    $(".middleLayer>.middle").classList.add("next");
    $(".middleLayer>.middle").classList.remove("middle");
  }
  //*prev to middle
  if(prev) 
  {
    $(".middleLayer>.prev").classList.add("middle");
    $(".middleLayer>.prev").classList.remove("prev");
  }
  //*leftmost to prev
  if(leftmost)
  {
    $(".middleLayer>.leftmost").classList.add("prev");
    $(".middleLayer>.leftmost").classList.remove("leftmost");
  }
  //*rightmost to leftmost
  if(rightmost)
  {
    $(".middleLayer").insertBefore(rightmost, $(".middleLayer").firstChild);
    rightmost.classList.remove("rightmost");
    rightmost.classList.add("leftmost");
  }
}//prev function ends

//?vertical navigation via swipes
function goOneLayerDown()  //middleLayer, aboveLayer, topLayer, belowLayer, bottomLayer
{
  let middleLayer, aboveLayer, topLayer, belowLayer, bottomLayer;
  if($(".middleLayer")) {middleLayer = $(".middleLayer");}
  if($(".belowLayer")) {belowLayer = $(".belowLayer");}
  if($(".bottomLayer")) {bottomLayer = $(".bottomLayer");}
  if($(".aboveLayer")) {aboveLayer = $(".aboveLayer");}
  if($(".topLayer")) {topLayer = $(".topLayer");}

  //*aboveLayer to topLayer
  if (aboveLayer) 
  {
    aboveLayer.classList.remove("aboveLayer");
    aboveLayer.classList.add("topLayer");
  }
  //*middleLayer to aboveLayer
  if (middleLayer)
  {
    middleLayer.classList.remove("middleLayer");
    middleLayer.classList.add("aboveLayer");
  }
  //* belowLayer to middleLayer
  if (belowLayer)
  {
    belowLayer.classList.remove("belowLayer");
    belowLayer.classList.add("middleLayer");
  }
  //*bottomLayer to belowLayer
  if (bottomLayer)
  {

    bottomLayer.classList.remove("bottomLayer");
    bottomLayer.classList.add("belowLayer");
  }
  middleLayerLevel++;
  
}//goLayerBelow function ends

function goOneLayerUp() 
{
  let middleLayer, aboveLayer, topLayer, belowLayer, bottomLayer;
  if($(".middleLayer")) { middleLayer = $(".middleLayer");}
  if($(".belowLayer")) { belowLayer = $(".belowLayer");}
  if($(".bottomLayer")) { bottomLayer = $(".bottomLayer");}
  if($(".aboveLayer")) { aboveLayer = $(".aboveLayer");}
  if($(".topLayer")) { topLayer = $(".topLayer");}

  //*topLayer to aboveLayer
  if (topLayer)
  {
    topLayer.classList.remove("topLayer");
    topLayer.classList.add("aboveLayer");
  }
  //*aboveLayer to middleLayer
  if (aboveLayer)
  {
    aboveLayer.classList.remove("aboveLayer");
    aboveLayer.classList.add("middleLayer");
  }
  //*middleLayer to belowLayer
  if (middleLayer)
  {
    middleLayer.classList.remove("middleLayer");
    middleLayer.classList.add("belowLayer");
  }
  //*belowLayer to bottomLayer
  if (belowLayer)
  {
    belowLayer.classList.remove("belowLayer");
    belowLayer.classList.add("bottomLayer");
  }
  middleLayerLevel--;
  
}//goOneLayerUp function ends

//? horizontal,vertical navigation via clicks
function slideViaClick(targetElement) 
{
  //*Locals
  const calledOnMiddleLayer = targetElement.parentNode.classList.contains("middleLayer");
  const calledOnAboveLayer = targetElement.parentNode.classList.contains("aboveLayer");
  const calledOnBelowLayer = targetElement.parentNode.classList.contains("belowLayer");

  const calledOnMiddleCard = targetElement.classList.contains("middle");
  const calledOnNextCard = targetElement.classList.contains("next");
  const calledOnPrevCard = targetElement.classList.contains("prev");
  
  //*Do nothing
  if(calledOnMiddleLayer && calledOnMiddleCard) return;
  if(!calledOnMiddleLayer && !calledOnMiddleCard) return;
  //*Switch Cards
  if(calledOnMiddleLayer && !calledOnMiddleCard)
  {
    if      (calledOnNextCard) next();
    else if (calledOnPrevCard) prev();
    return;
  }
  //*Switch layers
  if(!calledOnMiddleLayer && calledOnMiddleCard)
  {
    if      (calledOnAboveLayer) goOneLayerUp();
    else if (calledOnBelowLayer) goOneLayerDown();
    return;
  }
}//slideViaClick function ends


// //!Main
// const list = $(".list");

const swipeDiv = new Hammer($("body"));
swipeDiv.get('swipe').set({
  direction: Hammer.DIRECTION_ALL
});

//?horizontal navigation
swipeDiv.on("swipeleft", (ev) => {
    next();
});

swipeDiv.on("swiperight", (ev) => {
  prev();
});

//?vertical navigation
swipeDiv.on("swipeup", (ev) => {
  middleLayerLevel<3? goOneLayerDown() : console.log("no more layers below");
});

swipeDiv.on("swipedown", (ev) => {
  middleLayerLevel>1? goOneLayerUp() : console.log("no more layers above");
});
