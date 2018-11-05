/* http://www.dotnettricks.com/learn/mvc/file-upload-with-strongly-typed-view-and-model-validation */

function getSingleFileSize(inputFileElementId) {    
    try {
        var fileSize = 0;
        //for IE
        if ($.browser.msie) {
            //before making an object of ActiveXObject, 
            //please make sure ActiveX is enabled in your IE browser
            var objFSO = new ActiveXObject("Scripting.FileSystemObject");
            var filePath = $("#" + fileid)[0].value;
            var objFile = objFSO.getFile(filePath);
            var fileSize = objFile.size; //size in kb
            fileSize = fileSize / 1048576; //size in mb 
        }
        //for FF, Safari, Opeara and Others
        else {
            fileSize = $("#" + inputFileElementId)[0].files[0].size //size in kb
            fileSize = fileSize / 1048576; //size in mb 
        }
        return fileSize;
    } catch (err) {
        alert("Error while checking file size: " + err);
    }
}

function getFileNameFromPath(strFilepath) {
    var objRE = new RegExp(/([^\/\\]+)$/);    
    var strName = objRE.exec(strFilepath);

    if (strName == null) {
        return null;
    }
    else {
        return strName[0];
    }
}

// not including the dot '.'
function getFileExtFromPath(strFilePath) {
    var fileName = getFileNameFromPath(strFilePath);
    return fileName.substr((file.lastIndexOf('.') + 1));
}

// string elements in strArrAllowedExts should not include the dot '.'
// a valid example of strArrAllowedExts is ['jpg', 'jpeg', 'png']
function isFileExtAllowed(strFilePath, strArrAllowedExts) {
    var isAllowed = false;
    var fileExt = getFileExtFromPath(strFilePath);
    for (var i = 0; i < strArrAllowedExts.length; i++) {
        if (fileExt == strArrAllowedExts[i]) {
            isAllowed = true;
            break;
        }
    }
    return isAllowed;
}