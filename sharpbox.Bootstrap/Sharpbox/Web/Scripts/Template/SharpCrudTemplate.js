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
                var html = "\n                    <div class=\"sharpform\">\n                        <div class=\"sharpform-header\">\n                            <div class=\"row\">\n                                <div class=\"col-sm-9\">\n                                    <p class=\"pull-left title\">" + viewModel.instanceName + ": Add/Update</p>\n                                </div>\n                                <div class=\"col-sm-3\">\n                                    <div class=\"btn-group pull-right\">\n                                      <button type=\"button\" onclick=\"$('form[name=" + viewModel.form.header.name + "]').triggerHandler('submit')\" class=\"btn btn-primary\"><i class=\"glyphicon glyphicon-floppy-disk\"></i></button>\n                                      <button type=\"button\" class=\"btn btn-default\"><i class=\"glyphicon glyphicon-question-sign\"></i></button>\n                                    </div>\n                                </div>\n                            </div>\n                            <hr />\n                            <div class=\"row\">\n                                <div class=\"col-sm-10\">\n                                    <p class=\"pull-left text-muted\">* Some attempt will be made in the future make this header more modular along with the rest of the form. However, the best approach currently is to create your own template if this one does not suite your needs.</p>\n                                </div>\n                            </div>\n                        </div>\n                        " + viewModel.form.header.toHtml("") + "\n                        " + Templating.fieldsToHtml(viewModel) + "\n                        <hr />\n                        <p><a href=\"" + viewModel.controllerUrl + "\">View all " + viewModel.instanceName + "(s)</a></p>\n                        <p class=\"text-muted bold\"><small>*This form created dynamically based on assumptions about your model.</small></p>\n                        <input type=\"hidden\" id=\"WebRequest_UiAction\" name=\"WebRequest.UiAction.Name\" value=\"" + viewModel.form.uiAction + "\" />\n                        " + viewModel.form.footer.toHtml(viewModel.form.header.name) + "\n                    </div>\n                    ";
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
