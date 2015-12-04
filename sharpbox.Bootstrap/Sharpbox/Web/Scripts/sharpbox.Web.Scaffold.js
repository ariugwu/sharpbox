/// <reference path="sharpbox.poco.d.ts"/>
/// <reference path="sharpbox.domain.ts"/>
/// <reference path="Typings/jquery.d.ts"/>
/// <reference path="Typings/jquery.dataTables.d.ts"/>
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
                this.loadSearchPage = function (containerSelector) {
                    _this.viewModel.getSchema(function () {
                        var formName = "SearchForm";
                        _this.viewModel.form = new sharpbox.Web.Form(_this.viewModel.schema, formName, _this.viewModel.instanceName, _this.viewModel.controllerUrl, "Search", "Get", _this.htmlStrategy);
                        var searchDash = sharpbox.Web.Templating.searchDash(_this.viewModel);
                        $(containerSelector).find(".searchPanel").html(searchDash);
                        _this.viewModel.form.htmlStrategy.wireSearchSubmit(formName, containerSelector, _this.reloadGrid);
                        _this.loadGrid(containerSelector, "");
                    });
                };
                this.reloadGrid = function (containerSelector, odataQuery) {
                    _this.loadGrid(containerSelector, odataQuery);
                };
                this.loadGrid = function (containerSelector, odataQuery) {
                    _this.viewModel.getAll(odataQuery, function (data) {
                        _this.viewModel.getSchema(function () {
                            var table = _this.htmlStrategy.makeTable(data, _this.pageArgs.controllerName);
                            $(containerSelector).find(".gridPanel").html(table);
                            var settings = new ScaffoldDataTableSettings();
                            settings.dom = 'Bfrtip';
                            settings.buttons = ['copy', 'excel', 'pdf'];
                            $(containerSelector).find("table").DataTable(settings);
                        });
                    });
                };
                this.pageArgs = pageArgs;
                this.htmlStrategy = htmlStrategy;
                this.viewModel = new sharpbox.Web.ViewModel(this.pageArgs.controllerName);
            }
            return Scaffold;
        })();
        Web.Scaffold = Scaffold;
        var ScaffoldDataTableSettings = (function () {
            function ScaffoldDataTableSettings() {
            }
            return ScaffoldDataTableSettings;
        })();
    })(Web = sharpbox.Web || (sharpbox.Web = {}));
})(sharpbox || (sharpbox = {}));
