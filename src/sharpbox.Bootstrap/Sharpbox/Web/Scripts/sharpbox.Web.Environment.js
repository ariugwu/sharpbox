/// <reference path="sharpbox.poco.d.ts"/>
/// <reference path="sharpbox.domain.ts"/>
/// <reference path="Typings/jquery.d.ts"/>
var sharpbox;
(function (sharpbox) {
    var Web;
    (function (Web) {
        var Site = (function () {
            function Site() {
                this.viewModel = new sharpbox.Web.ViewModel("environment");
            }
            Site.prototype.loadEnvironmentById = function (id, callback) {
                var _this = this;
                $.getJSON("/Environment/GetById?id=" + id, function (data) {
                    _this.environment = data;
                    callback();
                });
            };
            return Site;
        })();
        Web.Site = Site;
    })(Web = sharpbox.Web || (sharpbox.Web = {}));
})(sharpbox || (sharpbox = {}));
