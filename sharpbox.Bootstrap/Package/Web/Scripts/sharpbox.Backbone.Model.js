/// <reference path="sharpbox.enum.d.ts"/>
/// <reference path="sharpbox.poco.d.ts"/>
var sharpbox;
(function (sharpbox) {
    (function (WebModel) {
        var Model = (function () {
            function Model(instance, instanceName) {
                this.instance = instance;
                this.getUrl = "./" + instanceName + "/GetAll/";
                this.getByIdUrl = "./" + instanceName + "/GetById/";
                this.updateUrl = "./" + instanceName + "/Execute/";
                this.insertUrl = "./" + instanceName + "/Execute/";
                this.removeUrl = "./" + instanceName + "/Execute/";
            }
            Model.prototype.update = function () {
            };
            Model.prototype.insert = function () {
            };
            Model.prototype.remove = function () {
            };
            return Model;
        })();
        WebModel.Model = Model;
    })(sharpbox.WebModel || (sharpbox.WebModel = {}));
    var WebModel = sharpbox.WebModel;
})(sharpbox || (sharpbox = {}));

var test = new sharpbox.WebModel.Model({ Name: "Administrator", UserRoleNameId: 1 }, "UserRole");
//# sourceMappingURL=sharpbox.Backbone.Model.js.map
