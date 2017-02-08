// [root]/admin
/// <reference path="../node_modules/@types/jquery/index.d.ts"/>
var _experimentDataUploadId = "#experimentDataUpload";
// sends a user reset request
function clickResetUsers() {
    startGetData("/api/admin/resetusers", function () {
        alert("Users have been reset");
        window.location.reload();
    }, function () {
        alert("Unable to reset users!");
        goUrl("/admin/login");
    });
}
// sends a logout request
function clickLogout() {
    startGetData("/api/admin/logout", function () {
        goUrl("/admin");
    }, function () {
        alert("Unable to logout... goddammit...");
    });
}
//# sourceMappingURL=page.admin.js.map