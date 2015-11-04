/// <reference path="sharpbox.poco.d.ts"/>
/// <reference path="sharpbox.domain.ts"/>
/// <reference path="sharpbox.Web.ViewModel.ts"/>
/// <reference path="Typings/collections.d.ts"/>
/// <reference path="Typings/jquery.d.ts"/>
var sharpbox;
(function (sharpbox) {
    var Web;
    (function (Web) {
        var Form = (function () {
            function Form(schema, name, controllerUrl, uiAction, method) {
                this.schema = schema;
                this.header = new Header();
                this.header.name = name;
                this.header.action = controllerUrl + uiAction;
                this.header.method = method;
                this.footer = new Footer();
                this.fieldDictionary = new collections.Dictionary();
                this.populateFieldDictionary();
            }
            Form.prototype.populateFieldDictionary = function () {
                var _this = this;
                var properties = this.schema.properties;
                var self = this;
                $.each(properties, function (key, field) {
                    if (field.type == 'array') {
                        console.log("Would create a daughter grid for the array of:" + key);
                        $.each(field.items.properties, function (k1, f1) {
                            console.log(k1);
                        });
                    }
                    else if (field.type == 'object') {
                        var titleField = new Field(key, { type: "title", format: "title" });
                        self.insertField(key, titleField);
                        console.log("Would create an embedded form for the object:" + key);
                        $.each(field.properties, function (k, f) {
                            self.insertField(k, f);
                        });
                    }
                    else {
                        console.debug("Processing: " + key + ": " + field);
                        _this.insertField(key, field);
                    }
                });
            };
            Form.prototype.insertField = function (key, field) {
                this.fieldDictionary.setValue(key, new Field(key, field));
            };
            Form.prototype.fieldsToHtml = function () {
                var _this = this;
                var properties = this.schema.properties;
                var html = "";
                $.each(properties, function (key, field) {
                    var f = _this.fieldDictionary.getValue(key);
                    var fieldHtml = f.toHtml();
                    html = html + f.toHtml();
                });
                return html;
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
        var Field = (function () {
            function Field(key, field) {
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
//# sourceMappingURL=sharpbox.Web.Form.js.map