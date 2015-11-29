/// <reference path="sharpbox.poco.d.ts"/>
/// <reference path="sharpbox.domain.ts"/>
/// <reference path="Typings/jquery.d.ts"/>

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

                this.viewModel.form.htmlStrategy.wireSubmit(formName);

                if (this.pageArgs.id != null) {
                    this.viewModel.getById(this.pageArgs.id, () => {
                        this.viewModel.form.bindToForm(this.viewModel.instance);
                    });
                }
            });
        }

        loadGrid = (containerSelector: string) => {
            this.viewModel.getAll((data) => {
                var table = this.htmlStrategy.makeTable(data, this.pageArgs.controllerName);
                $(containerSelector).append(table);
            });
        }
    }
}

