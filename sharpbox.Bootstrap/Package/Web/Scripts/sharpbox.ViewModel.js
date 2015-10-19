/// <reference path="sharpbox.poco.d.ts"/>
/// <reference path="sharpbox.controllers.ts"/>
/// <reference path="Typings/jquery.d.ts"/>
var sharpbox;
(function (sharpbox) {
    (function (ViewModel) {
        var Model = (function () {
            function Model(instanceName) {
                this.controllerUrl = "./" + instanceName + "/";
            }
            Model.prototype.getAll = function () {
                var url = this.controllerUrl + "Get/";
                $.get(url, function (data) {
                    this.collection = data;
                });
            };

            Model.prototype.getById = function (id) {
                var url = this.controllerUrl + "GetById/" + id;
                $.get(url, function (data) {
                    this.instance = data;
                });
            };

            Model.prototype.execute = function (action) {
                var url = this.controllerUrl + "Execute/";

                var webRequest = {
                    UiAction: action,
                    Instance: this.instance
                };

                $.post(url, webRequest, function (webResponse) {
                    this.processWebResponse(webResponse);
                }, 'json');
            };

            Model.prototype.processWebResponse = function (webResponse) {
            };

            Model.prototype.bindToForm = function (formName) {
                var form = $(formName);
                for (var i; i < this.instance; i++) {
                }
            };

            Model.prototype.setProperty = function (name, value) {
                this.instance[name] = value;
            };
            return Model;
        })();
        ViewModel.Model = Model;
    })(sharpbox.ViewModel || (sharpbox.ViewModel = {}));
    var ViewModel = sharpbox.ViewModel;
})(sharpbox || (sharpbox = {}));

var test = new sharpbox.ViewModel.Model("UserRole");

test.instance = { Name: "Administrator", UserRoleNameId: 0 };

var wat = test.instance.UserRoleNameId;

test.execute(sharpbox.Controllers.TestController.Command[2 /* Add */]);
//# sourceMappingURL=sharpbox.ViewModel.js.map
