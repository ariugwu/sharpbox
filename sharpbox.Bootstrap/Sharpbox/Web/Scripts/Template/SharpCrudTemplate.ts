/// <reference path="../sharpbox.poco.d.ts"/>
/// <reference path="../sharpbox.domain.ts"/>
/// <reference path="../sharpbox.Web.ViewModel.ts"/>
/// <reference path="../Typings/collections.d.ts"/>

module sharpbox.Web.Templating {
    export var editForm = (viewModel: sharpbox.Web.ViewModel<any>): string => {
        var html = `<div class="container">
                        ${viewModel.form.header.toHtml()}
                        ${fieldsToHtml(viewModel)}
                        <input type=\"hidden\" id=\"WebRequest_UiAction\" name=\"WebRequest.UiAction.Name\" value=\"${viewModel.form.uiAction}\" />
                        <input type="submit" value="Update" />
                        ${viewModel.form.footer.toHtml(viewModel.form.header.name)}
                    </div>`;
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