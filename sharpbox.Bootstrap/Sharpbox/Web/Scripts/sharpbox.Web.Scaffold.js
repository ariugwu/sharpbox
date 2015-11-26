/// <reference path="sharpbox.poco.d.ts"/>
/// <reference path="sharpbox.domain.ts"/>
/// <reference path="Typings/jquery.d.ts"/>
var sharpbox;
(function (sharpbox) {
    var Web;
    (function (Web) {
        var Scaffold = (function () {
            function Scaffold() {
                var _this = this;
                this.loadEditForm = function (containerSelector, domainName, id) {
                    var detail = new sharpbox.Web.ViewModel(domainName);
                    detail.getSchema(function () {
                        var htmlStrategy = new sharpbox.Web.BootstrapHtmlStrategy();
                        var formName = "UpdateForm";
                        detail.form = new sharpbox.Web.Form(detail.schema, formName, detail.controllerUrl, "Update", "POST", htmlStrategy);
                        var template = sharpbox.Web.Templating.editForm(detail);
                        $(containerSelector).html(template);
                        detail.form.htmlStrategy.wireSubmit(formName);
                        if (id != null) {
                            detail.getById(id, function () {
                                detail.form.bindToForm(detail.instance);
                            });
                        }
                    });
                };
                this.loadGrid = function (containerSelector, domainName) {
                    $.getJSON("/" + domainName + "/Get", { _: new Date().getTime() }).done(function (data) {
                        var table = _this.makeTable(data, domainName);
                        $(containerSelector).append(table);
                    });
                };
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
            return Scaffold;
        })();
        Web.Scaffold = Scaffold;
    })(Web = sharpbox.Web || (sharpbox.Web = {}));
})(sharpbox || (sharpbox = {}));
