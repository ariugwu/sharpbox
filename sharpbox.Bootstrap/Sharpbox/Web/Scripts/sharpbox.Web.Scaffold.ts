/// <reference path="sharpbox.poco.d.ts"/>
/// <reference path="sharpbox.domain.ts"/>
/// <reference path="Typings/jquery.d.ts"/>

module sharpbox.Web {
    export class Scaffold {
        loadEditForm = (containerSelector: string, domainName: string, id: string): void => {
            var detail = new sharpbox.Web.ViewModel<any>(domainName);
            detail.getSchema(() => {
                var htmlStrategy: sharpbox.Web.IHtmlStrategy = new sharpbox.Web.BootstrapHtmlStrategy();
                var formName = "UpdateForm";
                detail.form = new sharpbox.Web.Form(detail.schema, formName, detail.controllerUrl, "Update", "POST", htmlStrategy);
                var template = sharpbox.Web.Templating.editForm(detail);

                $(containerSelector).html(template);

                detail.form.htmlStrategy.wireSubmit(formName);

                if (id != null) {
                    detail.getById(id, () => {
                        detail.form.bindToForm(detail.instance);
                    });
                }
            });
        }

        loadGrid = (containerSelector: string, domainName: string) => {
            $.getJSON(`/${domainName}/Get`).done(data => {
                var table = this.makeTable(data, domainName);
                $(containerSelector).append(table);
            });
        }

        makeTable = (data : any, domainName: string) => {
            var table = $("<table class=\"table table-striped\">");
            var tblHeader = "<tr>";
            var object = data[0];
            for (let k in object) {
                if (object.hasOwnProperty(k)) {
                    tblHeader += `<th>${k}</th>`;
                }
            }
            tblHeader += "</tr>";
            $(tblHeader).appendTo(table);
            $.each(data, (index, value) => {

                var tableRow = "<tr>";
                $.each(value, (key, val) => {
                    if (key == "SharpId") {
                        tableRow += `<td><a href="/${domainName}/Detail/?sharpId=${val}" class="btn btn-sm btn-info">Edit</a></td>`;
                    } else {
                        tableRow += `<td>${val}</td>`;
                    }

                });
                tableRow += "</tr>";
                $(table).append(tableRow);
            });
            return ($(table));
        };
    }
}

