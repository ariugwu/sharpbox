/// <reference path="sharpbox.poco.d.ts"/>
/// <reference path="sharpbox.domain.ts"/>
/// <reference path="Typings/jquery.d.ts"/>

module sharpbox.ViewModel {
    export class Model<T> {
        instance: T;
        collection: Array<T>;

        controllerUrl: string;

        constructor(instanceName: string) {
            this.controllerUrl = "./" + instanceName + "/";
        }

        getAll() {
            var url = this.controllerUrl + "Get/";
            $.get(url, function(data) {
                this.collection = data;
            });
        }

        getById(id: number) {
            var url = this.controllerUrl + "GetById/" + id;
            $.get(url, function (data) {
                this.instance = data;
            });
        }

        execute(action: string) {
            var url = this.controllerUrl + "Execute/";
            var webRequest = new sharpbox.WebLibrary.Core.WebRequest<T>();

             webRequest = {
                UiAction: action,
                Instance: this.instance
            }

            $.post(url, webRequest, function (webResponse : sharpbox.WebLibrary.Core.WebResponse<T>) {
                this.processWebResponse(webResponse);
            }, 'json');
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

var test = new sharpbox.ViewModel.Model<sharpbox.Membership.Model.UserRole>("UserRole");

test.instance = { Name: "Administrator", UserRoleNameId: 0};

var wat : number = test.instance.UserRoleNameId;

test.execute(sharpbox.Domain.TestController.Command.Add);