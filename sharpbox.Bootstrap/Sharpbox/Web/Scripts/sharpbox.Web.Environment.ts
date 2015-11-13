/// <reference path="sharpbox.poco.d.ts"/>
/// <reference path="sharpbox.domain.ts"/>
/// <reference path="Typings/jquery.d.ts"/>

module sharpbox.Web {
    export class Site {
        environment: sharpbox.App.Model.Environment;

        loadEnvironmentById(id: any, callback: Function): void {
            $.getJSON(`/Environment/GetById?id=${id}`, (data) => {
                this.environment = data;
                callback();
            });
        }
    }
}