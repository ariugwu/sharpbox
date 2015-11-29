﻿/// <reference path="sharpbox.poco.d.ts"/>
/// <reference path="sharpbox.domain.ts"/>
/// <reference path="sharpbox.Web.ViewModel.ts"/>
/// <reference path="Typings/collections.d.ts"/>
/// <reference path="Typings/jquery.d.ts"/>
/// <reference path="Typings/toastr.d.ts"/>

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
        instanceName: string;
        controllerUrl: string;
        uiAction: string;
        htmlStrategy: IHtmlStrategy;

        lookUpDictionary: collections.Dictionary<string, any>;

        constructor(schema: any, name: string, instanceName: string, controllerUrl: string, uiAction: string, method: string, htmlStrategy: IHtmlStrategy) {
            this.schema = schema;
            this.instanceName = instanceName;
            this.header = new Header();
            this.header.name = name;
            this.controllerUrl = controllerUrl;
            this.header.action = controllerUrl + "Execute";
            this.uiAction = uiAction;
            this.header.method = method;
            this.footer = new Footer();
            this.htmlStrategy = htmlStrategy;

            this.fieldDictionary = new collections.Dictionary<string, Field>();

            this.populateFieldDictionary();
            
        }

        // Takes all the properties and fields of from the schema and creates a dictionary
        populateFieldDictionary() {
            let properties = this.schema.properties;
            let self = this;

            $.each(properties, (key, field) => {
                // First if tries to identify lookup fields by convention
                if (this.isPrimaryKey(key)) {
                    self.insertField(key, new Field(key, "hidden", "hidden"));
                }
                else if (this.isLookupField(key)) {
                    var lookupField = new Field(key, "options", "options");
                    self.insertField(key, lookupField);           
                } else if (field.type == 'array') {
                    //Todo: console.log(`TODO: Would create a daughter grid for the array of:${key}`);
                    $.each(field.items.properties, (k1, f1) => {
                        //console.log(k1);
                    });
                } else if (field.type == 'object') {
                    var titleField = new Field(key, "title", "title");
                    self.insertField(key, titleField);
                    //TODO: console.log(`TODO: Would create an embedded form for the object:${key}`);
                    $.each(field.properties, (k, f) => {
                        self.insertField(k,f);
                    });
                } else {
                    console.debug(`Processing: ${key}: ${field.type}`);
                    this.insertField(key, new Field(key, field.type, field.format));
                }
            });         
        }

        insertField(key: string, field: any) {
            this.fieldDictionary.setValue(key, new Field(key, field.type, field.format));
        }

        fieldDictionaryToArray(): Array<Field> {
            let array = new Array<Field>();

            let properties = this.schema.properties;
            $.each(properties, (key, field) => {
                var f = this.fieldDictionary.getValue(key);

                if (f != null) {
                    array.push(f);
                }
            });
            return array;
        }
        // Try to bind the instance to the form we target with the 'name' property in our constructor
        bindToForm(instance: T) {
            $.each(instance, (key, value) => {
                if (instance.hasOwnProperty(key)) {
                    var field = this.fieldDictionary.getValue(key);
                    if (field != null && field.format != null && field.format == "date-time") {
                        var date = new Date(parseInt(value.substr(6)));
                        var d = date.getDate();
                        var m = date.getMonth() + 1;
                        var y = date.getFullYear();
                        value = `${m}/${d}/${y}`;
                    }

                    var inputName = `[name="${this.prefixFieldName(key) }"]`;
                    $(inputName).val(value);
                    $(`[data-bind="${key}"]`).html(value);
                }
            });
        }

        //Used in the bindToForm method to populate a form so the Scaffold controller can bind it
        prefixFieldName(key: string) : string {
            return `WebRequest.Instance.${key}`;
        }

        //#region Lookup Helpers

        isLookupField(key: string): boolean {
            return (this.endsWithId(key) && key.toLowerCase().slice(0, -2) != this.instanceName.toLowerCase());
        }

        isPrimaryKey(key: string): boolean {
            return (key.toLowerCase().slice(0, -2) == this.instanceName.toLowerCase());
        }

        endsWithId(source: string) {
            var subject = source.slice(-2);
            return subject.toLowerCase() == "id";
        }
        //#endregion
    }

    export class Header {
        name: string;
        action: string;
        method: string;

        toHtml(extraClasses: string) {
            return `<form class=\"form-horizontal ${extraClasses}\" role="form" name="${this.name}" action="${this.action}" method="${this.method}">`; 
        }
    }

    export class Button {
        type: string;
        value: string;
        onClick: Function;
    }

    export class Footer {
        toHtml(formName: string) {
            return `</form> <!-- End form: "${formName}"-->`;
        }
    }

    export class Field {
        name: string;
        type: any;
        format: string;

        constructor(name: string, type: any, format: string) {
            this.name = name;
            this.type = type;
            this.format = format;
        }
    }
    
    export interface IHtmlStrategy {
        labelHtml(field: Field, extraClasses: string) : string;
        inputHtml(field: Field, extraClasses: string): string;

        groupHtml(label: string, input: string): string;

        formatInputPrepend(field: Field): string;
        formatInputAppend(field: Field): string;

        makeTable(data: any, domainName: string) : any;

        wireSubmit(formName: string): void;
        submitForm(formName: string): void;
    }

    export class BaseHtmlStrategy implements IHtmlStrategy {

        labelHtml(field: Field, extraClasses: string): string {
            return `<label for="${field.name}"><small>${field.name.replace(/([a-z])([A-Z])/g, "$1 $2") }</small></label>`;
        }

        inputHtml(field: Field, extraClasses: string): string {
            let inputType = "text";

            switch(field.type) {
                default:
                    return `<input type="${inputType}" id="${field.name}" name="WebRequest.Instance.${field.name}" />`;
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

        wireSubmit(formName: string): void {
            var form = $(`[name="${formName}"]`);
            form.submit((event) => {
                event.preventDefault();
                
                this.submitForm(formName);
            });
        }

        submitForm(formName: string): void {
            
            var form = $(`[name="${formName}"]`);

            $.ajax({
                type: "POST",
                url: form.attr('action'),
                data: form.serialize(),
                success: (response) => {
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

                    } else {
                        toastr[responseType](null, response.Message);
                    }
                }
            });
        }
        makeTable = (data: any, domainName: string): any => {
            var msg = "Error: No table generation code for Base html strategy. See sharpbox.Web.Form.ts line 254(ish).";
            console.log(msg);
            alert(msg);
        }

        // Assume that you have a property "FooId" and it's *not* the primary key. We assume this is a lookup value to populate a tag or select field
        // We want to grab the lookup data from that controllers cached method
        populateDropdown(key: string, callback: Function) {
            var lookupName = key.slice(0, -2);
            $.getJSON(`/${lookupName}/GetAsLookUpDictionary/`, data => {
                let lookupData = [];
                $.each(data, (key, item) => {
                    lookupData.push({ key, item });
                });
            }).done(data => {
                callback(data);
            });
        }
    }

    export class BootstrapHtmlStrategy extends BaseHtmlStrategy {
        labelHtml(field: Field, extraClasses: string): string {
            return `<label class="control-label col-sm-2 ${extraClasses}" for="${field.name}"><small>${field.name.replace(/([a-z])([A-Z])/g, "$1 $2") }</small></label>`;
        }

        inputHtml(field: Field, extraClasses: string): string {
            let inputType = "text";
            switch (field.format) {
                case "hidden":
                    return `<div class="col-sm-10">
                                <strong><span data-bind="${field.name}"></span></strong>
                                <input type="hidden" name="WebRequest.Instance.${field.name}" />
                            </div>
                            `;
                case "date-time":
                    return `
                            <div class="col-sm-10">
                                <div class="input-group">
                                    ${this.formatInputPrepend(field)}
                                    <input type="${inputType}" class="form-control ${extraClasses}" id="${field.name}" name="WebRequest.Instance.${field.name}" />
                                    ${this.formatInputAppend(field)}
                                </div>
                            </div>
                            `;
                case "options":
                    var name = `WebRequest.Instance.${field.name}`;
                    var options = "";
                    var optData = {};
                    this.populateDropdown(field.name, (data) => {
                        optData = data;
                        $.each(optData, (key, value) => {
                            options += `<option value="${key}">${value}</option>`;
                        });
                        $(`select[name="${name}"]`).append(options);
                        console.debug("There is a likely race condition on line 325(ish) of sharpbox.web.Form.ts. We assume that the form will always render before we get our options back!");
                    });
                    
                    return `<div class="col-sm-10">
                                    <select class="form-control ${extraClasses}" id="${field.name}" name="${name}">
                                        ${options}
                                    </select>
                            </div>
                            `;
                default:
                    return `<div class="col-sm-10">
                                ${this.formatInputPrepend(field) }<input type="${inputType}" class="form-control ${extraClasses}" id="${field.name}" name="WebRequest.Instance.${field.name}" />${this.formatInputAppend(field) }
                            </div>
                            `;
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
            switch (field.format) {
                case "date-time":
                    return "";
                default:
                    return "";
            }
        }

        formatInputAppend(field: Field): string {
            switch (field.format) {
                case "date-time":
                    return "<span class=\"input-group-addon\"><i class=\"glyphicon glyphicon-calendar\"></i></span>";
                default:
                    return "";
            }
        }

        makeTable = (data: any, domainName: string): any => {
            var table = $("<table class=\"table table-striped\">");
            var caption = $(`<caption><div class=\"btn-group pull-right\"><a href="/${domainName}/Detail">Add</a></div></caption>`);

            $(caption).appendTo(table);

            var tblHeader = "<tr>";
            var object = data[0];
            for (let k in object) {
                if (object.hasOwnProperty(k)) {
                    if (k == `${domainName}Id`) {
                        tblHeader += `<th>Action(s)</th>`;
                    } else {

                        tblHeader += `<th>${k}</th>`;
                    }
                }
            }
            tblHeader += "</tr>";
            $(tblHeader).appendTo(table);
            $.each(data, (index, value) => {

                var tableRow = "<tr>";
                $.each(value, (key, val) => {
                    if (key == `${domainName}Id`) {
                        tableRow += `<td><a href="/${domainName}/Detail/?id=${val}" class="btn btn-sm btn-info">Edit</a></td>`;
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