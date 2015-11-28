/// <reference path="sharpbox.poco.d.ts"/>
/// <reference path="sharpbox.domain.ts"/>
/// <reference path="sharpbox.Web.Form.ts"/>
/// <reference path="Typings/jquery.d.ts"/>

module sharpbox.Web {
    export class ViewModel<T> {
        pageArgs: sharpbox.Web.PageArgs;
        lookUpDictionary: collections.Dictionary<string, any>;
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
            this.lookUpDictionary = new collections.Dictionary<string, any>();
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

        // Assume that you have a property "FooId" and it's *not* the primary key. We assume this is a lookup value to populate a tag or select field
        // We want to grab the lookup data from that controllers cached method
        getPropertyDataForLookup(lookupName: string, callback: Function) {
            const url = `/${lookupName}/GetAsLookUpDictionary/`;
            $.getJSON(url, data => {
                let lookupData = [];
                $.each(data, (key, item) => {
                    lookupData.push({ key, item });
                });
                this.lookUpDictionary.setValue(lookupName, data);
            }).done(data => {
                callback();
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