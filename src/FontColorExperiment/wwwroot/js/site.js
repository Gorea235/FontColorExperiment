// references - used instead of require because it is not needed
/// <reference path="../node_modules/@types/jquery/index.d.ts"/>
// main site code
// general helper functions
var _debug = false; // whether in debug or release, debug set by _Layout.cshtml
var _modalError = "#modalError"; // the id of the error modal
var _modalError_Text = "#modalError-Text"; // the id of the error modal text
var _userIdKey = "fontexperiment-userid"; // the key of the user id storage
// shows an error modal with the specified text
function alertError(text) {
    logd("showing error modal with data '" + text + "'");
    $(_modalError_Text).text(text);
    $(_modalError).modal("show");
}
// gets or sets the current user ID
function userId(id) {
    if (id == null) {
        return localStorage.getItem(_userIdKey);
    }
    else {
        localStorage.setItem(_userIdKey, id);
    }
}
// resets the current user ID
function userIdReset() {
    localStorage.removeItem(_userIdKey);
}
// goes to the specified URL
function goUrl(url) {
    logd("going to " + url);
    window.location.href = url;
}
// writes the text to the log if in debug mode
function logi(info) {
    if (window.console && window.console.log) {
        console.log(info);
    }
}
// writes the text to the log
function logd(info) {
    if (_debug) {
        logi(info);
    }
}
// ajax helper methods
// perform GET request
function startGetData(path, done, fail, data) {
    _startAjaxRequest(path, {
        url: path,
        data: data,
        cache: false,
        method: "GET"
    }, done, fail);
}
// perform POST request
function startPostData(path, done, fail, data) {
    _startAjaxRequest(path, {
        url: path,
        data: data,
        cache: false,
        method: "POST"
    }, done, fail);
}
// general request function
function _startAjaxRequest(path, options, done, fail) {
    logd("sending post request to " + path);
    var query = $.ajax(options).done(done);
    if (fail) {
        query.fail(fail);
    }
}
// displays the html string in the specified element with the given ID
function displayData(id, data) {
    $(id).html(data);
}
//# sourceMappingURL=site.js.map