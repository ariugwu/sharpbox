/// <reference path="sharpbox.poco.d.ts"/>
/// <reference path="sharpbox.domain.ts"/>
/// <reference path="sharpbox.Web.ViewModel.ts"/>
/// <reference path="Typings/jquery.d.ts"/>
var __extends = (this && this.__extends) || function (d, b) {
    for (var p in b) if (b.hasOwnProperty(p)) d[p] = b[p];
    function __() { this.constructor = d; }
    d.prototype = b === null ? Object.create(b) : (__.prototype = b.prototype, new __());
};
var sharpbox;
(function (sharpbox) {
    var Web;
    (function (Web) {
        var Form = (function () {
            function Form(name, controllerUrl, uiAction, method) {
                this.header = new Header();
                this.header.name = name;
                this.header.action = controllerUrl + uiAction;
                this.header.method = method;
            }
            Form.prototype.populateFieldDictionary = function (schema) {
                var _this = this;
                var properties = schema.properties;
                $.each(properties, function (key, field) {
                    if (field.type == 'array') {
                        console.log("Would create a daughter grid for the array of:" + key);
                        $.each(field.items.properties, function (k1, f1) {
                            console.log(k1);
                        });
                    }
                    else if (field.type == 'object') {
                        console.log("Would create an embedded form for the object:" + key);
                    }
                    else {
                        _this.insertField(key, field);
                    }
                });
            };
            Form.prototype.insertField = function (key, field) {
                this.fieldDictionary[key] = new Field(key, field);
            };
            return Form;
        })();
        Web.Form = Form;
        var Header = (function () {
            function Header() {
            }
            Header.prototype.toHtml = function () {
                return "<form role=\"form\" name=\"" + this.name + "\" action=\"" + this.action + "\" method=\"" + this.method + "\">";
            };
            return Header;
        })();
        Web.Header = Header;
        var Button = (function () {
            function Button() {
            }
            return Button;
        })();
        Web.Button = Button;
        var Footer = (function () {
            function Footer() {
            }
            Footer.prototype.toHtml = function (formName) {
                return "</form> <!-- End form: \"" + formName + "\"-->";
            };
            return Footer;
        })();
        Web.Footer = Footer;
        var InsertForm = (function (_super) {
            __extends(InsertForm, _super);
            function InsertForm(name, controllerUrl, uiAction, method) {
                _super.call(this, name, controllerUrl, uiAction, method);
                this.button.submit = new Button();
                this.button.submit.type = "submit";
                this.button.submit.value = "Insert";
            }
            return InsertForm;
        })(Form);
        Web.InsertForm = InsertForm;
        var UpdateForm = (function (_super) {
            __extends(UpdateForm, _super);
            function UpdateForm() {
                _super.apply(this, arguments);
            }
            return UpdateForm;
        })(InsertForm);
        Web.UpdateForm = UpdateForm;
        var Field = (function () {
            function Field(key, field) {
                var html = "";
                var inputExtraClasses = "";
                var inputAppend = "";
                var inputType = "text";
                if (field.format == "date-time") {
                    inputExtraClasses = "datepicker";
                    inputAppend = "<span class=\"input-group-addon\"><i class=\"glyphicon glyphicon-calendar\"></i></span>";
                }
                this.labelElement = "<label for=\"" + key + "\">" + key.replace(/([a-z])([A-Z])/g, "$1 $2") + "</label>";
                this.inputPrepend = "";
                this.inputElement = "<input type=\"" + inputType + "\" class=\"form-control " + inputExtraClasses + "\" id=\"" + key + "\" />";
                this.inputAppend = inputAppend;
            }
            Field.prototype.toHtml = function () {
                var html = "";
                html = html + "<div class=\"form-group\">";
                html = html + "<div class=\"col-md-4 text-right input-group\">";
                html = html + this.labelElement;
                html = html + "</div>";
                html = html + "<div class=\"col-md-8 input-group\">";
                html = html + this.inputPrepend;
                html = html + this.inputElement;
                html = html + this.inputAppend;
                html = html + "</div>";
                html = html + "</div>";
                return html;
            };
            return Field;
        })();
        Web.Field = Field;
    })(Web = sharpbox.Web || (sharpbox.Web = {}));
})(sharpbox || (sharpbox = {}));
