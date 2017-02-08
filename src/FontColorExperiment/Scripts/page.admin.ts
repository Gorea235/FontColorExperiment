// [root]/admin
/// <reference path="../node_modules/@types/jquery/index.d.ts"/>

const _experimentDataUploadId: string = "#experimentDataUpload";

// sends a user reset request
function clickResetUsers() {
    startGetData("/api/admin/resetusers", () => {
        alert("Users have been reset");
        window.location.reload();
    }, () => {
        alert("Unable to reset users!");
        goUrl("/admin/login");
    });
}

// sends a logout request
function clickLogout() {
    startGetData("/api/admin/logout", () => {
        goUrl("/admin");
    }, () => {
        alert("Unable to logout... goddammit...");
    });
}
