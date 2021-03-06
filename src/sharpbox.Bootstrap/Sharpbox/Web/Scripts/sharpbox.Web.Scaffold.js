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
                this.gridEditClick = function (id) {
                    _this.pageArgs.id = id;
                    _this.loadEditForm("#addPanel");
                };
                this.loadEditForm = function (containerSelector) {
                    _this.viewModel.getSchema(function () {
                        var formName = "UpdateForm";
                        _this.viewModel.form = new sharpbox.Web.Form(_this.viewModel.schema, formName, _this.viewModel.instanceName, _this.viewModel.controllerUrl, "Update", "POST", _this.htmlStrategy);
                        var template = sharpbox.Web.Templating.editForm(_this.viewModel);
                        $(containerSelector).html(template);
                        //$(".daterange").daterangepicker();
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
                        var self = _this;
                        self.viewModel.form = new sharpbox.Web.Form(self.viewModel.schema, formName, self.viewModel.instanceName, self.viewModel.controllerUrl, "Search", "Get", _this.htmlStrategy);
                        var searchDash = sharpbox.Web.Templating.searchDash(self.viewModel);
                        $("#searchPanel").html(searchDash);
                        self.loadEditForm("#addPanel");
                        //$(".daterange").daterangepicker();
                        self.viewModel.form.htmlStrategy.wireSearchSubmit(formName, containerSelector, self.reloadGrid);
                        self.loadGrid(containerSelector, "");
                    });
                };
                this.reloadGrid = function (containerSelector, odataQuery) {
                    _this.loadGrid(containerSelector, odataQuery);
                };
                this.loadGrid = function (containerSelector, odataQuery) {
                    var self = _this;
                    _this.viewModel.getAll(odataQuery, function (data) {
                        _this.viewModel.getSchema(function () {
                            var table = _this.htmlStrategy.makeTable(data, _this.pageArgs.controllerName);
                            $(containerSelector).find(".gridPanel").html(table);
                            var settings = new ScaffoldDataTableSettings();
                            //settings.dom = 'Bfrtip';
                            settings.buttons = ['copy', 'excel', 'pdf'];
                            $(containerSelector).find("table").DataTable(settings);
                            $(".editGrid").on("click", function (e) {
                                e.preventDefault();
                                var id = $(this).val();
                                self.gridEditClick(id);
                                $("#showAddPanel").click();
                            });
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
//# sourceMappingURL=sharpbox.Web.Scaffold.js.map