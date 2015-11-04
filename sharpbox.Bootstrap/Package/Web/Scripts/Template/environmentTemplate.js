/// <reference path="../sharpbox.poco.d.ts"/>
/// <reference path="../sharpbox.domain.ts"/>
/// <reference path="../sharpbox.Web.ViewModel.ts"/>
var sharpbox;
(function (sharpbox) {
    var Web;
    (function (Web) {
        var Templating;
        (function (Templating) {
            Templating.environmentEditForm = function (viewModel) {
                viewModel.updateForm = new Web.UpdateForm("Update", viewModel.controllerUrl, "Update", "POST");
                viewModel.updateForm.populateFieldDictionary(viewModel.schema);
                var html = "<div class=\"container\">\n                        " + viewModel.updateForm.header.toHtml() + "\n                        <div class=\"row\">\n                            <div class=\"col-md-6\">\n                                " + viewModel.updateForm.fieldsToHtml() + "\n                            </div>\n                            <div class=\"col-md-6\">\n                                " + viewModel.updateForm.fieldDictionary[viewModel.instance.BaseUrl].toHtml() + "\n                            </div>\n                        </div>\n                        " + viewModel.updateForm.footer.toHtml(viewModel.updateForm.header.name) + "\n                    </div>";
                return html;
            };
        })(Templating = Web.Templating || (Web.Templating = {}));
    })(Web = sharpbox.Web || (sharpbox.Web = {}));
})(sharpbox || (sharpbox = {}));
;
