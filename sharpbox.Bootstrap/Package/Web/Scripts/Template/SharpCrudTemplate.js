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
                var html = "<div class=\"container\">\n                        " + viewModel.form.header.toHtml() + "\n                        " + viewModel.form.fieldsToHtml() + "\n                        " + viewModel.form.footer.toHtml(viewModel.form.header.name) + "\n                    </div>";
                return html;
            };
            Templating.grid = function (schema, data) {
                var html = "<div class=\"container\">\n                        <div class=\"row\">\n                            <div class=\"col-lg-12\">\n                            </div>\n                        </div>\n                    </div>";
                return html;
            };
        })(Templating = Web.Templating || (Web.Templating = {}));
    })(Web = sharpbox.Web || (sharpbox.Web = {}));
})(sharpbox || (sharpbox = {}));
;
