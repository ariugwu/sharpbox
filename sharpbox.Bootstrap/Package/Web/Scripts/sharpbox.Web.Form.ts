/// <reference path="sharpbox.poco.d.ts"/>
/// <reference path="sharpbox.domain.ts"/>
/// <reference path="sharpbox.Web.ViewModel.ts"/>
/// <reference path="Typings/collections.d.ts"/>

/// <reference path="Typings/jquery.d.ts"/>

module sharpbox.Web {
    export class Form<T> {
        header: Header;
        fieldDictionary: collections.Dictionary<string, Field>;
        button: {
            submit: Button;
            reset: Button;
        }
        footer: Footer;
        schema: any;

        constructor(schema: any,name: string, controllerUrl: string, uiAction: string, method: string) {
            this.schema = schema;
            this.header = new Header();
            this.header.name = name;
            this.header.action = controllerUrl + uiAction;
            this.header.method = method;
            this.footer = new Footer();

            this.fieldDictionary = new collections.Dictionary<string, Field>();
            this.populateFieldDictionary();
        }

        populateFieldDictionary() {
            let properties = this.schema.properties;
            let self = this;

            $.each(properties, (key, field) => {
                if (field.type == 'array') {
                    console.log(`Would create a daughter grid for the array of:${key}`);
                    $.each(field.items.properties, (k1, f1) => {
                        console.log(k1);
                    });
                } else if (field.type == 'object') {
                    var titleField = new Field(key, { type: "title", format: "title" });
                    self.insertField(key, titleField);
                    console.log(`Would create an embedded form for the object:${key}`);
                    $.each(field.properties, (k, f) => {
                        self.insertField(k,f);
                    });
                } else {
                    console.debug("Processing: " + key + ": "+ field);
                    this.insertField(key, field);
                }
            });

            
        }

		insertField(key: string, field: any) {
            this.fieldDictionary.setValue(key, new Field(key, field));
        }

        fieldsToHtml() {
            let properties = this.schema.properties;
            var html = "";
            $.each(properties, (key, field) => {
                var f = this.fieldDictionary.getValue(key);
                var fieldHtml = f.toHtml();
                html = html + f.toHtml();
            });
            return html;
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

    export class Field {
        labelElement: string;
        inputElement: string;
        inputAppend: string;
        inputPrepend: string;
        targetContainer: string;
        ordinal: number;

        constructor(key: string, field: any) {
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