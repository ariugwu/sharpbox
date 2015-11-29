/// <reference path="sharpbox.poco.d.ts"/>
/// <reference path="sharpbox.domain.ts"/>
/// <reference path="sharpbox.Web.Form.ts"/>
/// <reference path="Typings/jquery.d.ts"/>

module sharpbox.Web {
    export class ViewModel<T> {
        pageArgs: sharpbox.Web.PageArgs;
        instance: T;
        instanceName: string;
        collection: Array<T>;
        controllerUrl: string;
        schema: any;

        form: Form<T>;

        createInputOverrideMap: { [id: string]: any }

        onSchemaLoad: Function;

        constructor(instanceName: string) {

            this.instanceName = instanceName;
            this.controllerUrl = `/${instanceName}/`;
            
        }

        getAll(callback: Function) {
            const url = this.controllerUrl + "Get/";
            $.getJSON(url, { _: new Date().getTime() }).done(data => {
                callback(data);
            });
        }

        getById(id: string, callback: Function) {
            const url = this.controllerUrl + "GetById/?id=" + id;
            $.get(url, data => {
                this.instance = data;
            }).done(data => {
                callback();
            });
        }

        getSchema(onSchemaLoad: Function) {
            const url = this.controllerUrl + "JsonSchema/";
            $.getJSON(url, data => {
                this.schema = JSON.parse(data);
            }).done(data => {
                onSchemaLoad();
            });
        }

        execute(action: string) {
            var self = this;
            const url = this.controllerUrl + "Execute/";
            let webRequest = {};
            webRequest = {
                UiAction: action,
                Instance: this.instance
            }

            $.post(url, webRequest, data => {
                self.processWebResponse(data);
            }, "json");
        }

        processWebResponse(webResponse: any) {

        }

        setProperty(name: string, value: string) {
            this.instance[name] = value;
        }        
    }
}