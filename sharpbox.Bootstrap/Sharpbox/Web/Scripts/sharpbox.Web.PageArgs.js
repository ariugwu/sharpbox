/// <reference path="sharpbox.poco.d.ts"/>
/// <reference path="sharpbox.domain.ts"/>
/// <reference path="Typings/jquery.d.ts"/>
var sharpbox;
(function (sharpbox) {
    var Web;
    (function (Web) {
        var PageArgs = (function () {
            function PageArgs(urlParams) {
                this.url = window.location.pathname.split("/");
                this.controllerName = this.url[1] || "Environment";
                this.actionName = this.url[2];
                this.id = urlParams["id"];
            }
            return PageArgs;
        })();
        Web.PageArgs = PageArgs;
    })(Web = sharpbox.Web || (sharpbox.Web = {}));
})(sharpbox || (sharpbox = {}));
