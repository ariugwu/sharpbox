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
                this.controllerUrl = "/" + instanceName + "/";
            }
            ViewModel.prototype.getAll = function () {
                var _this = this;
                var url = this.controllerUrl + "Get/";
                $.get(url, function (data) {
                    _this.collection = data;
                });
            };
            ViewModel.prototype.getById = function (id) {
                var _this = this;
                var url = this.controllerUrl + "GetBySharpId/" + id;
                $.get(url, function (data) {
                    _this.instance = data;
                });
            };
            ViewModel.prototype.getSchema = function () {
                var _this = this;
                var url = this.controllerUrl + "JsonSchema/";
                $.getJSON(url, function (data) {
                    _this.schema = JSON.parse(data);
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
    })(Web = sharpbox.Web || (sharpbox.Web = {}));
})(sharpbox || (sharpbox = {}));
var test = new sharpbox.Web.ViewModel("Environment");
test.instance = {
    EnvironmentId: 0,
    ApplicationName: "Sample Application",
    BaseUrl: "http://example.com",
    CacheKey: "any",
    UploadDirectory: "~/Uploads/",
    LogoLocation: "~/Images/Logo",
    BrandTypeId: 1,
    BrandType: null,
    EnvironmentTypeId: 1,
    EnvironmentType: null,
    TechSheetId: 1,
    TechSheet: null,
    SharpId: null
};
var wat = test.instance.BrandTypeId;
test.execute(sharpbox.Domain.TestController.Command.Update);
