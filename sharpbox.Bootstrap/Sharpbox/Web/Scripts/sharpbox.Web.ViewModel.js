/// <reference path="sharpbox.poco.d.ts"/>
/// <reference path="sharpbox.domain.ts"/>
/// <reference path="sharpbox.Web.Form.ts"/>
/// <reference path="Typings/jquery.d.ts"/>
var sharpbox;
(function (sharpbox) {
    var Web;
    (function (Web) {
        var ViewModel = (function () {
            function ViewModel(instanceName) {
                this.instanceName = instanceName;
                this.controllerUrl = "/" + instanceName + "/";
            }
            ViewModel.prototype.getAll = function (callback) {
                var _this = this;
                var url = this.controllerUrl + "Get/";
                $.get(url, function (data) {
                    _this.collection = data;
                }).done(function (data) {
                    callback();
                });
            };
            ViewModel.prototype.getById = function (id, callback) {
                var _this = this;
                var url = this.controllerUrl + "GetById/?id=" + id;
                $.get(url, function (data) {
                    _this.instance = data;
                }).done(function (data) {
                    callback();
                });
            };
            ViewModel.prototype.getSchema = function (onSchemaLoad) {
                var _this = this;
                var url = this.controllerUrl + "JsonSchema/";
                $.getJSON(url, function (data) {
                    _this.schema = JSON.parse(data);
                }).done(function (data) {
                    onSchemaLoad();
                });
            };
            ViewModel.prototype.execute = function (action) {
                var self = this;
                var url = this.controllerUrl + "Execute/";
                var webRequest = {};
                webRequest = {
                    UiAction: action,
                    Instance: this.instance
                };
                $.post(url, webRequest, function (data) {
                    self.processWebResponse(data);
                }, "json");
            };
            ViewModel.prototype.processWebResponse = function (webResponse) {
            };
            ViewModel.prototype.setProperty = function (name, value) {
                this.instance[name] = value;
            };
            return ViewModel;
        })();
        Web.ViewModel = ViewModel;
    })(Web = sharpbox.Web || (sharpbox.Web = {}));
})(sharpbox || (sharpbox = {}));
//# sourceMappingURL=sharpbox.Web.ViewModel.js.map