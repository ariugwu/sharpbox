/// <reference path="../sharpbox.poco.d.ts"/>
/// <reference path="../sharpbox.domain.ts"/>
/// <reference path="../sharpbox.Web.ViewModel.ts"/>

module sharpbox.Web.Templating {
    export var generateFormGroup = (viewModel: sharpbox.Web.ViewModel<sharpbox.App.Model.Environment>) => {
        var html = "";
        for (let f in viewModel.updateForm.fieldDictionary) {
            if (viewModel.updateForm.fieldDictionary.hasOwnProperty(f)) {
                html = html + f.toHtml();
            }
        }

        return html;
    }

    export var environmentEditForm = (viewModel: sharpbox.Web.ViewModel<sharpbox.App.Model.Environment>) => {
        viewModel.updateForm = new UpdateForm("Update", viewModel.controllerUrl, "Update", "POST");
        viewModel.updateForm.populateFieldDictionary(viewModel.schema);

        var html = `<div class="container">
                        ${viewModel.updateForm.header.toHtml()}
                        <div class="row">
                            <div class="col-md-6">
                                ${generateFormGroup(viewModel) }
                            </div>
                            <div class="col-md-6"></div>
                        </div>
                        ${viewModel.updateForm.footer.toHtml(viewModel.updateForm.header.name)}
                    </div>`;
        return html;
    };

};