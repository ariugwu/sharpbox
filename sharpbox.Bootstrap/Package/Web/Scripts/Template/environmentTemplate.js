/// <reference path="../sharpbox.poco.d.ts"/>
/// <reference path="../sharpbox.domain.ts"/>
/// <reference path="../sharpbox.Web.ViewModel.ts"/>
var sharpbox;
(function (sharpbox) {
    var Web;
    (function (Web) {
        var Templating;
        (function (Templating) {
            Templating.generateFormGroup = function (viewModel) {
                var html = "";
                for (var f in viewModel.updateForm.fieldDictionary) {
                    if (viewModel.updateForm.fieldDictionary.hasOwnProperty(f)) {
                        html = html + f.toHtml();
                    }
                }
                return html;
            };
            Templating.environmentEditForm = function (viewModel) {
                viewModel.updateForm = new Web.UpdateForm("Update", viewModel.controllerUrl, "Update", "POST");
                viewModel.updateForm.populateFieldDictionary(viewModel.schema);
                var html = "<div class=\"container\">\n                        " + viewModel.updateForm.header.toHtml() + "\n                        <div class=\"row\">\n                            <div class=\"col-md-6\">\n                                " + Templating.generateFormGroup(viewModel) + "\n                            </div>\n                            <div class=\"col-md-6\"></div>\n                        </div>\n                        " + viewModel.updateForm.footer.toHtml(viewModel.updateForm.header.name) + "\n                    </div>";
                return html;
            };
        })(Templating = Web.Templating || (Web.Templating = {}));
    })(Web = sharpbox.Web || (sharpbox.Web = {}));
})(sharpbox || (sharpbox = {}));
;
