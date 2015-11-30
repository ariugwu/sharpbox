/// <reference path="sharpbox.poco.d.ts"/>
/// <reference path="sharpbox.domain.ts"/>
/// <reference path="Typings/jquery.d.ts"/>
var sharpbox;
(function (sharpbox) {
    var Web;
    (function (Web) {
        var Scaffold = (function () {
            function Scaffold(pageArgs, htmlStrategy) {
                var _this = this;
                this.loadEditForm = function (containerSelector) {
                    _this.viewModel.getSchema(function () {
                        var formName = "UpdateForm";
                        _this.viewModel.form = new sharpbox.Web.Form(_this.viewModel.schema, formName, _this.viewModel.instanceName, _this.viewModel.controllerUrl, "Update", "POST", _this.htmlStrategy);
                        var template = sharpbox.Web.Templating.editForm(_this.viewModel);
                        $(containerSelector).html(template);
                        _this.viewModel.form.htmlStrategy.wireSubmit(formName);
                        if (_this.pageArgs.id != null) {
                            _this.viewModel.getById(_this.pageArgs.id, function () {
                                _this.viewModel.form.bindToForm(_this.viewModel.instance);
                            });
                        }
                    });
                };
                this.loadGrid = function (containerSelector) {
                    _this.viewModel.getAll(function (data) {
                        var table = _this.htmlStrategy.makeTable(data, _this.pageArgs.controllerName);
                        $(containerSelector).append(table);
                    });
                };
                this.pageArgs = pageArgs;
                this.htmlStrategy = htmlStrategy;
                this.viewModel = new sharpbox.Web.ViewModel(this.pageArgs.controllerName);
            }
            return Scaffold;
        })();
        Web.Scaffold = Scaffold;
    })(Web = sharpbox.Web || (sharpbox.Web = {}));
})(sharpbox || (sharpbox = {}));
//# sourceMappingURL=sharpbox.Web.Scaffold.js.map