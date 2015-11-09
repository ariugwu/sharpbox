/// <reference path="../sharpbox.poco.d.ts"/>
/// <reference path="../sharpbox.domain.ts"/>
/// <reference path="../sharpbox.Web.ViewModel.ts"/>
/// <reference path="../Typings/collections.d.ts"/>

module sharpbox.Web.Templating {
    export var editForm = (viewModel: sharpbox.Web.ViewModel<any>): string => {
        var html = `
                    <div class="sharpform">
                        <div class="sharpform-header">
                            <div class="row">
                                <div class="col-sm-9">
                                    <p class="pull-left title">${viewModel.instanceName}: Add/Update</p>
                                </div>
                                <div class="col-sm-3">
                                    <div class="btn-group pull-right">
                                      <button type="button" class="btn btn-primary"><i class="glyphicon glyphicon-floppy-disk"></i></button>
                                      <button type="button" class="btn btn-default"><i class="glyphicon glyphicon-question-sign"></i></button>
                                    </div>
                                </div>
                            </div>
                            <hr />
                            <div class="row">
                                <div class="col-sm-10">
                                    <p class="pull-left text-muted">* Some attempt will be made in the future make this header more modular along with the rest of the form. However, the best approach currently is to create your own template if this one does not suite your needs.</p>
                                </div>
                            </div>
                        </div>
                        ${viewModel.form.header.toHtml("")}
                        ${fieldsToHtml(viewModel)}
                        <hr />
                        <input type=\"hidden\" id=\"WebRequest_UiAction\" name=\"WebRequest.UiAction.Name\" value=\"${viewModel.form.uiAction}\" />
                        <input type="submit" value="Save" class="btn btn-primary" style="width: 100%;" />
                        ${viewModel.form.footer.toHtml(viewModel.form.header.name)}
                    </div>
                    `;
        return html;
    };

    export var grid = (schema: any, data: Array<any>): string => {
        var html = `<div class="container">
                        <div class="row">
                            <div class="col-lg-12">
                            </div>
                        </div>
                    </div>`;
        return html;
    }

    export var fieldsToHtml = (viewModel: sharpbox.Web.ViewModel<any>) : string => {
        var fields : Field[] = viewModel.form.fieldDictionaryToArray();
        var htmlStrategy: IHtmlStrategy = viewModel.form.htmlStrategy;
        var html : string = "";
            $.each(fields, (key, field) => {
                    let label = htmlStrategy.labelHtml(field, "");
                    let input = htmlStrategy.inputHtml(field, "");
                    html = html + htmlStrategy.groupHtml(label, input);
            });

        return html;
    };
};