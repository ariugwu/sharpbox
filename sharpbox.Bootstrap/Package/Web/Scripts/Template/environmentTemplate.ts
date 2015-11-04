/// <reference path="../sharpbox.poco.d.ts"/>
/// <reference path="../sharpbox.domain.ts"/>
/// <reference path="../sharpbox.Web.ViewModel.ts"/>

module sharpbox.Web.Templating {
    export var environmentEditForm = (viewModel: sharpbox.Web.ViewModel<sharpbox.App.Model.Environment>) => {
        var html = `<div class="container">
                        ${viewModel.form.header.toHtml()}
                        <div class="row">
                            <div class="col-lg-12">
                                ${viewModel.form.fieldsToHtml()}
                            </div>
                        </div>
                        ${viewModel.form.footer.toHtml(viewModel.form.header.name)}
                    </div>`;
        return html;
    };

};