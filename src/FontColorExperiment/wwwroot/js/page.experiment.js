// [root]/test
/// <reference path="../node_modules/@types/jquery/index.d.ts"/>
var currentDisplay = 0; // the current type of the display, set by the view when loaded
var _dispId = "#experimentDisplay"; // the id of the element storing the display
var _btnContinueId = "#btnContinue"; // the id of the continue button
var _questionGroup = "#questionGroup";
// displays an alert informing the user that the connection failed
function failedUpdate(_, textStatus) {
    logi("connection to server failed failed, status: " + textStatus);
    alertError("Unable to connect to the server, please try again. If the problem persists, please"
        + (" contact an admin and give the follow message:\nSTATUS: " + textStatus));
}
// checks that the step request was a success
// if so, then execute the function, otherwise alert user
function checkStepFunc(success) {
    return function (data) {
        logd("received data " + JSON.stringify(data) + " from server");
        if (data.success) {
            success();
        }
        else {
            alertError("Unable to continue, please try again. If you have already seen this message, please contact an" +
                (" admin and give them this message:\n" + data.error.message));
        }
    };
}
// steps the current user on the server and fetches the new page on success
function nextStep() {
    startPostData("/api/experiment/user/step", checkStepFunc(beginReadyPage), failedUpdate, { "id": userId() });
}
// reloads the page to re-attempt the connection
function clickError() {
    logi("attempting to re-fetch the page");
    window.location.reload();
}
// clears user ID and goes to the main page
function clickFinish() {
    logi("finishing experiment and clearing user");
    userIdReset();
    goUrl("/");
}
// starts a new step request
function clickSample() {
    logi("start user step push");
    nextStep();
}
// gets the answers of the user from the current controls
function getQuestionAnswers() {
    var answers = {}; // {"question id": answer} answer: int, int[], string
    var questionIds = JSON.parse($(_questionGroup).attr("data-questions"));
    var value;
    for (var i = 0; i < questionIds.length; i++) {
        var questionContainer = $("#question-" + questionIds[i]);
        switch ($(questionContainer).attr("data-questiontype")) {
            case "0":
                value = $("input[name=question-" + questionIds[i] + "]:checked").val();
                logd("question " + questionIds[i] + " value: " + value);
                if (value) {
                    answers[questionIds[i]] = parseInt(value);
                }
                else {
                    return null;
                }
                break;
            case "1":
                value = $("input[name=question-" + questionIds[i] + "]:checked");
                logd("question " + questionIds[i] + " checked: " + value);
                if (!value) {
                    return null;
                }
                var values = [];
                for (var i = 0; i < value.length; i++) {
                    values.push(parseInt($(value[i]).val()));
                }
                answers[questionIds[i]] = values;
                break;
            case "2":
                value = $("#question-select-" + questionIds[i]).val();
                logd("question " + questionIds[i] + " value: " + value);
                if (value) {
                    answers[questionIds[i]] = parseInt(value);
                }
                else {
                    return null;
                }
                break;
            case "3":
                value = $("#question-text-" + questionIds[i]).val();
                logd("question " + questionIds[i] + " value: " + value);
                if (value) {
                    answers[questionIds[i]] = value;
                }
                else {
                    return null;
                }
                break;
            case "4":
                // ignore
                break;
        }
    }
    logi("built question answers: " + JSON.stringify(answers));
    return answers;
}
// checks if all questions have an answer,
// and submits and steps if so,
// else alerts user
function clickQuestion() {
    logi("starting question submition and user step push");
    var answers = getQuestionAnswers();
    if (answers) {
        startPostData("/api/experiment/user/answer", checkStepFunc(nextStep), failedUpdate, { "id": userId(), "answers": JSON.stringify(answers) });
    }
    else {
        alertError("Please fill in all of the questions before continuing.");
    }
}
// sets up the initial page data
function beginReadyPage() {
    if (userId()) {
        startPostData("/api/experiment/view", setupPage, function (_, textStatus) {
            alertError("An error occured while loading the page. Please give an admin this message:\n" + textStatus);
        }, { "id": userId() });
    }
    else {
        goUrl("/");
    }
}
// set's up the page with the data from the server
function setupPage(data) {
    logi("setting up page information and events");
    displayData(_dispId, data); // display view
    switch (currentDisplay) {
        case 0:
            logd("error page loaded");
            break;
        case 1:
            $(_btnContinueId).attr("onclick", "clickError()").prop("disabled", true);
            setTimeout(function () { $(_btnContinueId).removeAttr("disabled"); }, 5000);
            $(_btnContinueId).attr("onclick", "clickFinish()");
            logd("finish page loaded");
            break;
        case 2:
            $(_btnContinueId).attr("onclick", "clickSample()");
            logd("sample page loaded");
            break;
        case 3:
            $(_btnContinueId).attr("onclick", "clickQuestion()");
            logd("question page loaded");
            break;
    }
}
$(document).ready(beginReadyPage);
//# sourceMappingURL=page.experiment.js.map