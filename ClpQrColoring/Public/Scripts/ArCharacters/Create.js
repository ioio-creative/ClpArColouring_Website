/// <reference path="~/Scripts/jquery.validate.js" />

/* globals */

// TODO: This string array contains all the modalIds
// Need to update the modalIds if any modals are added or removed
var modalIds = ['tipsModal', 'imgUploadedModal',
    'videoRenderingModal'];

/* end of globals */


$(function () {
    // show tips modal on page loads
    hideAllModals();
    $('#tipsModal').modal();    

    // https://stackoverflow.com/questions/12308138/jquery-validator-plugin-with-displaynone-form-elements
    // https://stackoverflow.com/questions/22613983/jquery-validate-ignore-and-more
    // jquery validator plugin with display:none form elements
    // file upload input element is (display: none) but it still requires validation
    $.validator.setDefaults({
        ignore: []
    });


    /* registering event handlers */

    // https://stackoverflow.com/questions/5794039/how-can-i-create-a-custom-file-upload-with-only-html-js-and-css *@
    // http://jsfiddle.net/edelman/sSSNj/    

    /*
        Important Note:
        Triggering fileInput.click() this way does not invoke unobstrusive validation.
        Needs to manually trigger validation on events.
    */
    // for controlling file upload behaviour
    $('#btnUploadFile').click(function () {
        // https://stackoverflow.com/questions/12030686/html-input-file-selection-event-not-firing-upon-selecting-the-same-file
        $('#PostedFile').click();

        // !!!Important!!! have to return false
        // otherwise, since btnUploadFile is inside an html form
        // returning true would cause a post request to server 
        return false;
    });    

    // for controlling file upload behaviour
    $('#PostedFile').change(function () {
        $('#lbSelectedFileName').text($(this).val().replace(/^.*[\\\/]/, ''));
        $(this).valid();
    });

    /* end of registering event handlers */


    /* swapping images in imgUploadedModal */

    var isFanShown = true;
    var imgUploadedModalSetInterval = setInterval(
        function () {
            // if imgUploadedModal is shown
            // https://stackoverflow.com/questions/19674701/can-i-check-if-bootstrap-modal-shown-hidden
            if ($('#imgUploadedModal').hasClass('in')) {
                if (isFanShown) {                    
                    hideItem('imgProgressModalFan');
                    hideItem('imgProgressModalFanMsg');

                    showItem('imgProgressModalAirCon');
                    showItem('imgProgressModalAirConMsg');
                } else {
                    showItem('imgProgressModalFan');
                    showItem('imgProgressModalFanMsg');

                    hideItem('imgProgressModalAirCon');
                    hideItem('imgProgressModalAirConMsg');
                }
                isFanShown = !isFanShown;
            }                
        }, 4000
    );
    
    /* end of swapping images in imgUploadedModal */
});


/* hiding all modals in Create.cshtml */

function hideAllModals() {
    for (var i = 0; i < modalIds.length; i++) {
        hideModal(modalIds[i]);
    }
}

/* end of hiding all modals in Create.cshtml */