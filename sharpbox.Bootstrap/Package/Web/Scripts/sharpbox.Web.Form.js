/// <reference path="sharpbox.poco.d.ts"/>
/// <reference path="sharpbox.domain.ts"/>
/// <reference path="sharpbox.Web.ViewModel.ts"/>
/// <reference path="Typings/collections.d.ts"/>
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
            function Form(schema, name, controllerUrl, uiAction, method, htmlStrategy) {
                this.schema = schema;
                this.header = new Header();
                this.header.name = name;
                this.header.action = controllerUrl + uiAction;
                this.header.method = method;
                this.footer = new Footer();
                this.htmlStrategy = htmlStrategy;
                this.fieldDictionary = new collections.Dictionary();
                this.populateFieldDictionary();
            }
            // Takes all the properties and fields of from the schema and creates a dictionary
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
                        var titleField = new Field(key, { dataType: "title", format: "title" });
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
            Form.prototype.fieldDictionaryToArray = function () {
                var _this = this;
                var array = new Array();
                var properties = this.schema.properties;
                $.each(properties, function (key, field) {
                    var f = _this.fieldDictionary.getValue(key);
                    array.push(f);
                });
                return array;
            };
            Form.prototype.bindToForm = function (instance) {
                var _this = this;
                $.each(instance, function (key, value) {
                    if (instance.hasOwnProperty(key)) {
                        $(_this.prefixFieldName(key)).html(value);
                    }
                });
            };
            Form.prototype.prefixFieldName = function (key) {
                return "WebRequest.Intance." + key;
            };
            return Form;
        })();
        Web.Form = Form;
        var Header = (function () {
            function Header() {
            }
            Header.prototype.toHtml = function () {
                return "<form class=\"form-horizontal\" role=\"form\" name=\"" + this.name + "\" action=\"" + this.action + "\" method=\"" + this.method + "\">";
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
            function Field(name, data) {
                this.name = name;
                this.data = data;
            }
            return Field;
        })();
        Web.Field = Field;
        var BaseHtmlStrategy = (function () {
            function BaseHtmlStrategy() {
            }
            BaseHtmlStrategy.prototype.labelHtml = function (field, extraClasses) {
                return "<label for=\"" + field.name + "\"><small>" + field.name.replace(/([a-z])([A-Z])/g, "$1 $2") + "</small></label>";
            };
            BaseHtmlStrategy.prototype.inputHtml = function (field, extraClasses) {
                var inputType = "text";
                switch (field.data.dataType) {
                    default:
                        return "<input type=\"" + inputType + "\" id=\"" + field.name + "\" />";
                }
            };
            BaseHtmlStrategy.prototype.groupHtml = function (label, input) {
                return "<div class=\"form-group\">\n                        " + label + "\n                        " + input + "\n                    </div>\n                    ";
            };
            BaseHtmlStrategy.prototype.formatInputPrepend = function (field) {
                return "";
            };
            BaseHtmlStrategy.prototype.formatInputAppend = function (field) {
                return "";
            };
            return BaseHtmlStrategy;
        })();
        Web.BaseHtmlStrategy = BaseHtmlStrategy;
        var BootstrapHtmlStrategy = (function (_super) {
            __extends(BootstrapHtmlStrategy, _super);
            function BootstrapHtmlStrategy() {
                _super.apply(this, arguments);
            }
            BootstrapHtmlStrategy.prototype.labelHtml = function (field, extraClasses) {
                return "<label class=\"" + extraClasses + "\"\" for=\"" + field.name + "\"><small>" + field.name.replace(/([a-z])([A-Z])/g, "$1 $2") + "</small></label>";
            };
            BootstrapHtmlStrategy.prototype.inputHtml = function (field, extraClasses) {
                var inputType = "text";
                switch (field.data.dataType) {
                    default:
                        return this.formatInputPrepend(field) + "<input type=\"" + inputType + "\" class=\"form-control " + extraClasses + "\" id=\"" + field.name + "\" />" + this.formatInputAppend(field);
                }
            };
            BootstrapHtmlStrategy.prototype.groupHtml = function (label, input) {
                return "<div class=\"form-group\">\n                        " + label + "\n                        " + input + "\n                    </div>\n                    ";
            };
            BootstrapHtmlStrategy.prototype.formatInputPrepend = function (field) {
                switch (field.data.format) {
                    case "date-time":
                        return "<span class=\"input-group-addon\"><i class=\"glyphicon glyphicon-calendar\"></i></span>";
                    default:
                        return "";
                }
            };
            BootstrapHtmlStrategy.prototype.formatInputAppend = function (field) {
                switch (field.data.format) {
                    case "date-time":
                        return "";
                    default:
                        return "";
                }
            };
            return BootstrapHtmlStrategy;
        })(BaseHtmlStrategy);
        Web.BootstrapHtmlStrategy = BootstrapHtmlStrategy;
    })(Web = sharpbox.Web || (sharpbox.Web = {}));
})(sharpbox || (sharpbox = {}));
//# sourceMappingURL=sharpbox.Web.Form.js.map