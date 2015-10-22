/// <reference path="sharpbox.poco.d.ts"/>
/// <reference path="sharpbox.domain.ts"/>
/// <reference path="Typings/jquery.d.ts"/>
var sharpbox;
(function (sharpbox) {
    (function (Web) {
        var ViewModel = (function () {
            function ViewModel(instanceName) {
                this.controllerUrl = "./" + instanceName + "/";
            }
            ViewModel.prototype.getAll = function () {
                var url = this.controllerUrl + "Get/";
                $.get(url, function (data) {
                    this.collection = data;
                });
            };

            ViewModel.prototype.getById = function (id) {
                var url = this.controllerUrl + "GetById/" + id;
                $.get(url, function (data) {
                    this.instance = data;
                });
            };

            ViewModel.prototype.execute = function (action) {
                var url = this.controllerUrl + "Execute/";
                var webRequest = new sharpbox.WebLibrary.Core.WebRequest();

                webRequest = {
                    UiAction: action,
                    Instance: this.instance
                };

                $.post(url, webRequest, function (webResponse) {
                    this.processWebResponse(webResponse);
                }, 'json');
            };

            ViewModel.prototype.processWebResponse = function (webResponse) {
            };

            ViewModel.prototype.bindToForm = function (formName) {
                var form = $(formName);
                for (var i; i < this.instance; i++) {
                }
            };

            ViewModel.prototype.setProperty = function (name, value) {
                this.instance[name] = value;
            };
            return ViewModel;
        })();
        Web.ViewModel = ViewModel;
    })(sharpbox.Web || (sharpbox.Web = {}));
    var Web = sharpbox.Web;
})(sharpbox || (sharpbox = {}));

var test = new sharpbox.Web.ViewModel("UserRole");

test.instance = { Name: "Administrator", UserRoleNameId: 0 };

var wat = test.instance.UserRoleNameId;

test.execute(sharpbox.Domain.TestController.Command.Add);
//# sourceMappingURL=sharpbox.Web.js.map
