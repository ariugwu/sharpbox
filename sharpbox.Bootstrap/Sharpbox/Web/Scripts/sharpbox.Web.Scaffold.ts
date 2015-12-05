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
                this.viewModel.form = new sharpbox.Web.Form(this.viewModel.schema, formName, this.viewModel.instanceName, this.viewModel.controllerUrl, "Search", "Get", this.htmlStrategy);
                var searchDash = sharpbox.Web.Templating.searchDash(this.viewModel);
                $("#searchPanel").html(searchDash);
                this.loadEditForm("#addPanel");
                //$(".daterange").daterangepicker();

                this.viewModel.form.htmlStrategy.wireSearchSubmit(formName, containerSelector, this.reloadGrid);
                this.loadGrid(containerSelector, "");
            });
        }
        reloadGrid = (containerSelector: string, odataQuery: string) => {
            this.loadGrid(containerSelector, odataQuery);
        }

        loadGrid = (containerSelector: string, odataQuery: string) => {
            this.viewModel.getAll(odataQuery,(data) => {
                this.viewModel.getSchema(() => {
                    var table = this.htmlStrategy.makeTable(data, this.pageArgs.controllerName);

                    $(containerSelector).find(".gridPanel").html(table);
                    
                    var settings: DataTables.Settings = new ScaffoldDataTableSettings();
                    //settings.dom = 'Bfrtip';
                    settings.buttons = ['copy', 'excel', 'pdf'];
                    $(containerSelector).find("table").DataTable(settings);
                });
            });
        }
    }

    class ScaffoldDataTableSettings implements  DataTables.Settings {
        buttons: string[];
    }
}

