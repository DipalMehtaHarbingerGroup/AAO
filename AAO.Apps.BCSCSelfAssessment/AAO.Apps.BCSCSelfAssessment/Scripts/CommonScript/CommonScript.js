$(function () {
    setNavigation();
});
function setNavigation() {
    var path = window.location.pathname;
    $(".LayoutMainTabs .nav li").each(function () {
        $(this).removeClass('active');
        var href = $(this).find("a").attr("href");
        if (href != undefined) {
            if (path.substring(0, href.length) === href) {
                $(this).addClass('active');
            }

        }

    });
    var path = window.location.pathname;
    if (path == '/Dashboard') {
        $("#Dashboard").addClass('active');
    }
}
$(function () {
    $('#about-drop').popover({
        placement: 'bottom',
        html: true,
        content: $('#about-popover').html()
    })
})


$(document).keydown(function(event){
    if(event.keyCode==123){
        return false;
    }
    else if (event.ctrlKey && event.shiftKey && event.keyCode==73){
        return false;
    }
});

$(document).on("contextmenu",function(e){
    e.preventDefault();
});

var isCtrl = false;
document.onkeyup=function(e)
{
    if(e.which == 17)
        isCtrl=false;
}
document.onkeydown=function(e)
{
    if(e.which == 17)
        isCtrl=true;
    if ((e.which == 85 && isCtrl == true) || (e.which == 67 && isCtrl == true) && isCtrl == true)
    {
        return false;
    }
}
var isNS = (navigator.appName == "Netscape") ? 1 : 0;
if(navigator.appName == "Netscape") document.captureEvents(Event.MOUSEDOWN||Event.MOUSEUP);
function mischandler(){
    return false;
}
function mousehandler(e){
    var myevent = (isNS) ? e : event;
    var eventbutton = (isNS) ? myevent.which : myevent.button;
    if((eventbutton==2)||(eventbutton==3)) return false;
}
document.oncontextmenu = mischandler;
document.onmousedown = mousehandler;
document.onmouseup = mousehandler;
