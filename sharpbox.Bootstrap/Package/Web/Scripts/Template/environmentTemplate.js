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
                var html = "<div class=\"container\">\n                        " + viewModel.form.header.toHtml() + "\n                        <div class=\"row\">\n                            <div class=\"col-lg-12\">\n                                " + viewModel.form.fieldsToHtml() + "\n                            </div>\n                        </div>\n                        " + viewModel.form.footer.toHtml(viewModel.form.header.name) + "\n                    </div>";
                return html;
            };
        })(Templating = Web.Templating || (Web.Templating = {}));
    })(Web = sharpbox.Web || (sharpbox.Web = {}));
})(sharpbox || (sharpbox = {}));
;
//# sourceMappingURL=environmentTemplate.js.map