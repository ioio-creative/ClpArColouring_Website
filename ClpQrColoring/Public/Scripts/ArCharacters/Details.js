$(function () {
    /* deciding whether to show whatsapp button */

    if (isMobileBrowser()) {
        showItem('divWhatsapp');
        $('#divSharingBtns > div').each(function () {
            $(this).addClass('col-xs-3');
            $(this).removeClass('col-xs-4');
        });
    } else {
        hideItem('divWhatsapp');
        $('#divSharingBtns > div').each(function () {
            $(this).addClass('col-xs-4');
            $(this).removeClass('col-xs-3');
        });
    }

    /* end of deciding whether to show whatsapp button */
    
    /* The following is not used any more as I now use node.js */
    /* http://videojs.com/ */

    ///* controlling video controls */
    //// show video controls only when hover

    //var videoOutput = document.getElementById("videoOutput");
    //var hasVideoStarted = false;

    //// https://jsfiddle.net/hibbard_eu/3qe1a9bc/
    //$('#videoOutput').on({
    //    mouseenter: function () {
    //        if (!hasVideoStarted) {
    //            videoOutput.play();
    //            hasVideoStarted = true;
    //        }
    //        videoOutput.setAttribute("controls", "controls")
    //    },
    //    mouseleave: function () {
    //        videoOutput.removeAttribute("controls");
    //    },
    //});

    ///* end of controlling video controls */
});