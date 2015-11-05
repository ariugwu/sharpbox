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
        htmlStrategy: IHtmlStrategy;

        constructor(schema: any, name: string, controllerUrl: string, uiAction: string, method: string, htmlStrategy: IHtmlStrategy) {
            this.schema = schema;
            this.header = new Header();
            this.header.name = name;
            this.header.action = controllerUrl + uiAction;
            this.header.method = method;
            this.footer = new Footer();
            this.htmlStrategy = htmlStrategy;

            this.fieldDictionary = new collections.Dictionary<string, Field>();

            this.populateFieldDictionary(htmlStrategy);
        }

        populateFieldDictionary(htmlStrategy: IHtmlStrategy) {
            let properties = this.schema.properties;
            let self = this;

            $.each(properties, (key, field) => {
                if (field.type == 'array') {
                    console.log(`Would create a daughter grid for the array of:${key}`);
                    $.each(field.items.properties, (k1, f1) => {
                        console.log(k1);
                    });
                } else if (field.type == 'object') {
                    var titleField = new Field(key, { dataType: "title", format: "title" });
                    self.insertField(key, titleField);
                    console.log(`Would create an embedded form for the object:${key}`);
                    $.each(field.properties, (k, f) => {
                        self.insertField(k,f);
                    });
                } else {
                    console.debug(`Processing: ${key}: ${field}`);
                    this.insertField(key, field);
                }
            });

            
        }

        insertField(key: string, field: any) {
            this.fieldDictionary.setValue(key, new Field(key, field));
        }

        fieldDictionaryToArray() : Array<Field> {
            let array = new Array<Field>();

            let properties = this.schema.properties;
            $.each(properties, (key, field) => {
                var f = this.fieldDictionary.getValue(key);
                array.push(f);
            });

            return array;
        }
        bindToForm(instance: T) {
            $.each(instance, (key, value) => {
                if (instance.hasOwnProperty(key)) {
                    $(key).html(value);
                }
            });
        }
    }

    export class Header {
        name: string;
        action: string;
        method: string;

        toHtml() {
            return `<form class=\"form-horizontal\" role="form" name="${this.name}" action="${this.action}" method="${this.method}">`;
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
        name: string;
        data: {
            dataType: any;
            format: string;
        }

        constructor(name: string, data: { dataType: any; format: string }) {
            this.name = name;
            this.data = data;
        }
    }

    export interface IHtmlStrategy {
        labelHtml(field: Field, extraClasses: string) : string;
        inputHtml(field: Field, extraClasses: string): string;

        groupHtml(label: string, input: string): string;

        formatInputPrepend(field: Field): string;
        formatInputAppend(field: Field): string;
    }

    export class BaseHtmlStrategy implements IHtmlStrategy {

        labelHtml(field: Field, extraClasses: string): string {
            return `<label for="${field.name}"><small>${field.name.replace(/([a-z])([A-Z])/g, "$1 $2") }</small></label>`;
        }

        inputHtml(field: Field, extraClasses: string): string {
            let inputType = "text";

            switch(field.data.dataType) {
                default:
                    return `<input type="${inputType}" id="${field.name}" />`;
            }
        }

        groupHtml(label: string, input: string) : string {
            return `<div class="form-group">
                        ${label}
                        ${input}
                    </div>
                    `;
        }

        formatInputPrepend(field: Field): string {
            return "";
        }

        formatInputAppend(field: Field): string {
            return "";
        }
    }

    export class BootstrapHtmlStrategy extends BaseHtmlStrategy {
        labelHtml(field: Field, extraClasses: string): string {
            return `<label class=\"${extraClasses}"\" for="${field.name}"><small>${field.name.replace(/([a-z])([A-Z])/g, "$1 $2") }</small></label>`;
        }

        inputHtml(field: Field, extraClasses: string): string {
            let inputType = "text";

            switch (field.data.dataType) {
                default:
                    return `${this.formatInputPrepend(field) }<input type="${inputType}" class="form-control ${extraClasses}" id="${field.name}" />${this.formatInputAppend(field)}`;
            }
        }

        groupHtml(label: string, input: string): string {
            return `<div class="form-group">
                        ${label}
                        ${input}
                    </div>
                    `;
        }

        formatInputPrepend(field: Field): string {
            switch (field.data.format) {
                case "date-time":
                    return "<span class=\"input-group-addon\"><i class=\"glyphicon glyphicon-calendar\"></i></span>";
                default:
                    return "";
            }
        }

        formatInputAppend(field: Field): string {
            switch (field.data.format) {
                case "date-time":
                    return "";
                default:
                    return "";
            }
        }
    }

}