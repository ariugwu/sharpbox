/// <reference path="sharpbox.poco.d.ts"/>
/// <reference path="sharpbox.domain.ts"/>
/// <reference path="Typings/jquery.d.ts"/>
var sharpbox;
(function (sharpbox) {
    (function (Web) {
        var ViewModel = (function () {
            function ViewModel(instanceName, targetContainer, uiAction, formMethod) {
                this.controllerUrl = "/" + instanceName + "/";
                this.targetContainer = targetContainer;
                this.uiAction = uiAction;
                this.formMethod = formMethod;
                this.actionUrl = this.controllerUrl + uiAction;
            }
            ViewModel.prototype.getAll = function () {
                var url = this.controllerUrl + "Get/";
                $.get(url, function (data) {
                    this.collection = data;
                });
            };

            ViewModel.prototype.getById = function (id) {
                var url = this.controllerUrl + "GetBySharpId/" + id;
                $.get(url, function (data) {
                    this.instance = data;
                });
            };

            ViewModel.prototype.getSchema = function () {
                var url = this.controllerUrl + "JsonSchema/";
                $.getJSON(url, function (data) {
                    this.schema = JSON.parse(data);
                });
            };

            ViewModel.prototype.execute = function (action) {
                var url = this.controllerUrl + "Execute/";
                var webRequest = new sharpbox.WebLibrary.Core.WebRequest();

                webRequest = {
                    UiAction: action,
                    Instance: this.instance
                };

                $.post(url, webRequest, function (data) {
                    this.processWebResponse(data);
                }, 'json');
            };

            ViewModel.prototype.processWebResponse = function (webResponse) {
            };

            ViewModel.prototype.bindToForm = function (formName) {
                var form = $(formName);
                for (var i; i < this.instance; i++) {
                }
            };

            ViewModel.prototype.setProperty = function (name, value) {
                this.instance[name] = value;
            };

            ViewModel.prototype.buildForm = function (template) {
                var properties = this.schema.properties;

                this.formHeader = "<form role=\"form\" action=\"" + this.actionUrl + "\" method=\"" + this.formMethod + "\">";

                var html = this.formHeader;

                $.each(properties, function (key, field) {
                    if (field.type == 'array') {
                        console.log("Would create a daughter grid for the array of:" + key);
                        $.each(field.items.properties, function (k1, f1) {
                            console.log(k1);
                        });
                    } else if (field.type == 'object') {
                        console.log("Would create an embedded form for the object:" + key);
                    } else {
                        html = html + this.createInput(key, field);
                        console.log(field);
                    }
                });

                html = html + "<button type=\"submit\" class=\"btn btn-default\">Submit</button>";
                html = html + "</form>";

                $(this.targetContainer).html(html);
            };

            ViewModel.prototype.createInput = function (key, field) {
                var html = "";
                var inputExtraClasses = "";
                var inputAppend = "";
                var inputType = "text";

                if (field.format == "date-time") {
                    inputExtraClasses = "datepicker";
                    inputAppend = "<span class=\"input-group-addon\"><i class=\"glyphicon glyphicon-calendar\"></i></span>";
                }

                this.inputMap[key].labelElement = "<label for=\"" + key + "\">" + key.replace(/([a-z])([A-Z])/g, '$1 $2') + "</label>";
                this.inputMap[key].inputPrepend = "";
                this.inputMap[key].inputElement = "<input type=\"" + inputType + "\" class=\"form-control " + inputExtraClasses + "\" id=\"" + key + "\" />";
                this.inputMap[key].inputAppend = inputAppend;

                html = html + "<div class=\"form-group\">";
                html = html + "<div class=\"col-md-4 text-right input-group\">";
                html = html + this.inputMap[key].labelElement;
                html = html + "</div>";
                html = html + "<div class=\"col-md-8 input-group\">";
                html = html + this.inputMap[key].inputPrepend;
                html = html + this.inputMap[key].inputElement;
                html = html + this.inputMap[key].inputAppend;
                html = html + "</div>";
                html = html + "</div>";

                return html;
            };
            return ViewModel;
        })();
        Web.ViewModel = ViewModel;
    })(sharpbox.Web || (sharpbox.Web = {}));
    var Web = sharpbox.Web;
})(sharpbox || (sharpbox = {}));

var test = new sharpbox.Web.ViewModel("Environment", "#example", "Update", "POST");

test.instance = {
    EnvironmentId: 0,
    ApplicationName: "Sample Application",
    BaseUrl: "http://example.com",
    CacheKey: "any",
    UploadDirectory: "~/Uploads/",
    LogoLocation: "~/Images/Logo",
    BrandTypeId: 1,
    BrandType: null,
    EnvironmentTypeId: 1,
    EnvironmentType: null,
    TechSheetId: 1,
    TechSheet: null,
    SharpId: null
};

var wat = test.instance.BrandTypeId;

var label = test.inputMap[this.instance.ApplicationName].labelElement;

test.execute(sharpbox.Domain.TestController.Command.Update);
//# sourceMappingURL=sharpbox.Web.ViewModel.js.map
