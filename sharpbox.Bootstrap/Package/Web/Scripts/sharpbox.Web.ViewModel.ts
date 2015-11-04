/// <reference path="sharpbox.poco.d.ts"/>
/// <reference path="sharpbox.domain.ts"/>
/// <reference path="sharpbox.Web.Form.ts"/>
/// <reference path="Typings/jquery.d.ts"/>

module sharpbox.Web {
    export class ViewModel<T> {
        instance: T;
        collection: Array<T>;
        controllerUrl: string;
        schema: any;

        updateForm: UpdateForm<T>;

        createInputOverrideMap: { [id: string]: any }

        constructor(instanceName: string) {
            this.controllerUrl = `/${instanceName}/`;
        }

        getAll() {
            const url = this.controllerUrl + "Get/";
            $.get(url, data => {
                this.collection = data;
            });
        }

        getById(id: string) {
            const url = this.controllerUrl + "GetBySharpId/" + id;
            $.get(url, data => {
                this.instance = data;
            });
        }

        getSchema() {
            const url = this.controllerUrl + "JsonSchema/";
            $.getJSON(url, data => {
                this.schema = JSON.parse(data);
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

        bindToForm(formName: string) {
            var form = $(formName);
            for (var i; i < this.instance; i++) {

            }
        }

        setProperty(name: string, value: string) {
            this.instance[name] = value;
        }        
    }
}

var test = new sharpbox.Web.ViewModel<sharpbox.App.Model.Environment>("Environment");

test.instance = {
    EnvironmentId: 0,
    ApplicationName: "Sample Application",
    BaseUrl: "http://example.com",
    CacheKey: "any",
    UploadDirectory: "~/Uploads/",
    LogoLocation: "~/Images/Logo",
    BrandTypeId: 1,
    BrandType: null,
    EnvironmentTypeId: 1,
    EnvironmentType: null,
    TechSheetId: 1,
    TechSheet: null,
    SharpId: null
};

var wat: number = test.instance.BrandTypeId;

test.execute(sharpbox.Domain.TestController.Command.Update);