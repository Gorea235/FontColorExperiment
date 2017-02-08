// [root]
/// <reference path="../node_modules/@types/jquery/index.d.ts"/>

// resets user ID if it exists,
// then fetches a new user ID and starts the experiment
function clickStart() {
    if (userId()) {
        if (!confirm("Are you sure you want to reset the user ID?"))
        { return; }
    }
    startGetData("/api/experiment/user/new", (data) => {
        userId(data);
        goUrl("/experiment");
    },
        (_, textStatus) => {
            alertError(`An error occured while starting the experiment.\nSTATUS: ${textStatus}`)
        });
}

// if user ID exists, checks if it is valid,
// if so, redirects to experiment page,
// else clear it from storage
$(document).ready(() => {
    if (userId()) {
        startPostData("/api/experiment/user/exists", (data) => {
            logd(`user exists response: ${data}`)
            let value: boolean = data.toLowerCase() === "true";
            if (value) {
                logi("user id exists in server, continuing...");
                alert("An experiment is already progress, redirecting...");
                goUrl("/experiment");
            }
            else
            {
                logi("user id does not exist in server, resetting...")
                userIdReset();
            }
        }, (_, textStatus) => {
            alertError(`Unable to connect to server, status:\n${textStatus}`);
        }, { "id": userId() });
    }
})
