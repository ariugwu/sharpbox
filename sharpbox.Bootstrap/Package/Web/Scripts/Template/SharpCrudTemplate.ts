/// <reference path="../sharpbox.poco.d.ts"/>
/// <reference path="../sharpbox.domain.ts"/>
/// <reference path="../sharpbox.Web.ViewModel.ts"/>
/// <reference path="../Typings/collections.d.ts"/>

module sharpbox.Web.Templating {
    export var editForm = (viewModel: sharpbox.Web.ViewModel<any>) => {
        var html = `<div class="container">
                        ${viewModel.form.header.toHtml()}
                        ${viewModel.form.fieldsToHtml()}
                        ${viewModel.form.footer.toHtml(viewModel.form.header.name)}
                    </div>`;
        return html;
    };

    export var grid = (schema: any, data: Array<any>) =>
    {
        var html = `<div class="container">
                        <div class="row">
                            <div class="col-lg-12">
                            </div>
                        </div>
                    </div>`;
        return html;
    }
};