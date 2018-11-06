/// <reference path="~/Scripts/jquery.validate.js" />
/// <reference path="~/Scripts/jquery.validate.unobtrusive.js" />

/*
    Note:
    Somehow using the jQuery document.ready() syntax $(function () { });
    does not work when it comes to triggering unobstrusive validation.
*/

// https://code.msdn.microsoft.com/MVC-File-upload-unobtrusive-d0099c30
(function () {
    $.validator.unobtrusive.adapters.addSingleVal("validfiletype", "filetypes");
    $.validator.unobtrusive.adapters.addSingleVal("maximumfilesize", "size");
    $.validator.unobtrusive.adapters.addSingleVal("minimumfilesize", "size");    

    $.validator.addMethod('validfiletype', function (value, element, validFileTypes) {
        var validFileTypesDelimiter = ',';
        var fileTypeDelimiter = '.';

        if (validFileTypes.indexOf(validFileTypesDelimiter) > -1) {
            validFileTypes = validFileTypes.split(validFileTypesDelimiter);
        } else {
            validFileTypes = [validFileTypes];
        }

        var splitFileValueByTypeDelimiter = value.split(fileTypeDelimiter);
        var fileType = splitFileValueByTypeDelimiter[splitFileValueByTypeDelimiter.length - 1];

        for (var i = 0; i < validFileTypes.length; i++) {
            if (validFileTypes[i].toUpperCase() === fileType.toUpperCase()) {
                return true;
            }
        }

        return false;
    });

    $.validator.addMethod('maximumfilesize', function (value, element, maxSize) {
        // use < rather than <= as 0 file size is not allowed
        return convertBytesToMegabytes(element.files[0].size) < parseFloat(maxSize);
    });

    $.validator.addMethod('minimumfilesize', function (value, element, minSize) {
        return convertBytesToMegabytes(element.files[0].size) >= parseFloat(minSize);
    });

    //$.validator.unobtrusive.adapters.add('fileuploadvalidator', ['clientvalidationmethods', 'parameters', 'errormessages'], function (options) {
    //    options.rules['fileuploadvalidator'] = {
    //        clientvalidationmethods: options.params['clientvalidationmethods'].split(','),
    //        parameters: options.params['parameters'].split('|'),
    //        errormessages: options.params['errormessages'].split(',')
    //    };
    //});

    //$.validator.addMethod("fileuploadvalidator", function (value, element, param) {
    //    if (value == "" || value == null || value == undefined) {
    //        return true;
    //    }
    //    //array of jquery validation rule names 
    //    var validationrules = param["clientvalidationmethods"];

    //    //array of paramteres required by rules, in this case regex patterns 
    //    var patterns = param["parameters"];

    //    //array of error messages for each rule 
    //    var rulesErrormessages = param["errormessages"];

    //    var validNameErrorMessage = new Array();
    //    var index = 0

    //    for (i = 0; i < patterns.length; i++) {
    //        var valid = true;
    //        var pattern = patterns[i].trim();

    //        //get a jquery validator method.   
    //        var rule = $.validator.methods[validationrules[i].trim()];

    //        //create a paramtere object 
    //        var parameter = new Object();
    //        parameter = pattern;

    //        //execute the rule 
    //        var isValid = rule.call(this, value, element, parameter);

    //        if (!isValid) {
    //            //if rule fails, add error message 
    //            validNameErrorMessage[index] = rulesErrormessages[i];
    //            index++;
    //        }
    //    }
    //    //if we have more than on error message, one of the rule has failed 
    //    if (validNameErrorMessage.length > 0) {
    //        //update the error message for 'validname' rule 
    //        $.validator.messages.fileuploadvalidator = validNameErrorMessage.toString();
    //        return false;
    //    }
    //    return true;
    //}, "This is not a valid individual name");  //default error message 

    function convertBytesToMegabytes(bytes) {
        return (bytes / 1024) / 1024;
    }
})();