/// <reference path="../sharpbox.poco.d.ts"/>
/// <reference path="../sharpbox.domain.ts"/>
/// <reference path="../sharpbox.Web.ViewModel.ts"/>
/// <reference path="../Typings/collections.d.ts"/>
var sharpbox;
(function (sharpbox) {
    var Web;
    (function (Web) {
        var Templating;
        (function (Templating) {
            Templating.editForm = function (viewModel) {
                var html = "<div class=\"container\">\n                        " + viewModel.form.header.toHtml() + "\n                        " + Templating.fieldsToHtml(viewModel) + "\n                        <input type=\"hidden\" id=\"WebRequest_UiAction\" name=\"WebRequest.UiAction.Name\" value=\"" + viewModel.form.uiAction + "\" />\n                        <input type=\"submit\" value=\"Update\" />\n                        " + viewModel.form.footer.toHtml(viewModel.form.header.name) + "\n                    </div>";
                return html;
            };
            Templating.grid = function (schema, data) {
                var html = "<div class=\"container\">\n                        <div class=\"row\">\n                            <div class=\"col-lg-12\">\n                            </div>\n                        </div>\n                    </div>";
                return html;
            };
            Templating.fieldsToHtml = function (viewModel) {
                var fields = viewModel.form.fieldDictionaryToArray();
                var htmlStrategy = viewModel.form.htmlStrategy;
                var html = "";
                $.each(fields, function (key, field) {
                    var label = htmlStrategy.labelHtml(field, "");
                    var input = htmlStrategy.inputHtml(field, "");
                    html = html + htmlStrategy.groupHtml(label, input);
                });
                return html;
            };
        })(Templating = Web.Templating || (Web.Templating = {}));
    })(Web = sharpbox.Web || (sharpbox.Web = {}));
})(sharpbox || (sharpbox = {}));
;
//# sourceMappingURL=SharpCrudTemplate.js.map