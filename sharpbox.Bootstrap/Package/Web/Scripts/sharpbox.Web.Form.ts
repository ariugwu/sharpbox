/// <reference path="sharpbox.poco.d.ts"/>
/// <reference path="sharpbox.domain.ts"/>
/// <reference path="sharpbox.Web.ViewModel.ts"/>
/// <reference path="Typings/jquery.d.ts"/>

module sharpbox.Web {
    export class Form<T> {
        header: Header;
        fieldDictionary: { [id: string]: Field };
        button: {
            submit: Button;
            reset: Button;
        }
        footer: Footer;

        constructor(name: string, controllerUrl: string, uiAction: string, method: string) {
            this.header = new Header();
            this.header.name = name;
            this.header.action = controllerUrl + uiAction;
            this.header.method = method;
        }

        populateFieldDictionary(schema: any) {
            let properties = schema.properties;

            $.each(properties, (key, field) => {
                if (field.type == 'array') {
                    console.log(`Would create a daughter grid for the array of:${key}`);
                    $.each(field.items.properties, (k1, f1) => {
                        console.log(k1);
                    });
                } else if (field.type == 'object') {
                    console.log(`Would create an embedded form for the object:${key}`);
                } else {
                    this.insertField(key, field);
                }
            });
        }

		insertField(key: string, field: any) {
		    this.fieldDictionary[key] = new Field(key, field);
		}
    }

    export class Header {
        name: string;
        action: string;
        method: string;

        toHtml() {
            return `<form role="form" name="${this.name}" action="${this.action}" method="${this.method}">`;
        }
    }

    export class Button {
        type: string;
        value: string;
    }

    export class Footer {
        toHtml(formName: string) {
            return `</form> <!-- End form: "${formName}"-->`;
        }
    }

    export class InsertForm<T> extends Form<T> {

        constructor(name: string, controllerUrl: string, uiAction: string, method: string) {
            super(name, controllerUrl, uiAction, method);

            this.button.submit = new Button();
            this.button.submit.type = "submit";
            this.button.submit.value = "Insert";
        }
    }

    export class UpdateForm<T> extends InsertForm<T> {

    }

    export class Field {
        labelElement: string;
        inputElement: string;
        inputAppend: string;
        inputPrepend: string;
        targetContainer: string;
        ordinal: number;

        constructor(key: string, field: any) {
            let html = "";
            let inputExtraClasses = "";
            let inputAppend = "";
            let inputType = "text";

            if (field.format == "date-time") {
                inputExtraClasses = "datepicker";
                inputAppend = "<span class=\"input-group-addon\"><i class=\"glyphicon glyphicon-calendar\"></i></span>";
            }

            this.labelElement = `<label for="${key}">${key.replace(/([a-z])([A-Z])/g, "$1 $2") }</label>`;
            this.inputPrepend = "";
            this.inputElement = `<input type="${inputType}" class="form-control ${inputExtraClasses}" id="${key}" />`;
            this.inputAppend = inputAppend;
			
        }

        toHtml() {
            let html = "";
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
        }
    }
}