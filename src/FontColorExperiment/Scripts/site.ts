// references - used instead of require because it is not needed
/// <reference path="../node_modules/@types/jquery/index.d.ts"/>
// main site code

// general helper functions

const _debug: boolean = false; // whether in debug or release, debug set by _Layout.cshtml
const _modalError: string = "#modalError"; // the id of the error modal
const _modalError_Text: string = "#modalError-Text"; // the id of the error modal text
const _userIdKey: string = "fontexperiment-userid"; // the key of the user id storage

// shows an error modal with the specified text
function alertError(text: string) {
    logd(`showing error modal with data '${text}'`);
    $(_modalError_Text).text(text);
    (<any>$(_modalError)).modal("show");
}

// gets or sets the current user ID
function userId(id?: string) {
    if (id == null) { return localStorage.getItem(_userIdKey); }
    else { localStorage.setItem(_userIdKey, id); }
}

// resets the current user ID
function userIdReset() {
    localStorage.removeItem(_userIdKey);
}

// goes to the specified URL
function goUrl(url: string) {
    logd(`going to ${url}`);
    window.location.href = url;
}

// writes the text to the log if in debug mode
function logi(info?: any) {
    if (window.console && window.console.log)
    { console.log(info); }
}

// writes the text to the log
function logd(info?: any) {
    if (_debug)
    { logi(info); }
}

// ajax helper methods

// perform GET request
function startGetData(path: string,
    done: ((data?: any, textStatus?: any, jqXHR?: any) => void) | ((data?: any, textStatus?: any, jqXHR?: any) => void)[],
    fail?: (jqXHR?: any, textStatus?: any, errorThrown?: any) => void,
    data?: any) {
    _startAjaxRequest(path,
        {
            url: path,
            data: data,
            cache: false,
            method: "GET"
        },
        done,
        fail);
}

// perform POST request
function startPostData(path: string,
    done: ((data?: any, textStatus?: any, jqXHR?: any) => void) | ((data?: any, textStatus?: any, jqXHR?: any) => void)[],
    fail?: (jqXHR?: any, textStatus?: any, errorThrown?: any) => void,
    data?: any) {
    _startAjaxRequest(path,
        {
            url: path,
            data: data,
            cache: false,
            method: "POST"
        },
        done,
        fail);
}

// general request function
function _startAjaxRequest(path: string,
    options: JQueryAjaxSettings,
    done: ((data?: any, textStatus?: any, jqXHR?: any) => void) | ((data?: any, textStatus?: any, jqXHR?: any) => void)[],
    fail?: (jqXHR?: any, textStatus?: any, errorThrown?: any) => void) {
    logd(`sending post request to ${path}`);
    var query = $.ajax(options).done(done);
    if (fail) { query.fail(fail); }
}

// displays the html string in the specified element with the given ID
function displayData(id: string, data: string) {
    $(id).html(data);
}
