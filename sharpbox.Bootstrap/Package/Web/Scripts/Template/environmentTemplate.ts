/// <reference path="../sharpbox.poco.d.ts"/>
/// <reference path="../sharpbox.domain.ts"/>
/// <reference path="../sharpbox.Web.ViewModel.ts"/>

module sharpbox.Web.Templating {
    export var environmentEditForm = (viewModel: sharpbox.Web.ViewModel<sharpbox.App.Model.Environment>) => {
        viewModel.updateForm = new UpdateForm("Update", viewModel.controllerUrl, "Update", "POST");
        viewModel.updateForm.populateFieldDictionary(viewModel.schema);

        var html = `<div class="container">
                        ${viewModel.updateForm.header.toHtml()}
                        <div class="row">
                            <div class="col-md-6">
                                ${viewModel.updateForm.fieldsToHtml()}
                            </div>
                            <div class="col-md-6">
                                ${viewModel.updateForm.fieldDictionary[viewModel.instance.BaseUrl].toHtml()}
                            </div>
                        </div>
                        ${viewModel.updateForm.footer.toHtml(viewModel.updateForm.header.name)}
                    </div>`;
        return html;
    };

};