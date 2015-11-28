/// <reference path="sharpbox.poco.d.ts"/>
/// <reference path="sharpbox.domain.ts"/>
/// <reference path="sharpbox.Web.ViewModel.ts"/>
/// <reference path="Typings/collections.d.ts"/>
/// <reference path="Typings/jquery.d.ts"/>
/// <reference path="Typings/toastr.d.ts"/>
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
                this.controllerUrl = controllerUrl;
                this.header.action = controllerUrl + "Execute";
                this.uiAction = uiAction;
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
                    if (key == "PLACEHOLDER") {
                    }
                    else if (field.type == 'array') {
                        console.log("TODO: Would create a daughter grid for the array of:" + key);
                        $.each(field.items.properties, function (k1, f1) {
                            console.log(k1);
                        });
                    }
                    else if (field.type == 'object') {
                        var titleField = new Field(key, { dataType: "title", format: "title" });
                        self.insertField(key, titleField);
                        console.log("TODO: Would create an embedded form for the object:" + key);
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
                this.fieldDictionary.setValue(key, new Field(key, { dataType: field.type, format: field.format }));
            };
            Form.prototype.fieldDictionaryToArray = function () {
                var _this = this;
                var array = new Array();
                var properties = this.schema.properties;
                $.each(properties, function (key, field) {
                    var f = _this.fieldDictionary.getValue(key);
                    if (f != null)
                        array.push(f);
                });
                return array;
            };
            // Try to bind the instance to the form we target with the 'name' property in our constructor
            Form.prototype.bindToForm = function (instance) {
                var _this = this;
                $.each(instance, function (key, value) {
                    if (instance.hasOwnProperty(key)) {
                        var field = _this.fieldDictionary.getValue(key);
                        if (field != null && field.data != null && field.data.format == "date-time") {
                            var date = new Date(parseInt(value.substr(6)));
                            var d = date.getDate();
                            var m = date.getMonth() + 1;
                            var y = date.getFullYear();
                            value = d + "/" + m + "/" + y;
                        }
                        var inputName = "[name=\"" + _this.prefixFieldName(key) + "\"]";
                        $(inputName).val(value);
                    }
                });
            };
            //Used in the bindToForm method to populate a form so the Scaffold controller can bind it
            Form.prototype.prefixFieldName = function (key) {
                return "WebRequest.Instance." + key;
            };
            return Form;
        })();
        Web.Form = Form;
        var Header = (function () {
            function Header() {
            }
            Header.prototype.toHtml = function (extraClasses) {
                return "<form class=\"form-horizontal " + extraClasses + "\" role=\"form\" name=\"" + this.name + "\" action=\"" + this.action + "\" method=\"" + this.method + "\">";
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
                this.makeTable = function (data, domainName) {
                    var msg = "Error: No table generation code for Base html strategy. See sharpbox.Web.Form.ts line 254(ish).";
                    console.log(msg);
                    alert(msg);
                };
            }
            BaseHtmlStrategy.prototype.labelHtml = function (field, extraClasses) {
                return "<label for=\"" + field.name + "\"><small>" + field.name.replace(/([a-z])([A-Z])/g, "$1 $2") + "</small></label>";
            };
            BaseHtmlStrategy.prototype.inputHtml = function (field, extraClasses) {
                var inputType = "text";
                switch (field.data.dataType) {
                    default:
                        return "<input type=\"" + inputType + "\" id=\"" + field.name + "\" name=\"WebRequest.Instance." + field.name + "\" />";
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
            BaseHtmlStrategy.prototype.wireSubmit = function (formName) {
                var _this = this;
                var form = $("[name=\"" + formName + "\"]");
                form.submit(function (event) {
                    event.preventDefault();
                    _this.submitForm(formName);
                });
            };
            BaseHtmlStrategy.prototype.submitForm = function (formName) {
                var form = $("[name=\"" + formName + "\"]");
                $.ajax({
                    type: "POST",
                    url: form.attr('action'),
                    data: form.serialize(),
                    success: function (response) {
                        console.log(response);
                        response = JSON.parse(response);
                        var responseType = response.ResponseType.toLowerCase();
                        //BEGIN: Error Processing
                        //TODO: Refactor this
                        if (responseType == "error") {
                            var auth = (typeof response.LifeCycleTrail.Auth != 'undefined') ?
                                response.LifeCycleTrail.Auth[0].Item2 : "";
                            var loadContext = (typeof response.LifeCycleTrail.LoadContext != 'undefined') ?
                                response.LifeCycleTrail.LoadContext[0].Item2 : "";
                            var validation = (typeof response.LifeCycleTrail.Validation != 'undefined') ?
                                response.LifeCycleTrail.Validation[0].Item2 : "";
                            var execution = (typeof response.LifeCycleTrail.Execution != 'undefined') ?
                                response.LifeCycleTrail.Execution[0].Item2 : "";
                            var toastTitle = "Unspecified error: showing whole stack";
                            var toastMsg = response.LifeCycleTrail.toString();
                            if (auth != "") {
                                toastTitle = "Error in Authorization";
                                toastMsg = auth;
                            }
                            if (loadContext != "") {
                                toastTitle = "Error in Loading Context";
                                toastMsg = loadContext;
                            }
                            if (validation != "") {
                                toastTitle = "Error in Validation";
                                toastMsg = validation;
                                console.log(toastMsg);
                            }
                            if (execution != "") {
                                toastTitle = "Error in Execution";
                                toastMsg = execution;
                            }
                            //END: Validation
                            toastr[responseType](toastMsg, toastTitle);
                        }
                        else {
                            toastr[responseType](null, response.Message);
                        }
                    }
                });
            };
            return BaseHtmlStrategy;
        })();
        Web.BaseHtmlStrategy = BaseHtmlStrategy;
        var BootstrapHtmlStrategy = (function (_super) {
            __extends(BootstrapHtmlStrategy, _super);
            function BootstrapHtmlStrategy() {
                _super.apply(this, arguments);
                this.makeTable = function (data, domainName) {
                    var table = $("<table class=\"table table-striped\">");
                    var caption = $("<caption><div class=\"btn-group pull-right\"><a href=\"/" + domainName + "/Detail\">Add</a></div></caption>");
                    $(caption).appendTo(table);
                    var tblHeader = "<tr>";
                    var object = data[0];
                    for (var k in object) {
                        if (object.hasOwnProperty(k)) {
                            if (k == domainName + "Id") {
                                tblHeader += "<th>Action(s)</th>";
                            }
                            else {
                                tblHeader += "<th>" + k + "</th>";
                            }
                        }
                    }
                    tblHeader += "</tr>";
                    $(tblHeader).appendTo(table);
                    $.each(data, function (index, value) {
                        var tableRow = "<tr>";
                        $.each(value, function (key, val) {
                            if (key == domainName + "Id") {
                                tableRow += "<td><a href=\"/" + domainName + "/Detail/?id=" + val + "\" class=\"btn btn-sm btn-info\">Edit</a></td>";
                            }
                            else {
                                tableRow += "<td>" + val + "</td>";
                            }
                        });
                        tableRow += "</tr>";
                        $(table).append(tableRow);
                    });
                    return ($(table));
                };
            }
            BootstrapHtmlStrategy.prototype.labelHtml = function (field, extraClasses) {
                return "<label class=\"control-label col-sm-2 " + extraClasses + "\" for=\"" + field.name + "\"><small>" + field.name.replace(/([a-z])([A-Z])/g, "$1 $2") + "</small></label>";
            };
            BootstrapHtmlStrategy.prototype.inputHtml = function (field, extraClasses) {
                var inputType = "text";
                switch (field.data.format) {
                    case "date-time":
                        return "\n                            <div class=\"col-sm-10\">\n                                <div class=\"input-group\">\n                                    " + this.formatInputPrepend(field) + "\n                                    <input type=\"" + inputType + "\" class=\"form-control " + extraClasses + "\" id=\"" + field.name + "\" name=\"WebRequest.Instance." + field.name + "\" />\n                                    " + this.formatInputAppend(field) + "\n                                </div>\n                            </div>\n                            ";
                    default:
                        return "<div class=\"col-sm-10\">\n                                " + this.formatInputPrepend(field) + "<input type=\"" + inputType + "\" class=\"form-control " + extraClasses + "\" id=\"" + field.name + "\" name=\"WebRequest.Instance." + field.name + "\" />" + this.formatInputAppend(field) + "\n                            </div>\n                            ";
                }
            };
            BootstrapHtmlStrategy.prototype.groupHtml = function (label, input) {
                return "<div class=\"form-group\">\n                        " + label + "\n                        " + input + "\n                    </div>\n                    ";
            };
            BootstrapHtmlStrategy.prototype.formatInputPrepend = function (field) {
                switch (field.data.format) {
                    case "date-time":
                        return "";
                    default:
                        return "";
                }
            };
            BootstrapHtmlStrategy.prototype.formatInputAppend = function (field) {
                switch (field.data.format) {
                    case "date-time":
                        return "<span class=\"input-group-addon\"><i class=\"glyphicon glyphicon-calendar\"></i></span>";
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