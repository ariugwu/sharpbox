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
                this.lookUpDictionary = new collections.Dictionary();
            }
            ViewModel.prototype.getAll = function (callback) {
                var url = this.controllerUrl + "Get/";
                $.getJSON(url, { _: new Date().getTime() }).done(function (data) {
                    callback(data);
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
            // Assume that you have a property "FooId" and it's *not* the primary key. We assume this is a lookup value to populate a tag or select field
            // We want to grab the lookup data from that controllers cached method
            ViewModel.prototype.getPropertyDataForLookup = function (lookupName, callback) {
                var _this = this;
                var url = "/" + lookupName + "/GetAsLookUpDictionary/";
                $.getJSON(url, function (data) {
                    var lookupData = [];
                    $.each(data, function (key, item) {
                        lookupData.push({ key: key, item: item });
                    });
                    _this.lookUpDictionary.setValue(lookupName, data);
                }).done(function (data) {
                    callback();
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