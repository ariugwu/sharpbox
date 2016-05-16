/// <reference path="sharpbox.poco.d.ts"/>
/// <reference path="sharpbox.domain.ts"/>
/// <reference path="Typings/jquery.d.ts"/>
/// <reference path="Typings/jquery.dataTables.d.ts"/>

module sharpbox.Web {

    export class Scaffold<T> {
        viewModel: sharpbox.Web.ViewModel<any>;
        pageArgs: sharpbox.Web.PageArgs;
        htmlStrategy: sharpbox.Web.IHtmlStrategy;

        constructor(pageArgs: sharpbox.Web.PageArgs, htmlStrategy: sharpbox.Web.IHtmlStrategy) {
            this.pageArgs = pageArgs;
            this.htmlStrategy = htmlStrategy;
            this.viewModel = new sharpbox.Web.ViewModel<any>(this.pageArgs.controllerName);
        }

        gridEditClick = (id: string): void => {
            this.pageArgs.id = id;
            this.loadEditForm("#addPanel");
        }

        loadEditForm = (containerSelector: string): void => {
            this.viewModel.getSchema(() => {
                var formName = "UpdateForm";
                this.viewModel.form = new sharpbox.Web.Form(this.viewModel.schema, formName, this.viewModel.instanceName, this.viewModel.controllerUrl, "Update", "POST", this.htmlStrategy);
                var template = sharpbox.Web.Templating.editForm(this.viewModel);

                $(containerSelector).html(template);
                //$(".daterange").daterangepicker();
                this.viewModel.form.htmlStrategy.wireSubmit(formName);

                if (this.pageArgs.id != null) {
                    this.viewModel.getById(this.pageArgs.id, () => {
                        this.viewModel.form.bindToForm(this.viewModel.instance);
                    });
                }
            });
        }

        loadSearchPage = (containerSelector: string): void => {          
            this.viewModel.getSchema(() => {
                var formName = "SearchForm";
                var self = this;
                self.viewModel.form = new sharpbox.Web.Form(self.viewModel.schema, formName, self.viewModel.instanceName, self.viewModel.controllerUrl, "Search", "Get", this.htmlStrategy);
                var searchDash = sharpbox.Web.Templating.searchDash(self.viewModel);
                $("#searchPanel").html(searchDash);
                self.loadEditForm("#addPanel");
                //$(".daterange").daterangepicker();

                self.viewModel.form.htmlStrategy.wireSearchSubmit(formName, containerSelector, self.reloadGrid);
                self.loadGrid(containerSelector, "");

            });
        }
        reloadGrid = (containerSelector: string, odataQuery: string) => {
            this.loadGrid(containerSelector, odataQuery);
        }

        loadGrid = (containerSelector: string, odataQuery: string) => {
            var self = this;
            this.viewModel.getAll(odataQuery,(data) => {
                this.viewModel.getSchema(() => {
                    var table = this.htmlStrategy.makeTable(data, this.pageArgs.controllerName);

                    $(containerSelector).find(".gridPanel").html(table);
                    
                    var settings: DataTables.Settings = new ScaffoldDataTableSettings();
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
        }
    }

    class ScaffoldDataTableSettings implements  DataTables.Settings {
        buttons: string[];
    }
}

